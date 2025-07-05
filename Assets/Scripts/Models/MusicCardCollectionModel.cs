using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;

namespace Models
{
    public class MusicCardCollectionModel
    {
        private List<string> cardIds = new List<string>();

        public MusicCardCollectionModel()
        {
        }

        // Properties

        public int Count => cardIds.Count;
        public bool IsEmpty => cardIds.Count == 0;
        public IReadOnlyList<string> CardIds => cardIds.AsReadOnly();

        // Card Access
        public IEnumerable<MusicCardData> GetAllCards()
        {
            foreach (var cardId in cardIds)
            {
                var card = MusicCardRepository.Instance.GetCard(cardId);
                if (card != null)
                {
                    yield return card;
                }
            }
        }

        public MusicCardData FindCard(string cardId)
        {
            return cardIds.Contains(cardId) ? MusicCardRepository.Instance.GetCard(cardId) : null;
        }

        public string GetRandomCardId()
        {
            if (IsEmpty) return null;
            var randomIndex = UnityEngine.Random.Range(0, cardIds.Count);
            return cardIds[randomIndex];
        }

        // Modification Methods
        public bool AddCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId) || cardIds.Contains(cardId))
            {
                return false;
            }

            if (!MusicCardRepository.Instance.HasCard(cardId))
            {
                return false;
            }

            cardIds.Add(cardId);
            return true;
        }

        public bool RemoveCard(string cardId)
        {
            if (cardIds.Remove(cardId))
            {
                return true;
            }
            return false;
        }

        public bool MoveCardTo(string cardId, MusicCardCollectionModel targetCollection)
        {
            if (RemoveCard(cardId))
            {
                if (targetCollection.AddCard(cardId))
                {
                    return true;
                }
                AddCard(cardId);
            }
            return false;
        }

        public void Clear()
        {
            cardIds.Clear();
        }

        // Query Methods
        public bool Contains(string cardId)
        {
            return cardIds.Contains(cardId);
        }

        public IEnumerable<MusicCardData> Where(System.Func<MusicCardData, bool> predicate)
        {
            return GetAllCards().Where(predicate);
        }

        public int CountWhere(System.Func<MusicCardData, bool> predicate)
        {
            return GetAllCards().Count(predicate);
        }

        public MusicCardData FirstOrDefault(System.Func<MusicCardData, bool> predicate)
        {
            return GetAllCards().FirstOrDefault(predicate);
        }
        
    }
}