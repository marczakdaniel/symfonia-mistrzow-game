using System.Collections.Generic;
using DefaultNamespace.Data;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "ConcertCardData", menuName = "Game/ConcertCardData")]
    public class ConcertCardData : ScriptableObject
    {
        [SerializeField] 
        private string id;

        [SerializeField]
        private Sprite image;

        [SerializeField]
        private ResourceCost requirements;

        [SerializeField]
        private int points;

        public string Id => id;
        public Sprite Image => image;
        public ResourceCost Requirements => requirements;
        public int Points => points;

        public Dictionary<ResourceType, int> GetRequirements()
        {
            return requirements.GetResourceCollectionModel().GetAllResources();
        }


    }
}