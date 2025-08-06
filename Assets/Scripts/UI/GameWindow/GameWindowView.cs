using Assets.Scripts.UI.ConcertCardsWindow;
using UI.Board;
using UI.CardPurchaseWindow;
using UI.DeckCardInfoWindow;
using UI.InfoWindow;
using UI.MusicCardDetailsPanel;
using UI.PlayerResourcesWindow;
using UI.ReserveDeckCardWindow;
using UI.ReturnTokenWindow;
using UI.SelectTokenWindow;
using UI.StartTurnWindow;
using UnityEngine;

namespace UI.GameWindow
{
    public class GameWindowView : MonoBehaviour
    {
        public BoardView BoardView => boardView;
        public MusicCardDetailsPanelView MusicCardDetailsPanelView => musicCardDetailsPanelView;
        public SelectTokenWindowView SelectTokenWindowView => selectTokenWindowView;
        public StartTurnWindowView StartTurnWindowView => startTurnWindowView;
        public ReturnTokenWindowView ReturnTokenWindowView => returnTokenWindowView;
        public CardPurchaseWindowView CardPurchaseWindowView => cardPurchaseWindowView;
        public PlayerResourcesWindowView PlayerResourcesWindowView => playerResourcesWindowView;
        public ConcertCardsWindowView ConcertCardsWindowView => concertCardsWindowView;
        public ReserveDeckCardWindowView ReserveDeckCardWindowView => reserveDeckCardWindowView;
        public DeckCardInfoWindowView DeckCardInfoWindowView => deckCardInfoWindowView;
        [SerializeField] private BoardView boardView;
        [SerializeField] private MusicCardDetailsPanelView musicCardDetailsPanelView;
        [SerializeField] private SelectTokenWindowView selectTokenWindowView;
        [SerializeField] private StartTurnWindowView startTurnWindowView;
        [SerializeField] private ReturnTokenWindowView returnTokenWindowView;
        [SerializeField] private CardPurchaseWindowView cardPurchaseWindowView;
        [SerializeField] private PlayerResourcesWindowView playerResourcesWindowView;
        [SerializeField] private ConcertCardsWindowView concertCardsWindowView;
        [SerializeField] private ReserveDeckCardWindowView reserveDeckCardWindowView;
        [SerializeField] private DeckCardInfoWindowView deckCardInfoWindowView;
    }
}