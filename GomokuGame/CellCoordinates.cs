namespace GomokuGame
{
    public struct CellCoordinates
    {
        public CellCoordinates(int i, int j)
        {
            I = i;
            J = j;
        }
        
        public readonly int I;
        public readonly int J;

        public CellCoordinates WithOffset(int di, int dj)
        {
            int newI = I + di;
            int newJ = J + dj;

            return new CellCoordinates(newI, newJ);
        }

        public override string ToString()
        {
            return $"({I}, {J})";
        }
    }
}
