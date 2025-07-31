using Assets.Scripts.UI.Elements;
using UI.Board.BoardMusicCardPanel;
using UI.Board.BoardEndTurnButton;
using UI.Board.BoardPlayerPanel;
using UnityEngine;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardPanelView boardMusicCardPanelView;
        [SerializeField] private UniversalTokenElement[] boardTokenPanelView;
        [SerializeField] private BoardEndTurnButtonView boardEndTurnButtonView;
        [SerializeField] private BoardPlayerPanelView[] boardPlayerPanelViews = new BoardPlayerPanelView[4];
        public BoardMusicCardPanelView BoardMusicCardPanelView => boardMusicCardPanelView;
        public UniversalTokenElement[] BoardTokenPanelView => boardTokenPanelView;
        public BoardEndTurnButtonView BoardEndTurnButtonView => boardEndTurnButtonView;
        public BoardPlayerPanelView[] BoardPlayerPanelViews => boardPlayerPanelViews;
    }
}