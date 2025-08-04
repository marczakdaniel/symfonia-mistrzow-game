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
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnAddTokenClicked.Subscribe(_ => HandleAddTokenClicked().ToObservable()).AddTo(ref d);
            view.OnRemoveTokenClicked.Subscribe(_ => HandleRemoveTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAddTokenClicked()
        {
            var command = commandFactory.CreateAddTokenToCardPurchaseCommand(viewModel.CardId, viewModel.Token);
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
            AsyncEventBus.Instance.Subscribe<TokenAddedToCardPurchaseEvent>(this);
        }

        public async UniTask HandleAsync(TokenAddedToCardPurchaseEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.Token)
            {
                return;
            }
            view.UpdateCurrentSelectedTokensCount(gameEvent.CurrentTokenCount);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(TokenRemovedFromCardPurchaseEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.Token)
            {
                return;
            }
            view.UpdateCurrentSelectedTokensCount(gameEvent.CurrentTokenCount);
            await UniTask.CompletedTask;
        }
        public async UniTask HandleAsync(CardPurchaseWindowOpenedEvent gameEvent)
        {
            viewModel.SetCardId(gameEvent.MusicCardData.Id);
            view.Initialize(viewModel.Token, gameEvent.InitialSelectedTokens[viewModel.Token], gameEvent.TokensNeededToPurchase[viewModel.Token]);
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}