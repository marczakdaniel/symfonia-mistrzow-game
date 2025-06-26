using DefaultNamespace.Data;
using UnityEngine;

namespace DefaultNamespace.Views
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardView[] level1Cards = new BoardMusicCardView[4];
        [SerializeField] private BoardMusicCardView[] level2Cards = new BoardMusicCardView[4];
        [SerializeField] private BoardMusicCardView[] level3Cards = new BoardMusicCardView[4];

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