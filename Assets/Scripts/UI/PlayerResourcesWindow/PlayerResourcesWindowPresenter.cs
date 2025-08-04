using System;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using Command;

namespace UI.PlayerResourcesWindow
{
    public class PlayerResourcesWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<PlayerResourcesWindowOpenedEvent>,
        IAsyncEventHandler<PlayerResourcesWindowClosedEvent>,
        IAsyncEventHandler<CardPurchasedFromReserveEvent>,
        IAsyncEventHandler<CardPurchaseWindowOpenedFromReservedEvent>,
        IAsyncEventHandler<CardPurchaseWindowClosedFromReservedEvent>
    {
        private readonly PlayerResourcesWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;
        
        public PlayerResourcesWindowPresenter(PlayerResourcesWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
            view.OnCardClicked.Subscribe(x => HandleCardClicked(x.Item1, x.Item2).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateClosePlayerResourcesWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleCardClicked(string cardId, int cardIndex)
        {
            UnityEngine.Debug.Log($"Card clicked: {cardId}, card index: {cardIndex}");
            var command = commandFactory.CreateOpenCardPurchaseWindowCommandFromReserved(cardId, cardIndex);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<PlayerResourcesWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<PlayerResourcesWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedFromReservedEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedFromReservedEvent>(this, EventPriority.Low);
        }

        public async UniTask HandleAsync(PlayerResourcesWindowOpenedEvent playerResourcesWindowOpenedEvent)
        {
            view.Initialize(playerResourcesWindowOpenedEvent.PlayerName, playerResourcesWindowOpenedEvent.NumberOfPoints, playerResourcesWindowOpenedEvent.CurrentPlayerTokens, playerResourcesWindowOpenedEvent.CurrentPlayerCards, playerResourcesWindowOpenedEvent.ReservedMusicCards);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(PlayerResourcesWindowClosedEvent playerResourcesWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent cardPurchasedFromReserveEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(CardPurchaseWindowOpenedFromReservedEvent cardPurchaseWindowOpenedFromReservedEvent)
        {
            view.HideCard(cardPurchaseWindowOpenedFromReservedEvent.CardIndex).Forget();
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedFromReservedEvent cardPurchaseWindowClosedFromReservedEvent)
        {
            view.ShowCard(cardPurchaseWindowClosedFromReservedEvent.CardIndex);
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}