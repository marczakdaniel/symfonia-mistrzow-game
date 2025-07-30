using DefaultNamespace.Data;
using R3;

namespace UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken
{
    public enum ReturnTokenSinglePlayerTokenState
    {
        Disabled,
        Active,
        DuringReturnTokenInitialization,
        DuringChangingTokenValue,
    }

    public class ReturnTokenSinglePlayerTokenViewModel
    {
        public ResourceType Token { get; private set; }
        public int Count { get; private set; } = 0;
        public ReactiveProperty<ReturnTokenSinglePlayerTokenState> State { get; private set; } = new ReactiveProperty<ReturnTokenSinglePlayerTokenState>(ReturnTokenSinglePlayerTokenState.Disabled);

        public ReturnTokenSinglePlayerTokenViewModel(ResourceType token)
        {
            Token = token;
        }

        private void SetCount(int count)
        {
            Count = count;
        }

        private void SetState(ReturnTokenSinglePlayerTokenState state)
        {
            State.Value = state;
        }

        public void ChangeTokenValue(int newValue)
        {
            SetCount(newValue);
            SetState(ReturnTokenSinglePlayerTokenState.DuringChangingTokenValue);
        }

        public void OnChangeTokenValueFinished()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Active);
        }

        public void ReturnTokenInitialization(int count)
        {
            SetCount(count);
            SetState(ReturnTokenSinglePlayerTokenState.DuringReturnTokenInitialization);
        }

        public void OnReturnTokenInitializationFinished()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Active);
        }

        public void OnReturnTokenWindowClosed()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Disabled);
        }
    }
}   