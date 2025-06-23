using UnityEngine;
using SymfoniaMistrzow.MVP.Common;

namespace SymfoniaMistrzow.MVP.Board
{
    public class BoardView : MonoBehaviour, IView
    {
        [SerializeField] private Transform tier1CardContainer;
        [SerializeField] private Transform tier2CardContainer;
        [SerializeField] private Transform tier3CardContainer;

        // References to card prefab
        [SerializeField] private GameObject cardPrefab;

        public Transform Tier1CardContainer => tier1CardContainer;
        public Transform Tier2CardContainer => tier2CardContainer;
        public Transform Tier3CardContainer => tier3CardContainer;
        public GameObject CardPrefab => cardPrefab;
    }
} 