using DefaultNamespace.Data;
using R3;
using UnityEngine;
namespace UI.Board.BoardMusicCardPanel.BoardMusicCard 
{
    public class BoardMusicCardViewModel {
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public BoardMusicCardViewModel(int level, int position) {
            Level = level;
            Position = position;
        }

        public void RevealCard(string musicCardId)
        {
            MusicCardId = musicCardId;
        }

        public void HideCard()
        {
            MusicCardId = null;
        }
    }
}