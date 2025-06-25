using DefaultNamespace.Data;
using UnityEngine;

namespace DefaultNamespace.Views
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private MusicCardView[] level1Cards = new MusicCardView[4];
        [SerializeField] private MusicCardView[] level2Cards = new MusicCardView[4];
        [SerializeField] private MusicCardView[] level3Cards = new MusicCardView[4];

        public void InitializeBoard()
        {

        }

        public void AddCard(int level, MusicCardData card)
        {

        }

        public void RemoveCard(int level, MusicCardData card)
        {

        }
    }
}