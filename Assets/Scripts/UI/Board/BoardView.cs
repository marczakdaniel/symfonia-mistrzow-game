using UI.Board.BoardMusicCardPanel;
using UI.Board.BoardTokenPanel;
using UnityEngine;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardPanelView boardMusicCardPanelView;
        [SerializeField] private BoardTokenPanelView boardTokenPanelView;

        public BoardMusicCardPanelView BoardMusicCardPanelView => boardMusicCardPanelView;
        public BoardTokenPanelView BoardTokenPanelView => boardTokenPanelView;
    }
}