using Command;
using DefaultNamespace.Data;
using R3;
using System;
using Cysharp.Threading.Tasks;
using Events;
namespace UI.ReturnTokenWindow.ReturnTokenSelectedPanel
{
    public class ReturnTokenSelectedPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<TokenAddedToReturnTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromReturnTokensEvent>,
        IAsyncEventHandler<ReturnTokensConfirmedEvent>,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>
    {
        private readonly ReturnTokenSelectedPanelView view;
        private readonly ReturnTokenSelectedPanelViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();
        private IDisposable disposables;

        public ReturnTokenSelectedPanelPresenter(ReturnTokenSelectedPanelView view, CommandFactory commandFactory)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.viewModel = new ReturnTokenSelectedPanelViewModel();
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));

            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OpenPanel()
        {
            viewModel.OnOpenAnimation();
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSelectedPanelState.Active);
        }

        public async UniTask ClosePanel()
        {
            viewModel.OnCloseAnimation();
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSelectedPanelState.Disabled);
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
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(subscriptions);
        }

        private async UniTask HandleStateChange(ReturnTokenSelectedPanelState state)
        {
            switch (state)
            {
                case ReturnTokenSelectedPanelState.Disabled:  
                    await view.OnDisabled();
                    break;
                case ReturnTokenSelectedPanelState.DuringOpenAnimation:
                    await view.OnOpenAnimation(viewModel.SelectedTokens);
                    viewModel.OnOpenAnimationFinished();
                    break;  
                case ReturnTokenSelectedPanelState.DuringCloseAnimation:
                    await view.OnCloseAnimation();
                    viewModel.OnCloseAnimationFinished();
                    break;
                case ReturnTokenSelectedPanelState.DuringAddingTokenAnimation:
                    await view.OnAddingTokenAnimation(viewModel.SelectedTokens);
                    viewModel.OnAddingTokenAnimationFinished();
                    break;
                case ReturnTokenSelectedPanelState.DuringRemovingTokenAnimation:
                    await view.OnRemovingTokenAnimation(viewModel.SelectedTokens);
                    viewModel.OnRemovingTokenAnimationFinished();
                    break;
                case ReturnTokenSelectedPanelState.Active:
                    await view.OnActivated();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnPanelClicked.Subscribe(_ => HandlePanelClick().ToObservable()).AddTo(subscriptions);
        }

        private async UniTask HandlePanelClick()
        {
            var lastSelectedToken = viewModel.GetLastSelectedToken();
            if (lastSelectedToken == null)
            {
                return;
            }
            var command = commandFactory.CreateRemoveTokenFromReturnTokensCommand(lastSelectedToken.Value);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenAddedToReturnTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromReturnTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokensConfirmedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokenWindowOpenedEvent>(this);
        }

        public async UniTask HandleAsync(TokenAddedToReturnTokensEvent gameEvent)
        {
            viewModel.AddToken(gameEvent.CurrentReturnTokens);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSelectedPanelState.Active);
        }

        public async UniTask HandleAsync(TokenRemovedFromReturnTokensEvent gameEvent)
        {
            viewModel.RemoveToken(gameEvent.CurrentReturnTokens);
            await UniTask.WaitUntil(() => viewModel.State.Value == ReturnTokenSelectedPanelState.Active);
        }

        public async UniTask HandleAsync(ReturnTokensConfirmedEvent gameEvent)
        {
            await ClosePanel();
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent gameEvent)
        {
            await OpenPanel();
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}