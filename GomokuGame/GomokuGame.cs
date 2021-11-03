using System;

namespace GomokuGame
{
    public class GomokuGame
    {
        private GomokuBoard Board;
        public Player NextPlayer { get; private set; }

        public GomokuGame(GomokuBoard board) : this (board, Player.Black)
        {
        }

        public GomokuGame(GomokuBoard board, Player nextPlayer)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            NextPlayer = nextPlayer;
        }

        public void NextMove(CellCoordinates cell)
        {
            try
            {
                Board.PlaceStone(NextPlayer, cell);
                bool isAWin = Board.IsPartOfWinningSequence(cell);
                // How to signal that game is still on, who have won, and
                
                switch (NextPlayer)
                {
                    case Player.Black:
                        NextPlayer = Player.White;
                        break;
                    case Player.White:
                        NextPlayer = Player.Black;
                        break;
                    default:
                        throw new ArgumentException("Unknown player type: " + NextPlayer.ToString());
                }
            }
            catch (GomokuGameException gex)
            {
            }
            catch (Exception ex)
            {

            }
        }
    }
}
