using UI.Board.BoardMusicCardPanel;
using UnityEngine;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardPanelView boardMusicCardPanelView;

        public BoardMusicCardPanelView BoardMusicCardPanelView => boardMusicCardPanelView;
    }
}