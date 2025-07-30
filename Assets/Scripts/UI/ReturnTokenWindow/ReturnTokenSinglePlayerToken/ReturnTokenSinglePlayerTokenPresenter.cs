
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using System;
using Assets.Scripts.UI.Elements;

namespace UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken
{
    public class ReturnTokenSinglePlayerTokenPresenter : 
        IDisposable,
        IAsyncEventHandler<TokenAddedToReturnTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromReturnTokensEvent>,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>,
        IAsyncEventHandler<ReturnTokensConfirmedEvent>
    {
        private readonly ReturnTokenSinglePlayerTokenViewModel viewModel;
        private readonly UniversalTokenElement view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public ReturnTokenSinglePlayerTokenPresenter(ResourceType token, UniversalTokenElement view, CommandFactory commandFactory)
        {
            this.viewModel = new ReturnTokenSinglePlayerTokenViewModel(token);
            this.commandFactory = commandFactory;
            this.view = view;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();
            ConnectModel(d);
            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleStateChange(ReturnTokenSinglePlayerTokenState state)
        {
            switch (state)
            {
                case ReturnTokenSinglePlayerTokenState.Disabled:
                    break;
                case ReturnTokenSinglePlayerTokenState.Active:
                    break;
                case ReturnTokenSinglePlayerTokenState.DuringReturnTokenInitialization:
                    view.Initialize(viewModel.Token, viewModel.Count);
                    viewModel.OnReturnTokenInitializationFinished();
                    break;
                case ReturnTokenSinglePlayerTokenState.DuringChangingTokenValue:
                    await view.UpdateValue(viewModel.Count, true);
                    viewModel.OnChangeTokenValueFinished();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClicked()
        {
            var command = commandFactory.CreateAddTokenToReturnTokensCommand(viewModel.Token);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<ReturnTokenWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenAddedToReturnTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromReturnTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokensConfirmedEvent>(this);
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent gameEvent)
        {
            viewModel.ReturnTokenInitialization(gameEvent.CurrentPlayerTokens[viewModel.Token]);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public async UniTask HandleAsync(TokenAddedToReturnTokensEvent gameEvent)
        {
            if (viewModel.Token != gameEvent.ResourceType) return;
            viewModel.ChangeTokenValue(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public async UniTask HandleAsync(TokenRemovedFromReturnTokensEvent gameEvent)
        {
            if (viewModel.Token != gameEvent.ResourceType) return;
            viewModel.ChangeTokenValue(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public async UniTask HandleAsync(ReturnTokensConfirmedEvent gameEvent)
        {
            viewModel.OnReturnTokenWindowClosed();
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Disabled);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

    }
}