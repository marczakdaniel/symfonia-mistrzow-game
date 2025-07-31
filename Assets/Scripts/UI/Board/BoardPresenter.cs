using Command;
using UI.Board.BoardMusicCardPanel;
using Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Events;
using UI.Board.BoardTokenPanel;
using R3;
using UI.Board.BoardEndTurnButton;
using UI.Board.BoardPlayerPanel;
using UI.Board.BoardTokenPanel.BoardToken;
using DefaultNamespace.Data;

namespace UI.Board
{
    public class BoardPresenter 
        : IAsyncEventHandler<GameStartedEvent>
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private readonly IGameModelReader gameModelReader;
        private BoardMusicCardPanelPresenter boardMusicCardPanelPresenter;
        private BoardTokenPresenter[] boardTokenPresenters = new BoardTokenPresenter[6];
        private BoardEndTurnButtonPresenter boardEndTurnButtonPresenter;
        private BoardPlayerPanelPresenter[] boardPlayerPanelPresenters = new BoardPlayerPanelPresenter[4];  
        private CommandFactory commandFactory;

        public BoardPresenter(BoardView view, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader;
            InitializeChildMCP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMCP()
        {
            boardMusicCardPanelPresenter = new BoardMusicCardPanelPresenter(view.BoardMusicCardPanelView, commandFactory, gameModelReader);
            for (int i = 0; i < boardTokenPresenters.Length; i++)
            {
                boardTokenPresenters[i] = new BoardTokenPresenter(view.BoardTokenPanelView[i], (ResourceType)i, commandFactory);
            }
            boardEndTurnButtonPresenter = new BoardEndTurnButtonPresenter(view.BoardEndTurnButtonView, commandFactory);
            for (int i = 0; i < boardPlayerPanelPresenters.Length; i++)
            {
                boardPlayerPanelPresenters[i] = new BoardPlayerPanelPresenter(view.BoardPlayerPanelViews[i], i, commandFactory);
            }
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

        // Event Handlers
        
        public async UniTask InitializeBoard()
        {
            await boardMusicCardPanelPresenter.InitializeBoard();
            await boardEndTurnButtonPresenter.OnGameStarted();
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            await InitializeBoard();
        }
    
    }
}