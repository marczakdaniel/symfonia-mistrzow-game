using System;
using System.Collections.Generic;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using UI.CardPurchaseWindow.CardPurchaseSingleToken;

namespace UI.CardPurchaseWindow
{
    public class CardPurchaseWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<CardPurchaseWindowOpenedEvent>, 
        IAsyncEventHandler<CardPurchaseWindowClosedEvent>,
        IAsyncEventHandler<CardPurchasedFromBoardEvent>,
        IAsyncEventHandler<CardPurchasedFromReserveEvent>
    {
        private readonly CardPurchaseWindowViewModel viewModel;
        private readonly CardPurchaseWindowView view;
        private readonly CommandFactory commandFactory;
        private readonly CardPurchaseSingleTokenPresenter[] cardPurchaseSingleTokenPresenters = new CardPurchaseSingleTokenPresenter[6];

        private IDisposable disposables;

        public CardPurchaseWindowPresenter(CardPurchaseWindowView view, CommandFactory commandFactory)
        {
            this.viewModel = new CardPurchaseWindowViewModel();
            this.view = view;
            this.commandFactory = commandFactory;   

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            for (int i = 0; i < 6; i++)
            {
                cardPurchaseSingleTokenPresenters[i] = new CardPurchaseSingleTokenPresenter((ResourceType)i, view.CardPurchaseSingleTokenViews[i], commandFactory);
            }
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectModel(d);
            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
        }

        public async UniTask HandleStateChange(CardPurchaseWindowState state)
        {
            switch (state)
            {
                case CardPurchaseWindowState.Closed:
                    view.Deactivate();
                    break;
                case CardPurchaseWindowState.Opened:
                    view.SetCardDetails(viewModel.MusicCardData);
                    view.Setup(viewModel.CurrentPlayerTokens, viewModel.CurrentCardTokens);
                    view.Activate();
                    break;
            }
        }


        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClick.Subscribe(_ => HandleCloseButtonClick().ToObservable()).AddTo(ref d);
            view.OnConfirmButtonClick.Subscribe(_ => HandleConfirmButtonClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClick()
        {
            var command = commandFactory.CreateCloseCardPurchaseWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleConfirmButtonClick()
        {
            var command = commandFactory.CreatePurchaseCardCommand(viewModel.MusicCardData.id);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedEvent gameEvent)
        {
            viewModel.OpenCardPurchaseWindow(gameEvent.MusicCardData, gameEvent.CurrentPlayerTokens, gameEvent.CurrentCardTokens);
            await UniTask.WaitUntil(() => viewModel.State.Value == CardPurchaseWindowState.Opened);
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedEvent gameEvent)
        {
            viewModel.CloseCardPurchaseWindow();
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent gameEvent)
        {
            viewModel.CloseCardPurchaseWindow();
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent gameEvent)
        {
            viewModel.CloseCardPurchaseWindow();
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}