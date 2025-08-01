using Assets.Scripts.UI.Elements;
using UI.Board.BoardMusicCardPanel;
using UI.Board.BoardEndTurnButton;
using UI.Board.BoardPlayerPanel;
using UnityEngine;
using DefaultNamespace.Elements;
using R3;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        public Subject<Unit> OnBoardConcertCardButtonClicked { get; private set; } = new Subject<Unit>();
        [SerializeField] private BoardMusicCardPanelView boardMusicCardPanelView;
        [SerializeField] private UniversalTokenElement[] boardTokenPanelView;
        [SerializeField] private BoardEndTurnButtonView boardEndTurnButtonView;
        [SerializeField] private BoardPlayerPanelView[] boardPlayerPanelViews = new BoardPlayerPanelView[4];
        [SerializeField] private ButtonElement boardConcertCardButton;
        public BoardMusicCardPanelView BoardMusicCardPanelView => boardMusicCardPanelView;
        public UniversalTokenElement[] BoardTokenPanelView => boardTokenPanelView;
        public BoardEndTurnButtonView BoardEndTurnButtonView => boardEndTurnButtonView;
        public BoardPlayerPanelView[] BoardPlayerPanelViews => boardPlayerPanelViews;

        public void Awake()
        {
            boardConcertCardButton.OnClick.Subscribe(_ => OnBoardConcertCardButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }
}