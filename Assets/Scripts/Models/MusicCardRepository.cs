using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using UnityEngine;

namespace Models
{
    public class MusicCardRepository
    {
        private static MusicCardRepository instance;

        public static MusicCardRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MusicCardRepository();
                }
                return instance;
            }
        }

        private Dictionary<string, MusicCardData> allCards = new Dictionary<string, MusicCardData>();

        public void Initialize(MusicCardData[] musicCardDatas)
        {
            allCards.Clear();

            foreach (var card in musicCardDatas)
            {
                if (allCards.ContainsKey(card.id))
                {
                    Debug.LogWarning($"[CardRepository] Card with id {card.id} already exists");
                    continue;
                }

                if (card != null && !string.IsNullOrEmpty(card.id))
                {
                    allCards.Add(card.id, card);
                }
            }

            Debug.Log($"[CardRepository] Initialized with {allCards.Count} cards");
        }

        public MusicCardData GetCard(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogWarning($"[CardRepository] Cannot get card with id {cardId} because it is null or empty.");
                return null;
            }

            if (!HasCard(cardId))
            {
                Debug.LogWarning($"[CardRepository] Cannot get card with id {cardId} because it does not exist.");
                return null;
            }

            return allCards[cardId];
        }

        public bool HasCard(string cardId)
        {
            return !string.IsNullOrEmpty(cardId) && allCards.ContainsKey(cardId);
        }

        public IEnumerable<MusicCardData> GetAllCards()
        {
            return allCards.Values;
        }

        public IEnumerable<MusicCardData> GetAllCardsByLevel(int level)
        {
            return allCards.Values.Where(card => card.level == level);
        }
    }
}