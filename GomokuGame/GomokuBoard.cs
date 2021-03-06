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


        /// <summary>
        /// Creates a new board for a game of Gomoku with <paramref name="nrow"/> rows and <paramref name="ncol"/> columns,
        /// with winning sequence length of <paramref name="winCount"/>.
        /// Caller can provide initial state of cells in <paramref name="initBoardState"/>.
        /// </summary>
        /// <param name="nrow">Number of rows on the board.</param>
        /// <param name="ncol">Number of columns on the board.</param>
        /// <param name="winCount">Number of stones of the same colour treated as winning sequence.</param>
        /// <param name="initBoardState">Initial state of cells on the board. Should be the same length as nrow * ncol.</param>
        /// <remarks>If Parameter <paramref name="initBoardState"/> is not passed the new blank array
        /// of size nrow * ncol is created.</remarks>
        public GomokuBoard(int nrow, int ncol, int winCount, CellState[] initBoardState = null)
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
                throw new ArgumentException("Winning row length should be at least 2 stones", nameof(winCount));
            }
            else if (winCount > ncol || winCount > nrow)
            {
                throw new ArgumentException(
                    "Winning row length should not exceed width of height of the board",
                    nameof(winCount));
            }

            if ((initBoardState != null) && (initBoardState.Length != ncol * nrow))
            {
                throw new ArgumentException(
                    $"Provided board does not match expected {ncol} by {nrow} board.", nameof(initBoardState));
            }

            if (initBoardState == null)
            {
                initBoardState = new CellState[ncol * nrow];
            }

            BoardColumns = ncol;
            BoardRows = nrow;
            WinCount = winCount;
            Board = initBoardState;
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
                        case CellState.Empty:
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

        public string[] ToStringPerRow()
        {
            var strArray = new string[BoardRows];
            var sb = new StringBuilder();

            for (int i = 0; i < BoardRows; i++)
            {
                sb.Clear();
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
                        case CellState.Empty:
                            sb.Append('_');
                            break;
                        default:
                            throw new ArgumentException("Unknown cell type");
                    }
                }

                strArray[i] = sb.ToString();
            }

            return strArray;
        }

        public void Reset()
        {
            EmptyCells = BoardSize;
            Array.Fill(Board, CellState.Empty);
        }

        public bool IsOnTheBoard(CellCoordinates cell)
        {
            return (cell.Row >= 0 && cell.Row < BoardRows) &&
                (cell.Column >= 0 && cell.Row < BoardColumns);
        }

        public bool CanPlaceStone(CellCoordinates cell)
        {
            return this[cell] == CellState.Empty;
        }

        public bool AreMoreMovesAvailable()
        {
            return EmptyCells > 0;
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

            if (!CanPlaceStone(cell))
            {
                throw new GomokuGameException("Cannot place a stone to an occupied cell.");
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
            
            return cell.Row * BoardColumns + cell.Column;
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
                        case CellState.Empty:
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
            new(cell.Row, Math.Max(0, cell.Column - WinCount + 1));

        private CellCoordinates RelativeRightMost(CellCoordinates cell) =>
            new(cell.Row, Math.Min(cell.Column + WinCount - 1, BoardColumns - 1));

        private CellCoordinates RelativeTopMost(CellCoordinates cell) =>
            new(Math.Max(0, cell.Row - WinCount + 1), cell.Column);

        private CellCoordinates RelativeBottomMost(CellCoordinates cell) =>
            new(Math.Min(cell.Row + WinCount - 1, BoardRows - 1), cell.Column);

        private static int CellDistance(CellCoordinates cellA, CellCoordinates cellB)
        {
            int di = Math.Abs(cellA.Row - cellB.Row);
            int dj = Math.Abs(cellA.Column - cellB.Column);

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
                if (board[i] == CellState.Empty)
                {
                    uCells++;
                }
            }

            return uCells;
        }
    }
}
