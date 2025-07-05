using System.Collections.Generic;
using DefaultNamespace.Data;

namespace Models
{
    public class PlayerModel
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public int Points { get; private set; }
        public ResourceCollectionModel Tokens { get; private set; }
        public ResourceCollectionModel PermanentResources { get; private set; }

        public MusicCardCollectionModel ReservedCards { get; private set; }
        public MusicCardCollectionModel PurchasedCards { get; private set; }

        public PlayerModel(string playerId, string playerName, int score, int health)
        {
            PlayerId = playerId;

            PurchasedCards = new MusicCardCollectionModel();
            ReservedCards = new MusicCardCollectionModel();
        }

        public MusicCardData FindCard(string cardId)
        {
            var purchased = PurchasedCards.FindCard(cardId);
            if (purchased != null) return purchased;

            var reserved = ReservedCards.FindCard(cardId);
            if (reserved != null) return reserved;

            return null;
        }

        public bool HasCard(string cardId) => FindCard(cardId) != null;

        public int GetPurchasedCardCount() => PurchasedCards.Count;
        public int GetReservedCardCount() => ReservedCards.Count;

        public bool CanReserveMore() => ReservedCards.Count < 3;

        public ResourceCollectionModel CalculateRealCost(ResourceCollectionModel cost)
        {

            return cost;
        }

        public bool AddCardToReserved(string cardId)
        {
            return ReservedCards.AddCard(cardId);
        }

        public bool AddCardToPurchased(string cardId)
        {
            return PurchasedCards.AddCard(cardId);
        }

        public bool RemoveTokens()
        {
            return true;
        }

        public bool AddTokens(string cardId)
        {
            return true;
        }
    }
}