using DefaultNamespace.Data;
using R3;

namespace UI.CardPurchaseWindow.CardPurchaseSingleToken
{
    public enum CardPurchaseSingleTokenState
    {
        Disabled,
        Active
    }
    public class CardPurchaseSingleTokenViewModel
    {
        public ReactiveProperty<CardPurchaseSingleTokenState> State { get; private set; } = new ReactiveProperty<CardPurchaseSingleTokenState>(CardPurchaseSingleTokenState.Disabled);

        public ResourceType Token { get; private set; }
        public ReactiveProperty<int> CurrentSelectedTokensCount { get; private set; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> PlayerTokensCount { get; private set; } = new ReactiveProperty<int>(0);

        public CardPurchaseSingleTokenViewModel(ResourceType token)
        {
            Token = token;
        }

        public void SetCurrentSelectedTokensCount(int count)
        {
            CurrentSelectedTokensCount.Value = count;
            State.Value = CardPurchaseSingleTokenState.Active;
        }

        public void SetPlayerTokensCount(int count)
        {
            CurrentSelectedTokensCount.Value = 0;
            PlayerTokensCount.Value = count;
            State.Value = CardPurchaseSingleTokenState.Active;
        }
    }
}