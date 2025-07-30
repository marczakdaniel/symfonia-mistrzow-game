using R3;

namespace UI.StartTurnWindow
{

    public enum StartTurnWindowState
    {
        Closed,
        DuringOpenAnimation,
        DuringCloseAnimation,
        Opened
    }

    public class StartTurnWindowViewModel   
    {
        public ReactiveProperty<StartTurnWindowState> State { get; private set; } = new ReactiveProperty<StartTurnWindowState>(StartTurnWindowState.Closed);
        public ReactiveProperty<string> CurrentPlayerName { get; private set; } = new ReactiveProperty<string>();

        private void SetState(StartTurnWindowState state)
        {
            State.Value = state;
        }

        public void Open(string currentPlayerName)
        {
            CurrentPlayerName.Value = currentPlayerName;
            SetState(StartTurnWindowState.DuringOpenAnimation);
        }

        public void OpenAnimationCompleted()
        {
            SetState(StartTurnWindowState.Opened);
        }

        public void Close()
        {
            SetState(StartTurnWindowState.DuringCloseAnimation);
        }

        public void CloseAnimationCompleted()
        {
            SetState(StartTurnWindowState.Closed);
        }
    }
}