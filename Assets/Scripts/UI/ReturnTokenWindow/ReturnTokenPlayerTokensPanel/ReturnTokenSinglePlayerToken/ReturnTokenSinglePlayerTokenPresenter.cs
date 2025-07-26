
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using System;

namespace UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel.ReturnTokenSinglePlayerToken
{
    public class ReturnTokenSinglePlayerTokenPresenter : 
        IDisposable,
        IAsyncEventHandler<TokenAddedToReturnTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromReturnTokensEvent>,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>
    {
        private readonly ReturnTokenSinglePlayerTokenViewModel viewModel;
        private readonly ReturnTokenSinglePlayerTokenView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public ReturnTokenSinglePlayerTokenPresenter(ResourceType token, ReturnTokenSinglePlayerTokenView view, CommandFactory commandFactory)
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
            viewModel.Count.Subscribe(count => view.SetToken(viewModel.Token, count)).AddTo(ref d);
        }

        private async UniTask HandleStateChange(ReturnTokenSinglePlayerTokenState state)
        {
            switch (state)
            {
                case ReturnTokenSinglePlayerTokenState.Disabled:
                    view.gameObject.SetActive(false);
                    break;
                case ReturnTokenSinglePlayerTokenState.Active:
                    view.gameObject.SetActive(true);
                    break;
                case ReturnTokenSinglePlayerTokenState.DuringAddingTokenAnimation:
                    view.gameObject.SetActive(true);
                    view.SetToken(viewModel.Token, viewModel.Count.Value);
                    viewModel.OnAddTokenFinished();
                    break;
                case ReturnTokenSinglePlayerTokenState.DuringRemovingTokenAnimation:    
                    view.gameObject.SetActive(true);
                    view.SetToken(viewModel.Token, viewModel.Count.Value);
                    viewModel.OnRemoveTokenFinished();
                    break;
                case ReturnTokenSinglePlayerTokenState.DuringAcceptingTokensAnimation:
                    view.gameObject.SetActive(true);
                    viewModel.OnAcceptTokensFinished();
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
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent gameEvent)
        {
            viewModel.OnReturnTokenWindowOpened(gameEvent.CurrentPlayerTokens[viewModel.Token]);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public async UniTask HandleAsync(TokenAddedToReturnTokensEvent gameEvent)
        {
            if (viewModel.Token != gameEvent.ResourceType) return;
            viewModel.AddToken(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public async UniTask HandleAsync(TokenRemovedFromReturnTokensEvent gameEvent)
        {
            if (viewModel.Token != gameEvent.ResourceType) return;
            viewModel.RemoveToken(gameEvent.CurrentTokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSinglePlayerTokenState.Active);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

    }
}