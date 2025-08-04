using DefaultNamespace.Data;
using R3;

namespace UI.CardPurchaseWindow.CardPurchaseSingleToken
{
    public class CardPurchaseSingleTokenViewModel
    {
        public ResourceType Token { get; private set; }
        public string CardId { get; private set; }

        public CardPurchaseSingleTokenViewModel(ResourceType token)
        {
            Token = token;
        }

        public void SetCardId(string cardId)
        {
            CardId = cardId;
        }
    }
}