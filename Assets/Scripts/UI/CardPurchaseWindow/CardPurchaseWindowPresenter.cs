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
        IAsyncEventHandler<CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent>, 
        IAsyncEventHandler<CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent>,
        IAsyncEventHandler<CardPurchaseWindowOpenedFromReservedEvent>,
        IAsyncEventHandler<CardPurchaseWindowClosedFromReservedEvent>,
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
        }


        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClick.Subscribe(_ => HandleCloseButtonClick().ToObservable()).AddTo(ref d);
            view.OnConfirmButtonClick.Subscribe(_ => HandleConfirmButtonClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClick()
        {
            if (viewModel.IsFromMusicCardDetailsPanel)
            {
                var command = commandFactory.CreateCloseCardPurchaseWindowCommandFromMusicCardDetailsPanel();
                await CommandService.Instance.ExecuteCommandAsync(command);
            }
            else
            {
                var command = commandFactory.CreateCloseCardPurchaseWindowCommandFromReserved(viewModel.CardIndex);
                await CommandService.Instance.ExecuteCommandAsync(command);
            }
        }

        private async UniTask HandleConfirmButtonClick()
        {
            var command = commandFactory.CreatePurchaseCardCommand(viewModel.MusicCardData.id);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedFromReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedFromReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent gameEvent)
        {
            viewModel.SetMusicCardData(gameEvent.MusicCardData);
            viewModel.SetIsFromMusicCardDetailsPanel(true);
            view.SetCardDetails(gameEvent.MusicCardData);
            view.Setup(gameEvent.CurrentPlayerTokens, gameEvent.CurrentCardTokens);
            view.SetCanBePurchased(gameEvent.CanBePurchased);
            await view.PlayOpenAnimationFromMusicCardDetailsPanel();
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent gameEvent)
        {
            await view.PlayCloseAnimationToMusicCardDetailsPanel();
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedFromReservedEvent gameEvent)
        {
            viewModel.SetMusicCardData(gameEvent.MusicCardData);
            viewModel.SetIsFromMusicCardDetailsPanel(false);
            viewModel.SetCardIndex(gameEvent.CardIndex);
            view.SetCardDetails(gameEvent.MusicCardData);
            view.Setup(gameEvent.CurrentPlayerTokens, gameEvent.CurrentCardTokens);
            view.SetCanBePurchased(gameEvent.CanBePurchased);
            await view.PlayOpenAnimationFromReserved(gameEvent.CardIndex);
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedFromReservedEvent gameEvent)
        {
            await view.PlayCloseAnimationToReserved();
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent gameEvent)
        {
            await view.PlayPurchaseAnimation(gameEvent.PlayerIndex);
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent gameEvent)
        {
            await view.PlayPurchaseAnimation(gameEvent.PlayerIndex);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}