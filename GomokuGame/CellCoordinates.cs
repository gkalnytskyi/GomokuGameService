namespace GomokuGame
{
    public struct CellCoordinates
    {
        public CellCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }
        
        public readonly int Row { get; init; }
        public readonly int Column { get; init; }

        public CellCoordinates WithOffset(int di, int dj)
        {
            int newRow = Row + di;
            int newCol = Column + dj;

            return new CellCoordinates(newRow, newCol);
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }
    }
}
