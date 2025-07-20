using R3;

namespace UI.SelectTokenWindow.SelectBoardTokenPanel
{
    public enum SelectBoardTokenPanelState
    {
        Disabled,
        Active,
        DuringOpenAnimation,
    }

    public class SelectBoardTokenPanelViewModel
    {
        public ReactiveProperty<SelectBoardTokenPanelState> State { get; private set; } = new ReactiveProperty<SelectBoardTokenPanelState>(SelectBoardTokenPanelState.Disabled);

        private void SetState(SelectBoardTokenPanelState state)
        {
            State.Value = state;
        }

        public void OnOpenWindow()
        {
            SetState(SelectBoardTokenPanelState.DuringOpenAnimation);
        }

        public void OnOpenWindowFinished()
        {
            SetState(SelectBoardTokenPanelState.Active);
        }
    }
}