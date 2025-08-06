using System;
using System.Collections.Generic;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;
using UI.Board.BoardPlayerPanel;
using Unity.VisualScripting;

namespace UI.Board.BoardPlayerPanel
{
    public class BoardPlayerPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<GameStartedEvent>,
        IAsyncEventHandler<TurnStartedEvent>,
        IAsyncEventHandler<CardPurchasedFromBoardEvent>,
        IAsyncEventHandler<CardPurchasedFromReserveEvent>,
        IAsyncEventHandler<ConcertCardClaimedEvent>
    {
        private readonly BoardPlayerPanelViewModel viewModel;
        private readonly BoardPlayerPanelView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public BoardPlayerPanelPresenter(BoardPlayerPanelView view, int index, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardPlayerPanelViewModel(index);
            this.commandFactory = commandFactory;
            
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = new DisposableBuilder();

            ConnectView(d);

            disposables = d.Build();
        }


        private void ConnectView(DisposableBuilder d)
        {
            view.OnClick.Subscribe(_ => HandlePlayerResourcesButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandlePlayerResourcesButtonClicked()
        {
            var command = commandFactory.CreateOpenPlayerResourcesWindowCommand(viewModel.PlayerId);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
            AsyncEventBus.Instance.Subscribe<ConcertCardClaimedEvent>(this);
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            if (gameEvent.PlayerIds.Length <= viewModel.Index)
            {
                return;
            }

            viewModel.Initialize(gameEvent.PlayerIds[viewModel.Index], gameEvent.PlayerAvatars[viewModel.Index]);

            view.SetPlayerImage(viewModel.PlayerImage);
            view.SetPlayerPoints(0 );
            view.SetActivePlayerIndicator(false);
            await view.PlayActivateAnimation();
        }

        public async UniTask HandleAsync(TurnStartedEvent turnEvent)
        {
            if (turnEvent.CurrentPlayerId == viewModel.PlayerId)
            {
                viewModel.SetCurrentPlayer(true);
                await view.PlayCurrentPlayerAnimation();
            }
            else if (viewModel.IsCurrentPlayer && turnEvent.CurrentPlayerId != viewModel.PlayerId)
            {
                viewModel.SetCurrentPlayer(false);
                await view.PlayStopCurrentPlayerAnimation();
            }
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent cardPurchasedFromBoardEvent)
        {
            if (!viewModel.IsCurrentPlayer)
            {
                return;
            }
            view.SetPlayerPoints(cardPurchasedFromBoardEvent.Points);
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent cardPurchasedFromReserveEvent)
        {
            if (!viewModel.IsCurrentPlayer)
            {
                return;
            }
            view.SetPlayerPoints(cardPurchasedFromReserveEvent.Points);
        }

        public async UniTask HandleAsync(ConcertCardClaimedEvent concertCardClaimedEvent)
        {
            if (!viewModel.IsCurrentPlayer)
            {
                return;
            }
            view.SetPlayerPoints(concertCardClaimedEvent.Points);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}