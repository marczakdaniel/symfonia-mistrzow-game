using System;
using DefaultNamespace.Data;
using UnityEngine;

namespace Models
{
    public class MusicCardModel
    {
        public string MusicCardId { get; private set; }
        public string CardId => data.id;
        public string CardName => data.cardName;
        public string CardDescription => data.cardDescription;
        public Sprite CardImage => data.cardImage;
        public ResourceType ResourceType => data.resourceProvided;
        public ResourceCost Cost => data.cost;

        private MusicCardData data;

        public MusicCardModel(MusicCardData data)
        {
            MusicCardId = Guid.NewGuid().ToString();
            this.data = data;
        }
        // Business Logic
        public bool CanAfford(PlayerModel player)
        {
            return false;
        }
    }
}