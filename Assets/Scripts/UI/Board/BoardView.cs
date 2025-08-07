using Assets.Scripts.UI.Elements;
using UI.Board.BoardMusicCardPanel;
using UI.Board.BoardEndTurnButton;
using UI.Board.BoardPlayerPanel;
using UnityEngine;
using DefaultNamespace.Elements;
using R3;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using UI.Board.BoardMusicCardPanel.BoardCardDeck;

namespace UI.Board
{
    public class BoardView : MonoBehaviour
    {
        public Subject<Unit> OnBoardConcertCardButtonClicked { get; private set; } = new Subject<Unit>();
        [SerializeField] private UniversalTokenElement[] boardTokenPanelView;
        [SerializeField] private BoardEndTurnButtonView boardEndTurnButtonView;
        [SerializeField] private BoardPlayerPanelView[] boardPlayerPanelViews = new BoardPlayerPanelView[4];
        [SerializeField] private ButtonElement boardConcertCardButton;

        [SerializeField] private BoardMusicCardView[] level1Cards;
        [SerializeField] private BoardMusicCardView[] level2Cards;
        [SerializeField] private BoardMusicCardView[] level3Cards;

        [SerializeField] private BoardCardDeckView level1CardDeck;
        [SerializeField] private BoardCardDeckView level2CardDeck;
        [SerializeField] private BoardCardDeckView level3CardDeck;

        [SerializeField] private AnimationSequencerController opendAnimation;
        [SerializeField] private AnimationSequencerController closeAnimation;
        public UniversalTokenElement[] BoardTokenPanelView => boardTokenPanelView;
        public BoardEndTurnButtonView BoardEndTurnButtonView => boardEndTurnButtonView;
        public BoardPlayerPanelView[] BoardPlayerPanelViews => boardPlayerPanelViews;
        public BoardMusicCardView[] Level1Cards => level1Cards;
        public BoardMusicCardView[] Level2Cards => level2Cards;
        public BoardMusicCardView[] Level3Cards => level3Cards;

        public BoardCardDeckView Level1CardDeck => level1CardDeck;
        public BoardCardDeckView Level2CardDeck => level2CardDeck;
        public BoardCardDeckView Level3CardDeck => level3CardDeck;

        public AnimationSequencerController OpenedAnimation => opendAnimation;
        public AnimationSequencerController ClosedAnimation => closeAnimation;


        public async UniTask PlayOpenAnimation()
        {
            await opendAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }

        public void Awake()
        {
            boardConcertCardButton.OnClick.Subscribe(_ => OnBoardConcertCardButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }
}