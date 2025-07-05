using UI.Board;
using Command;
using UI.MusicCardDetailsPanel;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.GameWindow
{
    public class GameWindowPresenter : IAsyncEventHandler<GameStartedEvent>
    {
        private readonly GameWindowView view;
        private readonly GameWindowViewModel viewModel = new GameWindowViewModel();
        private BoardPresenter boardPresenter;
        private MusicCardDetailsPanelPresenter musicCardDetailsPanelPresenter;
        private CommandFactory commandFactory;

        public GameWindowPresenter(GameWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
        }

        private void InitializeChildMVP()
        {
            boardPresenter = new BoardPresenter(view.BoardView, commandFactory);
            musicCardDetailsPanelPresenter = new MusicCardDetailsPanelPresenter(view.MusicCardDetailsPanelView, commandFactory);
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
            SubscribeToEvents();
            var command = commandFactory.CreateStartGameCommand();
            await command.Execute();
        }

        // Event Handlers
        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            Debug.Log($"[GameWindowPresenter] Handling GameStartedEvent: {gameEvent.EventId}");
            
            // Start child presenters
            boardPresenter.StartGame();
            
            // Simulate UI update time
            await UniTask.Delay(100);
            
            Debug.Log($"[GameWindowPresenter] Completed handling GameStartedEvent: {gameEvent.EventId}");
        }
    }
}