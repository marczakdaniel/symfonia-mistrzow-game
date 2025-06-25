using UnityEngine;

namespace DefaultNamespace.Data
{
    public class MusicCardData : ScriptableObject
    {
        [Header("Card Identity")]
        public string id;
        public int level;
        public int points;

        [Header("Card Provides")]
        public ResourceType resourceProvided;

        [Header("Card Cost")]
        public ResourceCost cost;

        [Header("Card Visual")]
        public Sprite cardImage;

        [Header("Card Info")]
        public string cardName;
        public string cardDescription;
    }
}