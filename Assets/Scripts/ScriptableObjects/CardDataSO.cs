using UnityEngine;
using System.Collections.Generic;
using SymfoniaMistrzow.Core.Models;

namespace SymfoniaMistrzow.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Symfonia Mistrzów/Card Data", order = 1)]
    public class CardDataSO : ScriptableObject
    {
        public int id;
        public int level;
        public int points;
        public TokenColor gemColor;

        [System.Serializable]
        public struct CostEntry
        {
            public TokenColor tokenColor;
            public int amount;
        }

        public List<CostEntry> cost;

        public Card ToCardModel()
        {
            var card = new Card
            {
                Id = id,
                Level = level,
                Points = points,
                GemColor = gemColor,
                Cost = new Dictionary<TokenColor, int>()
            };

            foreach (var costEntry in cost)
            {
                card.Cost.Add(costEntry.tokenColor, costEntry.amount);
            }

            return card;
        }
    }
} 