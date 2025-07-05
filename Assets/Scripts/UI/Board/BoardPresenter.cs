using Command;
using UI.Board.BoardMusicCardPanel;
using Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Board
{
    public class BoardPresenter : IAsyncEventHandler<GameStartedEvent>, IAsyncEventHandler<BoardUpdatedEvent>, IAsyncEventHandler<CardPurchasedEvent>, IAsyncEventHandler<CardReservedEvent>
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private readonly IGameModelReader gameModelReader;
        private BoardMusicCardPanelPresenter boardMusicCardPanelPresenter;
        private CommandFactory commandFactory;

        public BoardPresenter(BoardView view, CommandFactory commandFactory, IGameModelReader gameModelReader = null)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader ?? GameModel.Instance;
            InitializeChildMCP();
            InitializeMVP();
        }

        private void InitializeChildMCP()
        {
            boardMusicCardPanelPresenter = new BoardMusicCardPanelPresenter(view.BoardMusicCardPanelView, commandFactory, gameModelReader);
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
            AsyncEventBus.Instance.Subscribe<BoardUpdatedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
        }

        public void StartGame()
        {
            SubscribeToEvents();
            boardMusicCardPanelPresenter.StartGame();
        }

        // Event Handlers
        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            Debug.Log($"[BoardPresenter] Handling GameStartedEvent: {gameEvent.EventId}");
            
            // Refresh board view based on game model
            await RefreshBoardView();
            
            // Simulate UI update time
            await UniTask.Delay(100);
            
            Debug.Log($"[BoardPresenter] Completed handling GameStartedEvent: {gameEvent.EventId}");
        }

        public async UniTask HandleAsync(BoardUpdatedEvent gameEvent)
        {
            Debug.Log($"[BoardPresenter] Handling BoardUpdatedEvent: {gameEvent.EventId}");
            
            // Refresh board view
            await RefreshBoardView();
            
            // Simulate UI update time
            await UniTask.Delay(100);
            
            Debug.Log($"[BoardPresenter] Completed handling BoardUpdatedEvent: {gameEvent.EventId}");
        }

        public async UniTask HandleAsync(CardPurchasedEvent gameEvent)
        {
            Debug.Log($"[BoardPresenter] Handling CardPurchasedEvent: {gameEvent.EventId}");
            
            // Refresh board view to reflect card removal
            await RefreshBoardView();
            
            // Simulate UI update time
            await UniTask.Delay(100);
            
            Debug.Log($"[BoardPresenter] Completed handling CardPurchasedEvent: {gameEvent.EventId}");
        }

        public async UniTask HandleAsync(CardReservedEvent gameEvent)
        {
            Debug.Log($"[BoardPresenter] Handling CardReservedEvent: {gameEvent.EventId}");
            
            // Refresh board view to reflect card removal
            await RefreshBoardView();
            
            // Simulate UI update time
            await UniTask.Delay(100);
            
            Debug.Log($"[BoardPresenter] Completed handling CardReservedEvent: {gameEvent.EventId}");
        }

        private async UniTask RefreshBoardView()
        {
            // Read latest board state from game model
            var boardCards = gameModelReader.GetCurrentBoardCards();
            
            // TODO: Update view model when methods are available
            // viewModel.UpdateBoardCards(boardCards);
            
            // TODO: Refresh child presenters when methods are available
            // boardMusicCardPanelPresenter.RefreshView();
            
            // Simulate view update time
            await UniTask.Delay(50);
        }
    }
}