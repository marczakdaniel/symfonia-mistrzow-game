using UnityEngine;

namespace DefaultNamespace.Data
{
    // Read-only interface for UI layer
    public interface IMusicCardDataReader
    {
        string Id { get; }
        int Level { get; }
        int Points { get; }
        ResourceType ResourceProvided { get; }
        IResourceCostReader Cost { get; }
        Sprite CardImage { get; }
        string CardName { get; }
        string CardDescription { get; }
    }

    public class MusicCardData : ScriptableObject, IMusicCardDataReader
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

        // Read-only interface implementation
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