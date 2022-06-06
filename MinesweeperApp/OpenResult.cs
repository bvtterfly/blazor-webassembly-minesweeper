namespace MinesweeperApp;

public enum OpenResultCellType
{
    Cell,
    FlaggedCell,
    Mine
}

public readonly record struct OpenResult(OpenResultCellType Type, int? NeighborMines);

