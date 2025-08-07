using System;
using Assets.Scripts.UI.Elements;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;

namespace UI.SelectTokenWindow.SelectBoardToken
{
    public class SelectBoardTokenPresenter :
        IDisposable,
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<TokenAddedToSelectedTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromSelectedTokensEvent>
    {
        private readonly UniversalTokenElement view;
        private readonly SelectBoardTokenViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public SelectBoardTokenPresenter(UniversalTokenElement view, ResourceType resourceType, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.viewModel = new SelectBoardTokenViewModel(resourceType);

            Initialize();
            SubscribeToEvents();
        }

        private void Initialize()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClicked()
        {
            var command = commandFactory.CreateAddTokenToSelectedTokensCommand(viewModel.ResourceType);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        public void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenAddedToSelectedTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromSelectedTokensEvent>(this);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent gameEvent)
        {
            var value = gameEvent.CurrentTokenCounts[viewModel.ResourceType];
            if (gameEvent.ResourceType.HasValue && gameEvent.ResourceType.Value == viewModel.ResourceType) 
            {
                value -= 1;
            }
            view.Initialize(viewModel.ResourceType, value);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(TokenAddedToSelectedTokensEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.ResourceType) {
                return;
            }
            view.UpdateValue(gameEvent.CurrentTokenCount, true).Forget();
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(TokenRemovedFromSelectedTokensEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.ResourceType) {
                return;
            }
            view.UpdateValue(gameEvent.CurrentTokenCount, true).Forget();
            await UniTask.CompletedTask;
        }   

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}