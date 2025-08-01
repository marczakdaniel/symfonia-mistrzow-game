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
        public int PlayerTokensCount { get; private set; } = 0;
        public string CardId { get; private set; }

        public CardPurchaseSingleTokenViewModel(ResourceType token)
        {
            Token = token;
        }

        public void SetCurrentSelectedTokensCount(int count)
        {
            CurrentSelectedTokensCount.Value = count;
        }

        public void Initialize(ResourceType token, int currentSelectedTokensCount, int playerTokensCount, string cardId)
        {
            Token = token;
            CardId = cardId;
            CurrentSelectedTokensCount.Value = currentSelectedTokensCount;
            PlayerTokensCount = playerTokensCount;
            State.Value = CardPurchaseSingleTokenState.Active;
        }

        public void Close()
        {
            State.Value = CardPurchaseSingleTokenState.Disabled;
        }
    }
}