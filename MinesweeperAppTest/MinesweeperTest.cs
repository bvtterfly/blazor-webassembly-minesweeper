using MinesweeperApp;
namespace MinesweeperAppTest;

public class MinesweeperTest
{
    [Fact]
    public void Create_NewMinesweeperGameWithMineShouldHavePlayingStatus()
    {
        // Arrange
        var expectedMinesCount = 4;
        var expectedRows = 5;
        var expectedCols = 5;
        var expectedStatus = GameStatus.Playing;
        var mines = new HashSet<Position>
        {
            new Position(0, 0),
            new Position(1, 2),
            new Position(3, 4),
            new Position(2, 0)
        };

        // Act

        var game = new Minesweeper(5, 5, mines);

        //Assert
        Assert.Equal(expectedMinesCount, game.MinesCount);
        Assert.Equal(expectedStatus, game.Status);
        Assert.Equal(expectedRows, game.Rows);
        Assert.Equal(expectedCols, game.Cols);

    }

    [Fact]
    public void Create_NewMinesweeperGameWithoutMineShouldHaveCreatedStatus()
    {
        // Arrange
        var mines = 9;
        var expectedGameStatus = GameStatus.Created;


        // Act
        var game = new Minesweeper(5, 5, mines);


        //Assert  
        Assert.Equal(expectedGameStatus, game.Status);

    }

    [Fact]
    public void Open_ACellShouldChangeTheStatusToPlaying()
    {
        // Arrange
        var mines = 9;
        var expectedGameStatus = GameStatus.Playing;


        // Act
        var game = new Minesweeper(5, 5, mines);
        game.Open(new Position(0, 0));


        //Assert  
        Assert.Equal(expectedGameStatus, game.Status);

    }

    [Fact]
    public void Open_AnEmptyCellShouldReturnNeighborsMines()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var emptyCell = new Position(0, 1);
        var expectedOpenResult = new OpenResult
        {
            NeighborMines = 2,
            Type = OpenResultCellType.Cell
        };
        var expectedGameStatus = GameStatus.Playing;


        // Act

        var result = game.Open(emptyCell);


        //Assert
        Assert.Equal(expectedOpenResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void Open_AMineCellShouldChangeStatusToGameOver()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var mineCell = new Position(0, 0);
        var expectedOpenResult = new OpenResult
        {
            Type = OpenResultCellType.Mine
        };
        var expectedGameStatus = GameStatus.GameOver;

        // Act

        var result = game.Open(mineCell);


        //Assert
        Assert.Equal(expectedOpenResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void ToggleFlagCell_ACellShouldFlagIt()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var toggleCell = new Position(0, 0);
        var expectedGameStatus = GameStatus.Playing;
        var expectedOpenResult = new OpenResult
        {
            Type = OpenResultCellType.FlaggedCell
        };

        // Act

        game.ToggleFlagCell(toggleCell);
        var result = game.Open(toggleCell);


        //Assert
        Assert.Equal(expectedOpenResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void ToggleFlagCell_AnOpenedCellShouldNotFlagged()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var toggleCell = new Position(0, 1);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.OpenedCell,
            NeighborMines = 2
        };

        // Act

        game.Open(toggleCell);
        game.ToggleFlagCell(toggleCell);
        var result = game.Get(toggleCell);

        //Assert
        Assert.Equal(expectedDisplayResult, result);
    }

    [Fact]
    public void ToggleFlagCell_ACellAgainShouldToggleCellType()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var toggleCell = new Position(0, 1);
        var expectedGameStatus = GameStatus.Playing;
        var expectedOpenResult = new OpenResult
        {
            NeighborMines = 2,
            Type = OpenResultCellType.Cell
        };

        // Act

        game.ToggleFlagCell(toggleCell);
        game.ToggleFlagCell(toggleCell);
        var result = game.Open(toggleCell);


        //Assert
        Assert.Equal(expectedOpenResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void Get_AClosedEmptyCellShouldReturnCellType()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var emptyCell = new Position(0, 1);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.Cell
        };

        // Act

        var result = game.Get(emptyCell);


        //Assert
        Assert.Equal(expectedDisplayResult, result);
    }

    [Fact]
    public void Get_AnOpenedEmptyCellShouldReturnNeighborMines()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var emptyCell = new Position(0, 1);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.OpenedCell,
            NeighborMines = 2,

        };

        // Act
        game.Open(emptyCell);
        var result = game.Get(emptyCell);


        //Assert
        Assert.Equal(expectedDisplayResult, result);

    }

    [Fact]
    public void Get_AClosedMineCellShouldReturnCellType()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var mineCell = new Position(0, 0);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.Cell
        };

        // Act
        var result = game.Get(mineCell);


        //Assert
        Assert.Equal(expectedDisplayResult, result);
    }

    [Fact]
    public void Get_AnOpenedMineCellShouldReturnMineType()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var mineCell = new Position(0, 0);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.Mine
        };
        var expectedGameStatus = GameStatus.GameOver;

        // Act
        game.Open(mineCell);
        var result = game.Get(mineCell);


        //Assert
        Assert.Equal(expectedDisplayResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void Get_AFlaggedCellShouldReturnFlaggedCellType()
    {
        // Arrange
        var game = CreateAMinesweeper();
        var flagCell = new Position(0, 0);
        var expectedDisplayResult = new DisplayResult
        {
            Type = DisplayResultCellType.FlaggedCell
        };
        var expectedGameStatus = GameStatus.Playing;

        // Act
        game.ToggleFlagCell(flagCell);
        var result = game.Get(flagCell);


        //Assert
        Assert.Equal(expectedDisplayResult, result);
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void Open_EveryEmptyCellsShouldChangeStatusToWon()
    {
        // Arrange
        var mines = new HashSet<Position>
        {
            new Position(0, 0),
            new Position(2, 0)
        };
        var game = new Minesweeper(5, 5, mines);
        var expectedGameStatus = GameStatus.Won;

        // Act
        for (var row = 0; row < 5; row++)
        {
            for (var col = 0; col < 5; col++)
            {
                var pos = new Position(row, col);
                if (mines.Contains(pos))
                {
                    continue;
                }
                game.Open(new Position(row, col));

            }
        }


        //Assert
        Assert.Equal(expectedGameStatus, game.Status);
    }

    [Fact]
    public void ToggleFlagCell_AllMinesShouldChangeStatusToWon()
    {
        // Arrange
        var mines = new HashSet<Position>
        {
            new Position(0, 0),
            new Position(1, 2),
        };
        var game = new Minesweeper(5, 5, mines);
        var expectedGameStatus = GameStatus.Won;

        // Act
        foreach (var minePos in mines.ToList())
        {
            game.ToggleFlagCell(minePos);
        }


        //Assert
        Assert.Equal(expectedGameStatus, game.Status);
    }

    private static Minesweeper CreateAMinesweeper()
    {
        var mines = new HashSet<Position>
        {
            new Position(0, 0),
            new Position(1, 2),
            new Position(3, 4),
            new Position(2, 0)
        };
        return new Minesweeper(5, 5, mines);
    }
}

