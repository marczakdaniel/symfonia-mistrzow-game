using DefaultNamespace.Data;
using R3;

namespace UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken
{
    public enum ReturnTokenSinglePlayerTokenState
    {
        Disabled,
        Active,
        DuringAddingTokenAnimation,
        DuringRemovingTokenAnimation,
        DuringAcceptingTokensAnimation,
    }

    public class ReturnTokenSinglePlayerTokenViewModel
    {
        public ResourceType Token { get; private set; }
        public ReactiveProperty<int> Count { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<ReturnTokenSinglePlayerTokenState> State { get; private set; } = new ReactiveProperty<ReturnTokenSinglePlayerTokenState>(ReturnTokenSinglePlayerTokenState.Disabled);

        public ReturnTokenSinglePlayerTokenViewModel(ResourceType token)
        {
            Token = token;
        }

        private void SetCount(int count)
        {
            Count.Value = count;
        }

        private void SetState(ReturnTokenSinglePlayerTokenState state)
        {
            State.Value = state;
        }

        public void AddToken(int newValue)
        {
            SetCount(newValue);
            SetState(ReturnTokenSinglePlayerTokenState.DuringAddingTokenAnimation);
        }

        public void OnAddTokenFinished()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Active);
        }

        public void RemoveToken(int newValue)
        {
            SetCount(newValue);
            SetState(ReturnTokenSinglePlayerTokenState.DuringRemovingTokenAnimation);
        }

        public void OnRemoveTokenFinished()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Active);
        }

        public void AcceptTokens()
        {
            SetState(ReturnTokenSinglePlayerTokenState.DuringAcceptingTokensAnimation);
        }

        public void OnAcceptTokensFinished()
        {
            SetState(ReturnTokenSinglePlayerTokenState.Disabled);
        }

        public void OnReturnTokenWindowOpened(int count)
        {
            SetCount(count);
            SetState(ReturnTokenSinglePlayerTokenState.Active);
        }
    }
}   