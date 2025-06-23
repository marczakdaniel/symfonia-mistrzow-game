using System;

namespace DefaultNamespace.UI.CardActionOverlay
{
    public class CardActionOverlayData
    {
        public CardData CardData;

        public Action<CardData> OnCardBuy;
        public Action<CardData> OnCardReserve;

        public CardActionOverlayData(CardData cardData, Action<CardData> onCardBuy, Action<CardData> onCardReserve)
        {
            CardData = cardData;
            OnCardBuy = onCardBuy;
            OnCardReserve = onCardReserve;
        }
    }
}