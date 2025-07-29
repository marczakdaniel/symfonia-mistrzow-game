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
        IAsyncEventHandler<CardPurchasedEvent>
    {
        private readonly CardPurchaseWindowViewModel viewModel;
        private readonly CardPurchaseWindowView view;
        private readonly CommandFactory commandFactory;

        private List<CardPurchaseSingleTokenPresenter> cardPurchaseSingleTokenPresenters = new List<CardPurchaseSingleTokenPresenter>(6);
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
            foreach (var tokenType in Enum.GetValues(typeof(ResourceType)))
            {
                var tokenView = view.CardPurchaseSingleTokenViews[(int)tokenType];
                var tokenPresenter = new CardPurchaseSingleTokenPresenter((ResourceType)tokenType, tokenView, commandFactory);
                cardPurchaseSingleTokenPresenters.Add(tokenPresenter);
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
            AsyncEventBus.Instance.Subscribe<CardPurchasedEvent>(this);
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedEvent gameEvent)
        {
            viewModel.OpenCardPurchaseWindow(gameEvent.MusicCardData);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedEvent gameEvent)
        {
            viewModel.CloseCardPurchaseWindow();
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchasedEvent gameEvent)
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