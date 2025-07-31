using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using System;

namespace UI.CardPurchaseWindow.CardPurchaseSingleToken
{
    public class CardPurchaseSingleTokenPresenter : 
        IDisposable, 
        IAsyncEventHandler<TokenAddedToCardPurchaseEvent>, 
        IAsyncEventHandler<CardPurchaseWindowOpenedEvent>,
        IAsyncEventHandler<CardPurchaseWindowClosedEvent>,
        IAsyncEventHandler<TokenRemovedFromCardPurchaseEvent>
    {
        private readonly CardPurchaseSingleTokenViewModel viewModel;
        private readonly CardPurchaseSingleTokenView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public CardPurchaseSingleTokenPresenter(ResourceType token, CardPurchaseSingleTokenView view, CommandFactory commandFactory)
        {
            this.viewModel = new CardPurchaseSingleTokenViewModel(token);
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
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
            viewModel.CurrentSelectedTokensCount.Subscribe(count => view.Initialize(viewModel.Token, count, viewModel.PlayerTokensCount)).AddTo(ref d);
        }

        private async UniTask HandleStateChange(CardPurchaseSingleTokenState state)
        {
            switch (state)
            {
                case CardPurchaseSingleTokenState.Disabled:
                    view.Deactivate();
                    break;
                case CardPurchaseSingleTokenState.Active:
                    view.Initialize(viewModel.Token, viewModel.CurrentSelectedTokensCount.Value, viewModel.PlayerTokensCount);
                    view.Activate();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnAddTokenClicked.Subscribe(_ => HandleAddTokenClicked().ToObservable()).AddTo(ref d);
            view.OnRemoveTokenClicked.Subscribe(_ => HandleRemoveTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAddTokenClicked()
        {
            var command = commandFactory.CreateAddTokenToCardPurchaseCommand(viewModel.Token);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleRemoveTokenClicked()
        {
            var command = commandFactory.CreateRemoveTokenFromCardPurchaseCommand(viewModel.Token);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenAddedToCardPurchaseEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromCardPurchaseEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(TokenAddedToCardPurchaseEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.Token)
            {
                return;
            }

            viewModel.SetCurrentSelectedTokensCount(gameEvent.CurrentTokenCount);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(TokenRemovedFromCardPurchaseEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.Token)
            {
                return;
            }

            viewModel.SetCurrentSelectedTokensCount(gameEvent.CurrentTokenCount);
            await UniTask.CompletedTask;
        }
        public async UniTask HandleAsync(CardPurchaseWindowOpenedEvent gameEvent)
        {
            UnityEngine.Debug.Log($"CardPurchaseWindowOpenedEvent: {gameEvent.CurrentPlayerTokens[viewModel.Token]}");
            viewModel.Initialize(viewModel.Token, 0, gameEvent.CurrentPlayerTokens[viewModel.Token]);
            await UniTask.WaitUntil(() => viewModel.State.Value == CardPurchaseSingleTokenState.Active);
        }

        public async UniTask HandleAsync(CardPurchaseWindowClosedEvent gameEvent)
        {
            viewModel.Close();
            await UniTask.WaitUntil(() => viewModel.State.Value == CardPurchaseSingleTokenState.Disabled);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}