using UI.Board;
using Command;
using UI.MusicCardDetailsPanel;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Events;
using Models;
using UI.SelectTokenWindow;
using UI.StartTurnWindow;
using UI.ReturnTokenWindow;
using UI.CardPurchaseWindow;
using UI.PlayerResourcesWindow;

namespace UI.GameWindow
{
    public class GameWindowPresenter : 
        IAsyncEventHandler<GameStartedEvent>
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
        private CommandFactory commandFactory;
        private IGameModelReader gameModelReader;
        public GameWindowPresenter(GameWindowView view, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader;
            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            boardPresenter = new BoardPresenter(view.BoardView, commandFactory, gameModelReader);
            musicCardDetailsPanelPresenter = new MusicCardDetailsPanelPresenter(view.MusicCardDetailsPanelView, commandFactory);
            selectTokenWindowPresenter = new SelectTokenWindowPresenter(view.SelectTokenWindowView, commandFactory);
            startTurnWindowPresenter = new StartTurnWindowPresenter(view.StartTurnWindowView, commandFactory);
            returnTokenWindowPresenter = new ReturnTokenWindowPresenter(view.ReturnTokenWindowView, commandFactory);
            cardPurchaseWindowPresenter = new CardPurchaseWindowPresenter(view.CardPurchaseWindowView, commandFactory);
            playerResourcesWindowPresenter = new PlayerResourcesWindowPresenter(view.PlayerResourcesWindowView, commandFactory);
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
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
        }

        public async UniTask StartGame()
        {
            var command = commandFactory.CreateStartGameCommand();
            await command.Execute();
        }

        // Event Handlers
        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            Debug.Log($"[GameWindowPresenter] Handling GameStartedEvent: {gameEvent.EventId}");
            
            // Start child presenters
            //boardPresenter.StartGame();
            
            // Simulate UI update time
            await UniTask.CompletedTask;
            
            Debug.Log($"[GameWindowPresenter] Completed handling GameStartedEvent: {gameEvent.EventId}");
        }
    }
}