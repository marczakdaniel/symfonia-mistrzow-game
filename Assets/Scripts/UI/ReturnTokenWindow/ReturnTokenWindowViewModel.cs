using R3;

namespace UI.ReturnTokenWindow
{
    public enum ReturnTokenWindowState
    {
        Disabled,
        Active,
    }
    public class ReturnTokenWindowViewModel
    {
        public ReactiveProperty<ReturnTokenWindowState> State { get; private set; } = new ReactiveProperty<ReturnTokenWindowState>(ReturnTokenWindowState.Disabled);

        private void SetState(ReturnTokenWindowState state)
        {
            State.Value = state;
        }

        public void OnReturnTokenWindowOpened()
        {
            SetState(ReturnTokenWindowState.Active);
        }

        public void OnReturnTokenWindowClosed()
        {
            SetState(ReturnTokenWindowState.Disabled);
        }
    }
}