using System;

namespace GomokuGameApi
{
    public class GameStatusDto
    {
        public bool HasGameEnded { get; init; }
        public string CurrentState { get; init; }
        public string[] BoardLayout { get; init; }

        public GameStatusDto(GomokuGame.GomokuGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            HasGameEnded = game.HasGameEnded;
            CurrentState = game.CurrentState.ToString();
            BoardLayout = game.Board.ToStringPerRow();
        }
    }
}
