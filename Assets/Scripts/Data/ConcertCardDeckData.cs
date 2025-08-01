using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "ConcertCardDeckData", menuName = "Game/ConcertCardDeckData")]
    public class ConcertCardDeckData : ScriptableObject
    {
        [SerializeField] private ConcertCardData[] concertCards;
        
        public ConcertCardData[] ConcertCards => concertCards;
    }
}