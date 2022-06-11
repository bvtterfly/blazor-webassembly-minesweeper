namespace MinesweeperApp;

public readonly record struct OpenResult(OpenResultCellType Type, int? NeighborMines);