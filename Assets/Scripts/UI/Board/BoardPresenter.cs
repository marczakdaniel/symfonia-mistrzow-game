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
using System;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using UI.Board.BoardMusicCardPanel.BoardCardDeck;

namespace UI.Board
{
    public class BoardPresenter 
        : IAsyncEventHandler<GameStartedEvent>
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private BoardTokenPresenter[] boardTokenPresenters = new BoardTokenPresenter[6];
        private BoardEndTurnButtonPresenter boardEndTurnButtonPresenter;
        private BoardPlayerPanelPresenter[] boardPlayerPanelPresenters = new BoardPlayerPanelPresenter[4];  

        private BoardMusicCardPresenter[] level1CardPresenters;
        private BoardMusicCardPresenter[] level2CardPresenters;
        private BoardMusicCardPresenter[] level3CardPresenters;
        private BoardCardDeckPresenter level1CardDeckPresenter;
        private BoardCardDeckPresenter level2CardDeckPresenter;
        private BoardCardDeckPresenter level3CardDeckPresenter;
        
        private CommandFactory commandFactory;

        private IDisposable disposable;

        public BoardPresenter(BoardView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            InitializeChildMCP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMCP()
        {
            for (int i = 0; i < boardTokenPresenters.Length; i++)
            {
                boardTokenPresenters[i] = new BoardTokenPresenter(view.BoardTokenPanelView[i], (ResourceType)i, commandFactory);
            }
            boardEndTurnButtonPresenter = new BoardEndTurnButtonPresenter(view.BoardEndTurnButtonView, commandFactory);
            for (int i = 0; i < boardPlayerPanelPresenters.Length; i++)
            {
                boardPlayerPanelPresenters[i] = new BoardPlayerPanelPresenter(view.BoardPlayerPanelViews[i], i, commandFactory);
            }

            level1CardPresenters = new BoardMusicCardPresenter[view.Level1Cards.Length];
            for (int i = 0; i < view.Level1Cards.Length; i++)
            {
                level1CardPresenters[i] = new BoardMusicCardPresenter(view.Level1Cards[i], 1, i, commandFactory);
            }
            level2CardPresenters = new BoardMusicCardPresenter[view.Level2Cards.Length];
            for (int i = 0; i < view.Level2Cards.Length; i++)
            {
                level2CardPresenters[i] = new BoardMusicCardPresenter(view.Level2Cards[i], 2, i, commandFactory);
            }
            level3CardPresenters = new BoardMusicCardPresenter[view.Level3Cards.Length];
            for (int i = 0; i < view.Level3Cards.Length; i++)
            {
                level3CardPresenters[i] = new BoardMusicCardPresenter(view.Level3Cards[i], 3, i, commandFactory);
            }

            level1CardDeckPresenter = new BoardCardDeckPresenter(view.Level1CardDeck);
            level2CardDeckPresenter = new BoardCardDeckPresenter(view.Level2CardDeck);
            level3CardDeckPresenter = new BoardCardDeckPresenter(view.Level3CardDeck);
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectModel(d);
            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {

        }
        
        private void ConnectView(DisposableBuilder d)
        {
            view.OnBoardConcertCardButtonClicked.Subscribe(_ => HandleBoardConcertCardButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleBoardConcertCardButtonClicked()
        {
            var command = commandFactory.CreateOpenConcertCardsWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this, EventPriority.Critical);
        }

        // Event Handlers
        
        public async UniTask InitializeBoard()
        {
            await boardEndTurnButtonPresenter.OnGameStarted();
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            await view.PlayOpenAnimation();
            await InitializeBoard();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    
    }
}