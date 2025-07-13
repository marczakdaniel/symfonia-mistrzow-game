using Command;
using UI.Board.BoardMusicCardPanel;
using Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Events;
using UI.Board.BoardTokenPanel;
using R3;

namespace UI.Board
{
    public class BoardPresenter 
        : IAsyncEventHandler<GameStartedEvent>
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private readonly IGameModelReader gameModelReader;
        private BoardMusicCardPanelPresenter boardMusicCardPanelPresenter;
        private BoardTokenPanelPresenter boardTokenPanelPresenter;
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
            boardTokenPanelPresenter = new BoardTokenPanelPresenter(view.BoardTokenPanelView, commandFactory);
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
            await boardTokenPanelPresenter.PutTokensOnBoard();
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            await InitializeBoard();
        }
    
    }
}