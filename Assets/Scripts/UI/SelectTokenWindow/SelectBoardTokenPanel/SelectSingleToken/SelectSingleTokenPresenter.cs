using System;
using R3;
using Events;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Models;
using Command;
using JetBrains.Annotations;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public class SelectSingleTokenPresenter :
        IDisposable,
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<TokenAddedToSelectedTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromSelectedTokensEvent>
    {
        private readonly SelectSingleTokenView view;
        private readonly SelectSingleTokenViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly IGameModelReader gameModelReader;
        private IDisposable disposables;

        public SelectSingleTokenPresenter(SelectSingleTokenView view, ResourceType resourceType, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new SelectSingleTokenViewModel(resourceType);
            this.gameModelReader = gameModelReader;
            this.commandFactory = commandFactory;
            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OnCloseWindow()
        {
            // TODO: Close window animation
            viewModel.OnCloseWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectSingleTokenState.Disabled);
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

        private async UniTask HandleStateChange(SelectSingleTokenState state)
        {
            switch (state)
            {
                case SelectSingleTokenState.Disabled:
                    await view.OnDisabled();
                    break;
                case SelectSingleTokenState.DuringOpenAnimation:
                    await view.OnOpenAnimation(viewModel.ResourceType, viewModel.Count);
                    viewModel.OnOpenAnimationFinished();
                    break;
                case SelectSingleTokenState.DuringCloseAnimation:
                    await view.OnCloseAnimation();
                    viewModel.OnCloseAnimationFinished();
                    break;
                case SelectSingleTokenState.DuringAddingTokenAnimation:
                    await view.OnAddingTokenAnimation(viewModel.ResourceType, viewModel.Count);
                    viewModel.OnAddingTokenAnimationFinished();
                    break;
                case SelectSingleTokenState.DuringRemovingTokenAnimation:
                    await view.OnRemovingTokenAnimation(viewModel.ResourceType, viewModel.Count);
                    viewModel.OnRemovingTokenAnimationFinished();
                    break;
                case SelectSingleTokenState.Active:
                    await view.OnActivated();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClicked()
        {
            UnityEngine.Debug.Log($"HandleTokenClicked: {viewModel.ResourceType}");
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
            var value = gameEvent.CurrentTokenCounts[viewModel.ResourceType] - (gameEvent.ResourceType == viewModel.ResourceType ? 1 : 0);
            viewModel.OnOpenWindow(value);
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectSingleTokenState.Active);
        }

        public async UniTask HandleAsync(TokenAddedToSelectedTokensEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.ResourceType) {
                return;
            }

            viewModel.AddToken(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectSingleTokenState.Active);
        }

        public async UniTask HandleAsync(TokenRemovedFromSelectedTokensEvent gameEvent)
        {
            if (gameEvent.ResourceType != viewModel.ResourceType) {
                return;
            }

            viewModel.RemoveToken(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectSingleTokenState.Active);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}