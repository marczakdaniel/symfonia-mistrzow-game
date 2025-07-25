using R3;

namespace UI.Board.BoardPlayersPanel.BoardSinglePlayer
{
    public enum BoardSinglePlayerState
    {
        Disabled,
        Hidden,
        Enabled,
        CurrentPlayer,
    }

    public class BoardSinglePlayerViewModel
    {
        public string PlayerId { get;}
        public ReactiveProperty<BoardSinglePlayerState> State { get;} = new ReactiveProperty<BoardSinglePlayerState>(BoardSinglePlayerState.Disabled);

        public BoardSinglePlayerViewModel(string playerId)
        {
            PlayerId = playerId;
        }

        public void Enable()
        {
            SetState(BoardSinglePlayerState.Enabled);
        }

        public void SetCurrentPlayer()
        {
            SetState(BoardSinglePlayerState.CurrentPlayer);
        }

        public void SetNotCurrentPlayer()
        {
            SetState(BoardSinglePlayerState.Enabled);
        }

        private void SetState(BoardSinglePlayerState state)
        {
            State.Value = state;
        }
    }
}