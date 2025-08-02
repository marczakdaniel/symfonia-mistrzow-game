using DefaultNamespace.Data;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "MusicCardDeckData", menuName = "Game/MusicCardDeckData")]
    public class MusicCardDeckData : ScriptableObject
    {

        [SerializeField] private MusicCardData[] cards = new MusicCardData[90];

        public MusicCardData[] Cards => cards;
    }
}