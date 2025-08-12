using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using Command;
using R3;
namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckPresenter : 
        IDisposable,
        IAsyncEventHandler<PutCardOnBoardEvent>,
        IAsyncEventHandler<DeckCardReservedEvent>
    {
        private readonly BoardCardDeckView view;
        private readonly BoardCardDeckViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;
        public BoardCardDeckPresenter(BoardCardDeckView view, int level, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardCardDeckViewModel(level);
            this.commandFactory = commandFactory;
            view.SetLevel(level);

            InitializeMVP();
            SubscribeToEvents();
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
            view.OnClick.Subscribe(_ => HandleClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleClick()
        {
            var command = commandFactory.CreateOpenReserveDeckCardWindowCommand(viewModel.Level);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {   
            AsyncEventBus.Instance.Subscribe<PutCardOnBoardEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<DeckCardReservedEvent>(this, EventPriority.High);
        }

        public async UniTask HandleAsync(PutCardOnBoardEvent eventData)
        {
            if (viewModel.Level != eventData.Level)
            {
                return;
            }

            await view.PlayPutCardOnBoardAnimationWithHide(eventData.Position, 50);
            
            if (eventData.IsDeckCardEmpty)
            {
                await view.PlayDeckDisabledAnimation();
            }
        }

        public async UniTask HandleAsync(DeckCardReservedEvent eventData)
        {
            if (viewModel.Level != eventData.MusicCardData.Level)
            {
                return;
            }

            if (eventData.IsDeckCardEmpty)
            {
                await view.PlayDeckDisabledAnimation(); 
            }
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}