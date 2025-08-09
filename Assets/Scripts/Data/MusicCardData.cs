using UnityEngine;

namespace DefaultNamespace.Data
{

    [CreateAssetMenu(fileName = "MusicCardData", menuName = "Game/MusicCardData")]
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
        
        [TextArea]
        public string cardDescription;

        public string Id => id;
        public int Level => level;
        public int Points => points;
        public ResourceType ResourceProvided => resourceProvided;
        public IResourceCostReader Cost => cost;
        public Sprite CardImage => cardImage;
        public string CardName => cardName;
        public string CardDescription => cardDescription;
    }
}