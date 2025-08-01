using System.Collections.Generic;
using Assets.Scripts.Data;
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
        public List<ConcertCardData> ConcertCards { get; private set; } = new List<ConcertCardData>();

        public MusicCardCollectionModel ReservedCards { get; private set; } = new MusicCardCollectionModel();
        public MusicCardCollectionModel PurchasedCards { get; private set; } = new MusicCardCollectionModel();

        public PlayerModel(PlayerConfig playerConfig)
        {
            PlayerId = playerConfig.PlayerId;
            PlayerName = playerConfig.PlayerName;
            Tokens = new ResourceCollectionModel();
            ConcertCards = new List<ConcertCardData>();
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

        public bool HasReserveCard(string cardId) => ReservedCards.FindCard(cardId) != null;

        public bool CanReserveMore() => ReservedCards.Count < 3;

        public ResourceCollectionModel CalculateRealCost(ResourceCollectionModel cost)
        {

            return cost;
        }

        public bool AddCardToReserved(string cardId)
        {
            return ReservedCards.AddCard(cardId);
        }

        public bool RemoveCardFromReserved(string cardId)
        {
            return ReservedCards.RemoveCard(cardId);
        }

        public bool AddCardToPurchased(string cardId)
        {
            return PurchasedCards.AddCard(cardId);
        }

        public bool RemoveTokens(ResourceCollectionModel tokens)
        {
            Tokens.Subtract(tokens);
            return true;
        }

        public bool AddTokens(ResourceCollectionModel tokens)
        {
            Tokens.Add(tokens);
            return true;
        }

        public bool AddConcertCard(ConcertCardData concertCard)
        {
            ConcertCards.Add(concertCard);
            return true;
        }

        public bool RemoveConcertCard(ConcertCardData concertCard)
        {
            ConcertCards.Remove(concertCard);
            return true;
        }

        public int CalculatePointsFromMusicCards()
        {
            return PurchasedCards.CalculatePoints();
        }

        public int CalculatePointsFromConcertCards()
        {
            int points = 0;
            foreach (var card in ConcertCards)
            {
                points += card.Points;
            }
            return points;
        }

        public int CalculatePoints()
        {
            return CalculatePointsFromMusicCards() + CalculatePointsFromConcertCards();
        }
    }
}