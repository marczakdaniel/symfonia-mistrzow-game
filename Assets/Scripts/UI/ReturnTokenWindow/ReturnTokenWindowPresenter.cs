using System;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using UI.ReturnTokenWindow.ReturnTokenSelectedPanel;
using UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken;

namespace UI.ReturnTokenWindow
{
    public class ReturnTokenWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>,
        IAsyncEventHandler<ReturnTokensConfirmedEvent>,
        IAsyncEventHandler<TokenAddedToReturnTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromReturnTokensEvent>
    {
        private readonly ReturnTokenWindowViewModel viewModel;
        private readonly ReturnTokenWindowView view;
        private ReturnTokenSinglePlayerTokenPresenter[] returnTokenSinglePlayerTokenPresenters;
        private ReturnTokenSelectedPanelPresenter returnTokenSelectedPanelPresenter;
        private IDisposable disposable;
        private readonly CommandFactory commandFactory;

        public ReturnTokenWindowPresenter(ReturnTokenWindowView view, CommandFactory commandFactory)
        {
            this.viewModel = new ReturnTokenWindowViewModel();
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            returnTokenSinglePlayerTokenPresenters = new ReturnTokenSinglePlayerTokenPresenter[6];
            int i = 0;
            foreach (var token in Enum.GetValues(typeof(ResourceType)))
            {
                returnTokenSinglePlayerTokenPresenters[i] = new ReturnTokenSinglePlayerTokenPresenter((ResourceType)token, view.ReturnTokenSinglePlayerTokenPrefab[i], commandFactory);
                i++;
            }
            returnTokenSelectedPanelPresenter = new ReturnTokenSelectedPanelPresenter(view.ReturnTokenSelectedPanelPrefab, commandFactory);
        }

        private void InitializeMVP()
        {
            var d = new DisposableBuilder();
            ConnectModel(d);
            ConnectView(d);
            disposable = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
            viewModel.AllPlayerTokensCount.Subscribe(count => view.SetAllTokenCount(count)).AddTo(ref d);
        }

        private async UniTask HandleStateChange(ReturnTokenWindowState state)
        {
            switch (state)
            {
                case ReturnTokenWindowState.DuringOpenAnimation:
                    await view.PlayOpenAnimation();
                    viewModel.OnOpenAnimationFinished();
                    break;
                case ReturnTokenWindowState.DuringCloseAnimation:
                    await view.PlayCloseAnimation();
                    viewModel.OnCloseAnimationFinished();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnAcceptClicked.Subscribe(_ => HandleAcceptClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAcceptClicked()
        {
            var command = commandFactory.CreateConfirmReturnTokensCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<ReturnTokenWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokensConfirmedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenAddedToReturnTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromReturnTokensEvent>(this);
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent gameEvent)
        {
            viewModel.UpdateAllPlayerTokensCount(gameEvent.AllPlayerTokensCount);
            viewModel.OpenWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenWindowState.Active);
        }

        public async UniTask HandleAsync(ReturnTokensConfirmedEvent gameEvent)
        {
            viewModel.CloseWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenWindowState.Disabled);
        }

        public async UniTask HandleAsync(TokenAddedToReturnTokensEvent gameEvent)
        {
            viewModel.UpdateAllPlayerTokensCount(gameEvent.AllPlayerTokensCount);
        }

        public async UniTask HandleAsync(TokenRemovedFromReturnTokensEvent gameEvent)
        {
            viewModel.UpdateAllPlayerTokensCount(gameEvent.AllPlayerTokensCount);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}