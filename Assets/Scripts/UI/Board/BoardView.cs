using UI.Board.BoardMusicCardPanel;
using UI.Board.BoardEndTurnButton;
using UI.Board.BoardTokenPanel;
using UI.Board.BoardPlayersPanel;
using UnityEngine;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardPanelView boardMusicCardPanelView;
        [SerializeField] private BoardTokenPanelView boardTokenPanelView;
        [SerializeField] private BoardEndTurnButtonView boardEndTurnButtonView;
        [SerializeField] private BoardPlayersPanelView boardPlayersPanelView;
        public BoardMusicCardPanelView BoardMusicCardPanelView => boardMusicCardPanelView;
        public BoardTokenPanelView BoardTokenPanelView => boardTokenPanelView;
        public BoardEndTurnButtonView BoardEndTurnButtonView => boardEndTurnButtonView;
        public BoardPlayersPanelView BoardPlayersPanelView => boardPlayersPanelView;
    }
}