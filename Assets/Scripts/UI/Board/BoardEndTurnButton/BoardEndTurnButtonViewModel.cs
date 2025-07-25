using R3;

namespace UI.Board.BoardEndTurnButton
{
    public enum BoardEndTurnButtonState
    {
        Disabled,
        Enabled,
    }

    public class BoardEndTurnButtonViewModel
    {
        public ReactiveProperty<BoardEndTurnButtonState> State { get; private set; } = new ReactiveProperty<BoardEndTurnButtonState>(BoardEndTurnButtonState.Disabled);

        public BoardEndTurnButtonViewModel()
        {

        }

        public void SetEnabled()
        {
            SetState(BoardEndTurnButtonState.Enabled);
        }

        public void SetDisabled()
        {
            SetState(BoardEndTurnButtonState.Disabled);
        }

        private void SetState(BoardEndTurnButtonState state)
        {
            State.Value = state;
        }
    }
}