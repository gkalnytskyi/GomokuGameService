using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GomokuGame;
using NUnit.Framework;

namespace GomokuGameTest
{
    [TestFixture]
    public class GomokuGameTest
    {
        [Test]
        public void WhenPlacingStoneOnOccupiedCellThrowsException()
        {
            var sut = new GomokuGame.GomokuGame(new GomokuBoard(4, 4, 2));

            sut.PlaceStone(new CellCoordinates(2, 3));

            Assert.Throws<GomokuGameException>(() => sut.PlaceStone(new CellCoordinates(2, 3)));
        }


        public static GomokuGameState[] EndStates = new[]
        {
            GomokuGameState.BlackWins,
            GomokuGameState.WhiteWins,
            GomokuGameState.Draw
        };

        [Test]
        [TestCaseSource(nameof(EndStates))]
        public void GivenGameEndedWhenPlacingStoneThrowsException(GomokuGameState endState)
        {
            var sut = new GomokuGame.GomokuGame(new GomokuBoard(4, 4, 2), endState);

            Assert.Throws<GomokuGameException>(() => sut.PlaceStone(new CellCoordinates(2, 3)));
        }

        public static object[] InitNextStates = new object[]
        {
            new GomokuGameState[] { GomokuGameState.BlackToMove, GomokuGameState.WhiteToMove },
            new GomokuGameState[] { GomokuGameState.WhiteToMove, GomokuGameState.BlackToMove}
        };

        [Test]
        [TestCaseSource(nameof(InitNextStates))]
        public void WhenPlaceStoneGameAndGameNotEndedItTrasitionsToAnotherPlayer(
            GomokuGameState initState, GomokuGameState nextState)
        {
            var sut = new GomokuGame.GomokuGame(new GomokuBoard(4, 4, 2), initState);

            sut.PlaceStone(new CellCoordinates(2, 3));

            Assert.False(sut.HasGameEnded);
            Assert.AreEqual(sut.CurrentState, nextState);
        }

        [Test]
        public void WhenBlackPlaceWinningStoneBlackWinsGameEnds()
        {
            var initBoardState = Enumerable.Repeat(CellState.White, 2).
                Concat(Enumerable.Repeat(CellState.Empty, 12)).
                Concat(Enumerable.Repeat(CellState.Black, 2)).
                ToArray();
            
            var sut = new GomokuGame.GomokuGame(new GomokuBoard(4, 4, 3, initBoardState), GomokuGameState.BlackToMove);

            sut.PlaceStone(new CellCoordinates(3, 1));

            Assert.True(sut.HasGameEnded);
            Assert.AreEqual(sut.CurrentState, GomokuGameState.BlackWins);
        }

        [Test]
        public void WhenWhitePlaceWinningStoneWhiteWinsGameEnds()
        {
            var initBoardState = Enumerable.Repeat(CellState.White, 2).
                Concat(Enumerable.Repeat(CellState.Empty, 12)).
                Concat(Enumerable.Repeat(CellState.Black, 2)).
                ToArray();

            var sut = new GomokuGame.GomokuGame(new GomokuBoard(4, 4, 3, initBoardState), GomokuGameState.WhiteToMove);

            sut.PlaceStone(new CellCoordinates(0, 2));

            Assert.True(sut.HasGameEnded);
            Assert.AreEqual(sut.CurrentState, GomokuGameState.WhiteWins);
        }
    }
}
