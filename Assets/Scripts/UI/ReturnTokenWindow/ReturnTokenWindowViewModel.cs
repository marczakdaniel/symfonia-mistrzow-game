using R3;

namespace UI.ReturnTokenWindow
{
    public enum ReturnTokenWindowState
    {
        Disabled,
        DuringOpenAnimation,
        DuringCloseAnimation,
        Active,
    }
    public class ReturnTokenWindowViewModel
    {
        public ReactiveProperty<ReturnTokenWindowState> State { get; private set; } = new ReactiveProperty<ReturnTokenWindowState>(ReturnTokenWindowState.Disabled);
        public ReactiveProperty<int> AllPlayerTokensCount { get; private set; } = new ReactiveProperty<int>(0);

        private void SetState(ReturnTokenWindowState state)
        {
            State.Value = state;
        }

        public void OpenWindow()
        {
            SetState(ReturnTokenWindowState.DuringOpenAnimation);
        }

        public void OnOpenAnimationFinished()
        {
            SetState(ReturnTokenWindowState.Active);
        }

        public void CloseWindow()
        {
            SetState(ReturnTokenWindowState.DuringCloseAnimation);
        }

        public void OnCloseAnimationFinished()
        {
            SetState(ReturnTokenWindowState.Disabled);
        }

        public void UpdateAllPlayerTokensCount(int allPlayerTokensCount)
        {
            AllPlayerTokensCount.Value = allPlayerTokensCount;
        }
    }
}