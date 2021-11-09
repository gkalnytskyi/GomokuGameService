using System;

namespace GomokuGame
{
    public interface IGame
    {
        void PlaceStone(CellCoordinates cell);
        bool HasGameEnded { get; }
    }
    
    public class GomokuGame : IGame
    {
        public GomokuBoard Board { get; private set; }
        private GomokuGameState CurrentState { get; set; }

        bool IGame.HasGameEnded => throw new NotImplementedException();

        public GomokuGame(GomokuBoard board) : this (board, GomokuGameState.BlackToMove)
        {
        }

        public GomokuGame(GomokuBoard board, GomokuGameState state)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            CurrentState = state;
        }

        public bool HasGameEnded()
        {
            return CurrentState switch
            {
                GomokuGameState.BlackWins or GomokuGameState.WhiteWins or GomokuGameState.Draw => true,
                _ => false,
            };
        }

        public void PlaceStone(CellCoordinates cell)
        {
            if (HasGameEnded())
            {
                throw new GomokuGameException("Game has ended");
            }
            
            if (!Board.IsOnTheBoard(cell))
            {
                throw new GomokuGameException(
                    $"Cell with these coordinates ({cell.Row}, {cell.Column}) does not exist on the board");
            }

            if (!Board.CanPlaceStone(cell))
            {
                throw new GomokuGameException($"This cell ({cell.Row}, {cell.Column}) is occupied");
            }

            var player = CurrentState switch
            {
                GomokuGameState.BlackToMove => Player.Black,
                GomokuGameState.WhiteToMove => Player.White,
                _ => throw new GomokuGameException("Game has ended"),
            };
            Board.PlaceStone(player, cell);
            var isAWinningMove = Board.IsPartOfWinningSequence(cell);
            var moreMovesAvailable = Board.AreMoreMovesAvailable();

            CurrentState = Transition(CurrentState, isAWinningMove, moreMovesAvailable);
        }

        private static GomokuGameState Transition(GomokuGameState current, bool wasAWinningMove, bool anyMoreMoves)
        {
            if (wasAWinningMove)
            {
                return current switch
                {
                    GomokuGameState.BlackToMove => GomokuGameState.BlackWins,
                    GomokuGameState.WhiteToMove => GomokuGameState.WhiteWins,
                    _ => current,
                };
            }
            else if (anyMoreMoves)
            {
                return current switch
                {
                    GomokuGameState.BlackToMove => GomokuGameState.WhiteToMove,
                    GomokuGameState.WhiteToMove => GomokuGameState.BlackToMove,
                    _ => current,
                };
            } else
            {
                return current switch
                {
                    GomokuGameState.BlackToMove or GomokuGameState.WhiteToMove => GomokuGameState.Draw,
                    _ => current
                };
            }
        }
    }
}
