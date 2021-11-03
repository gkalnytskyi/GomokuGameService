using System;
using System.Collections.Generic;
using System.Linq;

using GomokuGame;
using NUnit.Framework;

namespace GomokuGameTest
{
    public class GomokuBoardTest
    {
        [Test]
        public void CheckStringRepresentation()
        {
            var board =
                Enumerable.Repeat(CellState.Black, 1).Append(CellState.Unoccupied).Concat(Enumerable.Repeat(CellState.White, 2)).
                Concat(Enumerable.Repeat(CellState.Black, 3)).Append(CellState.Unoccupied).
                Concat(Enumerable.Repeat(CellState.White, 4)).
                Append(CellState.White).Concat(Enumerable.Repeat(CellState.Black, 3)).ToArray();

            var sut = new GomokuBoard(4, 4, 2, board);

            var actualStrImg = sut.ToString();
            var expectedStrImg = "X_OO" + Environment.NewLine +
                                 "XXX_" + Environment.NewLine +
                                 "OOOO" + Environment.NewLine +
                                 "OXXX" + Environment.NewLine;
            Assert.That(actualStrImg == expectedStrImg);
        }

        public static readonly CellCoordinates[] horizontalWin = new[]
        {
            new CellCoordinates(0, 0),
            new CellCoordinates(2, 2),
            new CellCoordinates(2, 3),
            new CellCoordinates(5, 5)
        };

        public static IEnumerable<object[]> HorizontalCases()
        {
            var cellStates = new CellState[] { CellState.Black, CellState.White };

            foreach (var cellState in cellStates)
            {
                foreach (var coordinates in horizontalWin)
                {
                    yield return new object[] { cellState, coordinates };
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(HorizontalCases))]
        public void CheckHorizontalWinningPositions(CellState color, CellCoordinates coords)
        {
            var board =
                Enumerable.Repeat(color, 4).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 6)).
                Append(CellState.Unoccupied).Concat(Enumerable.Repeat(color, 4)).Append(CellState.Unoccupied).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 6)).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 6)).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Concat(Enumerable.Repeat(color, 4)).ToArray();

            var gomokuBoard = new GomokuBoard(6, 6, 4, board);

            Assert.IsTrue(gomokuBoard.IsPartOfWinningSequence(coords));
        }

        public static readonly CellCoordinates[] verticalWin = new[]
        {
            new CellCoordinates(0, 0),
            new CellCoordinates(2, 2),
            new CellCoordinates(3, 2),
            new CellCoordinates(5, 5)
        };

        public static IEnumerable<object[]> VerticalCases()
        {
            var cellStates = new CellState[] { CellState.Black, CellState.White };

            foreach (var cellState in cellStates)
            {
                foreach (var coordinates in verticalWin)
                {
                    yield return new object[] { cellState, coordinates };
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(VerticalCases))]
        public void CheckVerticalWinningPositions(CellState color, CellCoordinates coords)
        {
            var board =
                Enumerable.Repeat(color, 1).Concat(Enumerable.Repeat(CellState.Unoccupied, 5)).
                Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 3)).
                Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).
                Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 5)).Append(color).ToArray();

            var gomokuBoard = new GomokuBoard(6, 6, 4, board);

            Assert.IsTrue(gomokuBoard.IsPartOfWinningSequence(coords));
        }

        public static readonly CellCoordinates[] primeDiagWin = new[]
        {
            new CellCoordinates(0, 0),
            new CellCoordinates(1, 3),
            new CellCoordinates(2, 4),
            new CellCoordinates(3, 5),
            new CellCoordinates(3, 3),
            new CellCoordinates(5, 5)
        };

        public static IEnumerable<object[]> PrimeDiagCases()
        {
            var cellStates = new CellState[] { CellState.Black, CellState.White };

            foreach (var cellState in cellStates)
            {
                foreach (var coordinates in primeDiagWin)
                {
                    yield return new object[] { cellState, coordinates };
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(PrimeDiagCases))]
        public void CheckPrimeDiagonalWinningPositions(CellState color, CellCoordinates coords)
        {
            var board =
                Enumerable.Repeat(color, 1).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 3)).
                Append(CellState.Unoccupied).Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).Append(CellState.Unoccupied).Append(color).Append(CellState.Unoccupied).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 3)).Append(color).Append(CellState.Unoccupied).Append(color).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 4)).Append(color).Append(CellState.Unoccupied).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 5)).Append(color).ToArray();

            var gomokuBoard = new GomokuBoard(6, 6, 4, board);

            Assert.IsTrue(gomokuBoard.IsPartOfWinningSequence(coords));
        }


        public static readonly CellCoordinates[] secondaryDiagWin = new[]
 {
            new CellCoordinates(0, 5),
            new CellCoordinates(0, 3),
            new CellCoordinates(1, 2),
            new CellCoordinates(2, 1),
            new CellCoordinates(3, 0),
            new CellCoordinates(5, 0)
        };

        public static IEnumerable<object[]> SecondaryDiagCases()
        {
            var cellStates = new CellState[] { CellState.Black, CellState.White };

            foreach (var cellState in cellStates)
            {
                foreach (var coordinates in secondaryDiagWin)
                {
                    yield return new object[] { cellState, coordinates };
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(SecondaryDiagCases))]
        public void CheckSecondaryDiagonalWinningPositions(CellState color, CellCoordinates coords)
        {
            var board =
                Enumerable.Repeat(CellState.Unoccupied, 3).Append(color).Append(CellState.Unoccupied).Append(color).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).Append(color).Append(CellState.Unoccupied).Append(color).Append(CellState.Unoccupied).
                Append(CellState.Unoccupied).Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 2)).
                Append(color).Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 3)).
                Append(CellState.Unoccupied).Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 4)).
                Append(color).Concat(Enumerable.Repeat(CellState.Unoccupied, 5)).ToArray();

            var gomokuBoard = new GomokuBoard(6, 6, 4, board);

            Assert.IsTrue(gomokuBoard.IsPartOfWinningSequence(coords));
        }


        [Test]
        public void NoWinningPositionFound(
            [Values(0, 1, 2, 3, 4, 5)] int i,
            [Values(0, 1, 2, 3, 4, 5)] int j)
        {
            var coords = new CellCoordinates(i, j);
            var board =
                Enumerable.Repeat(CellState.White, 1).Append(CellState.Black).Concat(Enumerable.Repeat(CellState.White, 2)).Concat(Enumerable.Repeat(CellState.Black, 2)).
                Append(CellState.Black).Append(CellState.White).Append(CellState.Black).Append(CellState.White).Append(CellState.Black).Append(CellState.White).
                Append(CellState.White).Append(CellState.Black).Concat(Enumerable.Repeat(CellState.White, 3)).Append(CellState.Black).
                Concat(Enumerable.Repeat(CellState.White, 2)).Concat(Enumerable.Repeat(CellState.Black, 2)).Append(CellState.White).Append(CellState.Black).
                Concat(Enumerable.Repeat(CellState.Black, 3)).Concat(Enumerable.Repeat(CellState.White, 3)).
                Append(CellState.White).Append(CellState.Black).Append(CellState.White).Concat(Enumerable.Repeat(CellState.Black, 2)).Append(CellState.White).
                ToArray();

            var gomokuBoard = new GomokuBoard(6, 6, 4, board);

            Assert.IsFalse(gomokuBoard.IsPartOfWinningSequence(coords));
        }

        public static IEnumerable<object[]> PlaceStoneCases()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    foreach (var player in Enum.GetValues<Player>())
                    {
                        var stoneColor = (player == Player.Black) ? CellState.Black : CellState.White;
                        yield return new object[] { new CellCoordinates(i, j), player, stoneColor };
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(PlaceStoneCases))]
        public void PlacingStoneUpdatesTheBoard(CellCoordinates cell, Player player, CellState stoneColor)
        {
            var board = new GomokuBoard(4, 4, 2);

            board.PlaceStone(player, cell);
            Assert.AreEqual(board[cell], stoneColor);
        }

        public static IEnumerable<object[]> OccupiedCellCases()
        {
            yield return new object[] { Player.Black, new CellCoordinates(1, 1) };
            yield return new object[] { Player.White, new CellCoordinates(1, 1) };
            yield return new object[] { Player.Black, new CellCoordinates(1, 2) };
            yield return new object[] { Player.White, new CellCoordinates(1, 2) };
        }

        [Test]
        [TestCaseSource(nameof(OccupiedCellCases))]
        public void PlacingStoneInOccupiedCellThrowsException(Player player, CellCoordinates cell)
        {
            var board = Enumerable.Repeat(CellState.Unoccupied, 4).
                Append(CellState.Unoccupied).Append(CellState.Black).Append(CellState.White).Append(CellState.Unoccupied).
                Concat(Enumerable.Repeat(CellState.Unoccupied, 8)).ToArray();

            var gomokuBoard = new GomokuBoard(4, 4, 2, board);

            Assert.Throws<GomokuGameException>(() => gomokuBoard.PlaceStone(player, cell));
        }
    }
}
