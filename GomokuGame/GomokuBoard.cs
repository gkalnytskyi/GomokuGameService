using System;
using System.Collections.Generic;
using System.Text;

namespace GomokuGame
{
    public class GomokuBoard
    {
        private readonly int BoardRows;
        private readonly int BoardColumns;
        
        private int BoardSize => BoardRows * BoardColumns;

        private readonly int WinCount;

        private readonly CellState[] Board;
        private int EmptyCells;

        public GomokuBoard(int nrow, int ncol, int winCount, CellState[] boardState = null)
        {
            if (ncol < 1)
            {
                throw new ArgumentException("Board width is too small.", nameof(ncol));
            }

            if (nrow < 1)
            {
                throw new ArgumentException("Board height is too small.", nameof(nrow));
            }

            if (winCount < 2)
            {
                throw new ArgumentException("Winning row length should be more than 2 stones", nameof(winCount));
            }
            else if (winCount > ncol || winCount > nrow)
            {
                throw new ArgumentException(
                    "Winning row length should not exceed width of height of the board",
                    nameof(winCount));
            }

            if (boardState == null)
            {
                boardState = new CellState[ncol * nrow];
            }

            if (boardState.Length != ncol * nrow)
            {
                throw new ArgumentException(
                    $"Provided board does not match expected {ncol} by {nrow} board.", nameof(boardState));
            }

            BoardColumns = ncol;
            BoardRows = nrow;
            WinCount = winCount;
            Board = boardState;
            EmptyCells = CountUnoccupiedCells(Board);
        }

        public CellState this[int i, int j] => this[new CellCoordinates(i, j)];

        public CellState this[CellCoordinates cell]
        {
            get => Board[GetBoardIndex(cell)];
            private set => Board[GetBoardIndex(cell)] = value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < BoardRows; i++)
            {
                for (int j = 0; j < BoardColumns; j++)
                {
                    switch (Board[(i * BoardColumns) + j])
                    {
                        case CellState.Black:
                            sb.Append('X');
                            break;
                        case CellState.White:
                            sb.Append('O');
                            break;
                        case CellState.Unoccupied:
                            sb.Append('_');
                            break;
                        default:
                            throw new ArgumentException("Unknown cell type");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void Reset()
        {
            EmptyCells = BoardSize;
            Array.Fill(Board, CellState.Unoccupied);
        }

        public void PlaceStone(Player player, CellCoordinates cell)
        {
            if (EmptyCells == 0)
            {
                throw new GomokuGameException("No cells left to place a stone to.");
            }

            if (!IsOnTheBoard(cell))
            {
                throw new GomokuGameException("Cell coordinates are off the board.");
            }

            if (this[cell] != CellState.Unoccupied)
            {
                throw new GomokuGameException("Cannot place a stone to and occupied cell.");
            }

            switch (player)
            {
                case Player.Black:
                    this[cell] = CellState.Black;
                    break;
                case Player.White:
                    this[cell] = CellState.White;
                    break;
            }

            EmptyCells--;
        }

        public bool IsPartOfWinningSequence(CellCoordinates cell)
        {
            IsOnTheBoard(cell);

            return CheckHorizontal(cell) ||
                CheckVertical(cell) ||
                CheckPrimaryDiagonal(cell) ||
                CheckSecondaryDiagonal(cell);
        }

        private int GetBoardIndex(CellCoordinates cell)
        {
            if (!IsOnTheBoard(cell))
            {
                throw new GomokuGameException("Cell coordinates are off the board.");
            }
            
            return cell.I * BoardColumns + cell.J;
        }

        private bool IsOnTheBoard(CellCoordinates cell)
        {
            return (cell.I >= 0 && cell.I < BoardRows) &&
                (cell.J >= 0 && cell.I < BoardColumns);
        }

        private bool CheckHorizontal(CellCoordinates cell)
        {
            var leftmost = RelativeLeftMost(cell);
            var rightmost = RelativeRightMost(cell);

            var cellCount = CellDistance(rightmost, leftmost) + 1;

            var horizontalCells = new List<CellCoordinates>(cellCount);
            for (int i = 0; i < cellCount; i++)
            {
                horizontalCells.Add(leftmost.WithOffset(0, i));
            }

            return CheckForVictoryConditions(horizontalCells);
        }

        private bool CheckVertical(CellCoordinates cell)
        {
            var topmost = RelativeTopMost(cell);
            var bottommost = RelativeBottomMost(cell);

            var cellCount = CellDistance(bottommost, topmost) + 1;

            var verticalCells = new List<CellCoordinates>(cellCount);
            for (int i = 0; i < cellCount; i++)
            {
                verticalCells.Add(topmost.WithOffset(i, 0));
            }

            return CheckForVictoryConditions(verticalCells);
        }

        private bool CheckPrimaryDiagonal(CellCoordinates cell)
        {
            var leftmost = RelativeLeftMost(cell);
            var topmost = RelativeTopMost(cell);
            var diagDistNw = Math.Min(CellDistance(cell, leftmost), CellDistance(cell, topmost));
            var topLeft = cell.WithOffset(-diagDistNw, -diagDistNw);

            var rightmost = RelativeRightMost(cell);
            var bottommost = RelativeBottomMost(cell);
            var diagDistSe = Math.Min(CellDistance(cell, rightmost), CellDistance(cell, bottommost));
            var bottomRight = cell.WithOffset(diagDistSe, diagDistSe);

            var cellcount = CellDistance(topLeft, bottomRight) + 1;

            var primeDiagonalCells = new List<CellCoordinates>(cellcount);
            for (int i = 0; i < cellcount; i++)
            {
                primeDiagonalCells.Add(topLeft.WithOffset(i, i));
            }

            return CheckForVictoryConditions(primeDiagonalCells);
        }

        private bool CheckSecondaryDiagonal(CellCoordinates cell)
        {
            var leftmost = RelativeLeftMost(cell);
            var bottommost = RelativeBottomMost(cell);
            var diagDistSw = Math.Min(CellDistance(cell, leftmost), CellDistance(cell, bottommost));
            var bottomLeft = cell.WithOffset(diagDistSw, -diagDistSw);

            var rightmost = RelativeRightMost(cell);
            var topmost = RelativeTopMost(cell);
            var diagDistNe = Math.Min(CellDistance(cell, rightmost), CellDistance(cell, topmost));
            var topRight = cell.WithOffset(-diagDistNe, +diagDistNe);

            var cellcount = CellDistance(topRight, bottomLeft) + 1;

            var secondaryDiagonalCells = new List<CellCoordinates>(cellcount);
            for (int i = 0; i < cellcount; i++)
            {
                secondaryDiagonalCells.Add(bottomLeft.WithOffset(-i, i));
            }

            return CheckForVictoryConditions(secondaryDiagonalCells);
        }

        private bool CheckForVictoryConditions(List<CellCoordinates> cells)
        {
            if (cells.Count < WinCount)
            {
                return false;
            }

            for (int i = 0; i < cells.Count - WinCount + 1; i++)
            {
                var sum = 0;
                for (int k = 0; k < WinCount; k++)
                {
                    switch (this[cells[i + k]])
                    {
                        case CellState.Black:
                            sum += 1;
                            break;
                        case CellState.White:
                            sum += -1;
                            break;
                        case CellState.Unoccupied:
                            break;
                        default:
                            break;
                    }
                }

                if (Math.Abs(sum) == WinCount)
                {
                    return true;
                }
            }

            return false;
        }

        private CellCoordinates RelativeLeftMost(CellCoordinates cell) =>
            new(cell.I, Math.Max(0, cell.J - WinCount + 1));

        private CellCoordinates RelativeRightMost(CellCoordinates cell) =>
            new(cell.I, Math.Min(cell.J + WinCount - 1, BoardColumns - 1));

        private CellCoordinates RelativeTopMost(CellCoordinates cell) =>
            new(Math.Max(0, cell.I - WinCount + 1), cell.J);

        private CellCoordinates RelativeBottomMost(CellCoordinates cell) =>
            new(Math.Min(cell.I + WinCount - 1, BoardRows - 1), cell.J);

        private static int CellDistance(CellCoordinates cellA, CellCoordinates cellB)
        {
            int di = Math.Abs(cellA.I - cellB.I);
            int dj = Math.Abs(cellA.J - cellB.J);

            if (di == 0)
            {
                return dj;
            }
            else if (dj == 0)
            {
                return di;
            }
            else if (di == dj)
            {
                return di;
            }
            else
            {
                throw new GomokuGameException(
                    "Cannot calculate distance. Cells are not in a single row, column, or diagonal.");
            }
        }

        private static int CountUnoccupiedCells(CellState[] board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }
            
            int uCells = 0;

            for (var i = 0; i < board.Length; i++)
            {
                if (board[i] == CellState.Unoccupied)
                {
                    uCells++;
                }
            }

            return uCells;
        }
    }
}
