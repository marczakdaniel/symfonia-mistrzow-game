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
        
        public CardPurchaseWindowViewModel()
        {
        }

        public void OpenCardPurchaseWindow(MusicCardData musicCardData)
        {
            MusicCardData = musicCardData;
            State.Value = CardPurchaseWindowState.Opened;
        }

        public void CloseCardPurchaseWindow()
        {
            State.Value = CardPurchaseWindowState.Closed;
        }
    }
}