using System;
using UI.Board;
using Command;
using UI.MusicCardDetailsPanel;
using UI.SelectTokenWindow;
using UI.StartTurnWindow;
using UI.ReturnTokenWindow;
using UI.CardPurchaseWindow;
using UI.PlayerResourcesWindow;
using Assets.Scripts.UI.ConcertCardsWindow;
using UI.ReserveDeckCardWindow;

namespace UI.GameWindow
{
    public class GameWindowPresenter :
        IDisposable
    {
        private readonly GameWindowView view;
        private readonly GameWindowViewModel viewModel = new GameWindowViewModel();
        private BoardPresenter boardPresenter;
        private MusicCardDetailsPanelPresenter musicCardDetailsPanelPresenter;
        private SelectTokenWindowPresenter selectTokenWindowPresenter;
        private StartTurnWindowPresenter startTurnWindowPresenter;
        private ReturnTokenWindowPresenter returnTokenWindowPresenter;
        private CardPurchaseWindowPresenter cardPurchaseWindowPresenter;
        private PlayerResourcesWindowPresenter playerResourcesWindowPresenter;
        private ConcertCardsWindowPresenter concertCardsWindowPresenter;
        private ReserveDeckCardWindowPresenter reserveDeckCardWindowPresenter;
        private CommandFactory commandFactory;
        public GameWindowPresenter(GameWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            boardPresenter = new BoardPresenter(view.BoardView, commandFactory);
            musicCardDetailsPanelPresenter = new MusicCardDetailsPanelPresenter(view.MusicCardDetailsPanelView, commandFactory);
            selectTokenWindowPresenter = new SelectTokenWindowPresenter(view.SelectTokenWindowView, commandFactory);
            startTurnWindowPresenter = new StartTurnWindowPresenter(view.StartTurnWindowView, commandFactory);
            returnTokenWindowPresenter = new ReturnTokenWindowPresenter(view.ReturnTokenWindowView, commandFactory);
            cardPurchaseWindowPresenter = new CardPurchaseWindowPresenter(view.CardPurchaseWindowView, commandFactory);
            playerResourcesWindowPresenter = new PlayerResourcesWindowPresenter(view.PlayerResourcesWindowView, commandFactory);
            concertCardsWindowPresenter = new ConcertCardsWindowPresenter(view.ConcertCardsWindowView, commandFactory);
            reserveDeckCardWindowPresenter = new ReserveDeckCardWindowPresenter(view.ReserveDeckCardWindowView, commandFactory);
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {

        }

        private void ConnectView()
        {

        }

        private void SubscribeToEvents()
        {
        }

        public void Dispose()
        {
        }
    }
}