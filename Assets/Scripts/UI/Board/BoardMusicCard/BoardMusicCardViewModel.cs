using DefaultNamespace.Data;
using R3;
using UnityEngine;
namespace UI.Board.BoardMusicCardPanel.BoardMusicCard 
{
    public class BoardMusicCardViewModel {
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public bool CardDisabled { get; private set; }
        public bool CardCanBeDisabled { get; private set; }

        public BoardMusicCardViewModel(int level, int position) {
            Level = level;
            Position = position;
            CardDisabled = false;
            CardCanBeDisabled = false;
        }

        public void RevealCard(string musicCardId)
        {
            MusicCardId = musicCardId;
        }

        public void HideCard()
        {
            MusicCardId = null;
        }

        public void SetCardDisabled()
        {
            CardDisabled = true;
        }

        public void SetCardCanBeDisabled()
        {
            CardCanBeDisabled = true;
        }
    }
}