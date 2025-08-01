using System.Collections.Generic;
using DefaultNamespace.Data;
using R3;

namespace UI.CardPurchaseWindow
{
    public enum CardPurchaseWindowState
    {
        Closed,
        Opened
    }
    public class CardPurchaseWindowViewModel
    {
        public ReactiveProperty<CardPurchaseWindowState> State { get; private set; } = new ReactiveProperty<CardPurchaseWindowState>(CardPurchaseWindowState.Closed);
        public MusicCardData MusicCardData { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentCardTokens { get; private set; }

        public CardPurchaseWindowViewModel()
        {
        }

        public void OpenCardPurchaseWindow(MusicCardData musicCardData, Dictionary<ResourceType, int> currentPlayerTokens, Dictionary<ResourceType, int> currentCardTokens)
        {
            MusicCardData = musicCardData;
            CurrentPlayerTokens = currentPlayerTokens;
            CurrentCardTokens = currentCardTokens;
            State.Value = CardPurchaseWindowState.Opened;
        }

        public void CloseCardPurchaseWindow()
        {
            State.Value = CardPurchaseWindowState.Closed;
        }
    }
}