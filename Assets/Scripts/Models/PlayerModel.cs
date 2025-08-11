using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using DefaultNamespace.Data;
using UnityEngine;

namespace Models
{
    public class PlayerModel
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public Sprite PlayerAvatar { get; private set; }
        public int Points { get; private set; }
        public ResourceCollectionModel Tokens { get; private set; }
        public ResourceCollectionModel PermanentResources { get; private set; }
        private List<ConcertCardData> concertCards = new List<ConcertCardData>();

        public MusicCardCollectionModel ReservedCards { get; private set; } = new MusicCardCollectionModel();
        private MusicCardCollectionModel purchasedCards = new MusicCardCollectionModel();

        public PlayerModel(PlayerConfig playerConfig)
        {
            PlayerId = playerConfig.PlayerId;
            PlayerName = playerConfig.PlayerName;
            PlayerAvatar = playerConfig.PlayerAvatar;
            Tokens = new ResourceCollectionModel();
            concertCards = new List<ConcertCardData>();
        }


        public MusicCardData FindCard(string cardId)
        {
            var purchased = purchasedCards.FindCard(cardId);
            if (purchased != null) return purchased;

            var reserved = ReservedCards.FindCard(cardId);
            if (reserved != null) return reserved;

            return null;
        }

        public bool HasCard(string cardId) => FindCard(cardId) != null;

        public int GetPurchasedCardCount() => purchasedCards.Count;
        public int GetReservedCardCount() => ReservedCards.Count;
        public int GetConcertCardCount() => concertCards.Count;
        public int GetTokenCount() => Tokens.GetTotalResourcese();

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
            var result = purchasedCards.AddCard(cardId);
            UpdatePoints();
            return result;
        }

        public ResourceCollectionModel GetPurchasedAllResourceCollection()
        {
            return purchasedCards.GetAllResourceCollection();
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
            concertCards.Add(concertCard);
            UpdatePoints();
            return true;
        }

        public bool RemoveConcertCard(ConcertCardData concertCard)
        {
            concertCards.Remove(concertCard);
            UpdatePoints();
            return true;
        }

        public int CalculatePointsFromMusicCards()
        {
            return purchasedCards.CalculatePoints();
        }

        public int CalculatePointsFromConcertCards()
        {
            int points = 0;
            foreach (var card in concertCards)
            {
                points += card.Points;
            }
            return points;
        }

        public int CalculatePoints()
        {
            return CalculatePointsFromMusicCards() + CalculatePointsFromConcertCards();
        }

        public void UpdatePoints()
        {
            Points = CalculatePoints();
        }

        public List<MusicCardData> GetPurchasedMusicCardDatas()
        {
            return purchasedCards.GetAllCards().ToList();
        }
    }
}