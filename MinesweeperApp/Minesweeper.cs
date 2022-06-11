namespace MinesweeperApp;

public class Minesweeper
{
    public int Rows { get; }
    public int Cols { get; }
    public int MinesCount { get; }

    private readonly HashSet<Position> _openCells = new();
    private readonly HashSet<Position> _mines = new();
    private readonly HashSet<Position> _flaggedCells = new();
    public GameStatus Status { get; private set; } = GameStatus.Created;

    public Minesweeper(int rows, int cols, int minesCount)
    {
        Rows = rows;
        Cols = cols;
        MinesCount = minesCount;
    }

    public Minesweeper(int rows, int cols, HashSet<Position> mines)
    {
        Rows = rows;
        Cols = cols;
        _mines = mines;
        MinesCount = mines.Count;
        Status = GameStatus.Playing;
    }

    private void InitMies(Position startPosition)
    {
        var rand = new Random();
        while (_mines.Count < MinesCount)
        {
            var pos = new Position
            {
                Row = rand.Next(Rows),
                Col = rand.Next(Cols)
            };
            if (startPosition != pos)
            {
                _mines.Add(pos);
            }
        }

        Status = GameStatus.Playing;
    }

    public DisplayResult Get(Position position)
    {
        if (_flaggedCells.Contains(position))
        {
            return new DisplayResult
            {
                Type = DisplayResultCellType.FlaggedCell
            };
        }

        if (_mines.Contains(position))
        {
            return new DisplayResult
            {
                Type = Status is GameStatus.GameOver or GameStatus.Won
                    ? DisplayResultCellType.Mine
                    : DisplayResultCellType.Cell
            };
        }

        if (!_openCells.Contains(position))
        {
            return new DisplayResult
            {
                Type = DisplayResultCellType.Cell
            };
        }


        var count = Neighbors(position).Count((Position p) => _mines.Contains(p));

        return new DisplayResult
        {
            Type = DisplayResultCellType.OpenedCell,
            NeighborMines = count
        };
    }

    private IEnumerable<Position> Neighbors(Position position)
    {
        var neighbors = new List<Position>();
        var row = Math.Max(position.Row, 1) - 1;
        while (row <= Math.Min(position.Row + 1, Rows - 1))
        {
            var col = Math.Max(position.Col, 1) - 1;
            while (col <= Math.Min(position.Col + 1, Cols - 1))
            {
                neighbors.Add(new Position
                {
                    Row = row,
                    Col = col
                });

                col++;
            }

            row++;
        }

        return neighbors.Where(n => n != position);
    }

    public void ToggleFlagCell(Position position)
    {
        if (_openCells.Contains(position))
        {
            return;
        }

        if (_flaggedCells.Contains(position))
        {
            _flaggedCells.Remove(position);
            return;
        }

        _flaggedCells.Add(position);
        if (_mines.SequenceEqual(_flaggedCells))
        {
            Status = GameStatus.Won;
        }
        else
        {
            CheckForWinning();
        }
    }

    public OpenResult Open(Position position)
    {
        if (Status.Equals(GameStatus.Created))
        {
            InitMies(position);
        }

        if (_flaggedCells.Contains(position))
        {
            return new OpenResult
            {
                Type = OpenResultCellType.FlaggedCell
            };
        }

        if (_mines.Contains(position))
        {
            Status = GameStatus.GameOver;
            _openCells.Add(position);
            return new OpenResult
            {
                Type = OpenResultCellType.Mine
            };
        }

        _openCells.Add(position);

        var neighbors = Neighbors(position);

        var count = Neighbors(position).Count(p => _mines.Contains(p));

        if (count != 0)
        {
            CheckForWinning();
            return new OpenResult
            {
                Type = OpenResultCellType.Cell,
                NeighborMines = count
            };
        }

        foreach (var neighbor in neighbors)
        {
            if (!_openCells.Contains(neighbor))
            {
                Open(neighbor);
            }
        }

        CheckForWinning();

        return new OpenResult
        {
            Type = OpenResultCellType.Cell,
            NeighborMines = count
        };
    }

    private void CheckForWinning()
    {
        var row = 0;
        while (row < Rows)
        {
            var col = 0;
            while (col < Cols)
            {
                var position = new Position(row, col);
                if (!_flaggedCells.Contains(position)
                    && !_openCells.Contains(position)
                    && !_mines.Contains(position))
                {
                    return;
                }

                col++;
            }

            row++;
        }

        Status = GameStatus.Won;
    }
}