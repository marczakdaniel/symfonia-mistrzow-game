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
        IAsyncEventHandler<CardPurchaseWindowOpenedEvent>
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
            viewModel.CurrentSelectedTokensCount.Subscribe(count => view.SetToken(viewModel.Token, count, viewModel.PlayerTokensCount.Value)).AddTo(ref d);
            viewModel.PlayerTokensCount.Subscribe(count => view.SetToken(viewModel.Token, viewModel.CurrentSelectedTokensCount.Value, count)).AddTo(ref d);
        }

        private async UniTask HandleStateChange(CardPurchaseSingleTokenState state)
        {
            switch (state)
            {
                case CardPurchaseSingleTokenState.Disabled:
                    view.Deactivate();
                    break;
                case CardPurchaseSingleTokenState.Active:
                    view.Activate();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClicked()
        {
            var command = commandFactory.CreateAddTokenToCardPurchaseCommand(viewModel.Token);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenAddedToCardPurchaseEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchaseWindowOpenedEvent>(this);
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

        public async UniTask HandleAsync(CardPurchaseWindowOpenedEvent gameEvent)
        {
            UnityEngine.Debug.Log($"CardPurchaseWindowOpenedEvent: {gameEvent.CurrentPlayerTokens[viewModel.Token]}");
            viewModel.SetPlayerTokensCount(gameEvent.CurrentPlayerTokens[viewModel.Token]);
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}