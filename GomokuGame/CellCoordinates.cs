namespace GomokuGame
{
    public struct CellCoordinates
    {
        public CellCoordinates(int row, int column)
        {
            Row = row;
            Column = column;
        }
        
        public readonly int Row;
        public readonly int Column;

        public CellCoordinates WithOffset(int di, int dj)
        {
            int newI = Row + di;
            int newJ = Column + dj;

            return new CellCoordinates(newI, newJ);
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }
    }
}
