namespace MinesweeperApp;

public enum DisplayResultCellType
{
    OpenedCell,
    Cell,
    FlaggedCell,
    Mine
}

public readonly record struct DisplayResult(DisplayResultCellType Type, int? NeighborMines);

