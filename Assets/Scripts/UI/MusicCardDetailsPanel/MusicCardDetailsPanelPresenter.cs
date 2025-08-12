using Command;
using Cysharp.Threading.Tasks;
using Events;
using DefaultNamespace.Data;
using Models;
using R3;
using UnityEngine;
using System;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<MusicCardDetailsPanelOpenedEvent>, 
        IAsyncEventHandler<MusicCardDetailsPanelClosedEvent>,
        IAsyncEventHandler<CardReservedEvent>,
        IAsyncEventHandler<CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent>,
        IAsyncEventHandler<CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent>,
        IAsyncEventHandler<CardPurchasedFromBoardEvent>
    {
        private readonly MusicCardDetailsPanelView view;
        private readonly CommandFactory commandFactory;
        private readonly MusicCardDetailsPanelViewModel viewModel;
        private IDisposable disposable;

        public MusicCardDetailsPanelPresenter(MusicCardDetailsPanelView view, CommandFactory commandFactory) {
            this.view = view;
            viewModel = new MusicCardDetailsPanelViewModel();
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP() {
            var d = Disposable.CreateBuilder();
            ConnectView(d);
            disposable = d.Build();
        }
        private void ConnectView(DisposableBuilder d) {
            view.OnCloseButtonClick.Subscribe(_ => HandleCloseButtonClick().Forget()).AddTo(ref d);
            view.OnBuyButtonClick.Subscribe(_ => HandleBuyButtonClick().Forget()).AddTo(ref d);
            view.OnReserveButtonClick.Subscribe(_ => HandleReserveButtonClick().Forget()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClick() {
            var command = commandFactory.CreateCloseMusicCardDetailsPanelCommand(viewModel.MusicCardData.Id);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }
        private async UniTask HandleBuyButtonClick() 
        {
            var command = commandFactory.CreateOpenCardPurchaseWindowCommandFromMusicCardDetailsPanel(viewModel.MusicCardData.Id);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleReserveButtonClick() {
            var command = commandFactory.CreateReserveCardCommand(viewModel.MusicCardData.Id);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents() {
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelClosedEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent>(this, EventPriority.Low);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this, EventPriority.High);
        }


        public async UniTask HandleAsync(MusicCardDetailsPanelOpenedEvent musicCardDetailsPanelOpenedEvent) {
            viewModel.SetMusicCardData(musicCardDetailsPanelOpenedEvent.MusicCardData);
            view.SetCardDetails(musicCardDetailsPanelOpenedEvent.MusicCardData);
            view.SetCanBePurchased(musicCardDetailsPanelOpenedEvent.CanBePurchased);
            await view.PlayOpenFromBoardAnimation(musicCardDetailsPanelOpenedEvent.Level, musicCardDetailsPanelOpenedEvent.Position);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelClosedEvent musicCardDetailsPanelClosedEvent) {  
            viewModel.ClearMusicCardData();
            await view.PlayCloseToBoardAnimation();
        }

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent) {
            viewModel.ClearMusicCardData();
            await view.PlayCloseForReservedAnimation(cardReservedEvent.PlayerIndex);
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent cardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent) {
            await view.PlayMoveToCardPurchaseWindowAnimation();
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent cardPurchaseWindowClosedFromMusicCardDetailsPanelEvent) {
            await view.PlayMoveFromCardPurchaseWindowAnimation();
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent cardPurchasedFromBoardEvent) {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}