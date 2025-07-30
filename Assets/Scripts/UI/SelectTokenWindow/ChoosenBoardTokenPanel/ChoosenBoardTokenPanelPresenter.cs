using System;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;
using UnityEngine;

namespace UI.SelectTokenWindow.ChoosenBoardTokenPanel
{
    public class ChoosenBoardTokenPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<TokenAddedToSelectedTokensEvent>,
        IAsyncEventHandler<TokenRemovedFromSelectedTokensEvent>,
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<TokenDetailsPanelClosedEvent>
    {
        private readonly ChoosenBoardTokenPanelView view;
        private readonly ChoosenBoardTokenPanelViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();
        private IDisposable disposables;

        public ChoosenBoardTokenPanelPresenter(ChoosenBoardTokenPanelView view, CommandFactory commandFactory)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.viewModel = new ChoosenBoardTokenPanelViewModel();
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));

            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OpenPanel(ResourceType? selectedToken)
        {
            viewModel.OnOpenAnimation(selectedToken);
            await UniTask.WaitUntil(() => viewModel.State.Value == ChoosenBoardTokenPanelState.Active);
        }

        public async UniTask ClosePanel()
        {
            viewModel.OnCloseAnimation();
            await UniTask.WaitUntil(() => viewModel.State.Value == ChoosenBoardTokenPanelState.Disabled);
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

        private async UniTask HandleStateChange(ChoosenBoardTokenPanelState state)
        {
            switch (state)
            {
                case ChoosenBoardTokenPanelState.Disabled:  
                    await view.OnDisabled();
                    break;
                case ChoosenBoardTokenPanelState.DuringOpenAnimation:
                    await view.OnOpenAnimation(viewModel.SelectedTokens);
                    viewModel.OnOpenAnimationFinished();
                    break;  
                case ChoosenBoardTokenPanelState.DuringCloseAnimation:
                    await view.OnCloseAnimation();
                    viewModel.OnCloseAnimationFinished();
                    break;
                case ChoosenBoardTokenPanelState.DuringAddingTokenAnimation:
                    await view.OnAddingTokenAnimation(viewModel.SelectedTokens);
                    viewModel.OnAddingTokenAnimationFinished();
                    break;
                case ChoosenBoardTokenPanelState.DuringRemovingTokenAnimation:
                    await view.OnRemovingTokenAnimation(viewModel.SelectedTokens);
                    viewModel.OnRemovingTokenAnimationFinished();
                    break;
                case ChoosenBoardTokenPanelState.Active:
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
            var command = commandFactory.CreateRemoveTokenFromSelectedTokensCommand(lastSelectedToken.Value);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenAddedToSelectedTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenRemovedFromSelectedTokensEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelClosedEvent>(this);
        }

        public async UniTask HandleAsync(TokenAddedToSelectedTokensEvent gameEvent)
        {
            viewModel.AddToken(gameEvent.CurrentSelectedTokens);
            await UniTask.WaitUntil(() => viewModel.State.Value == ChoosenBoardTokenPanelState.Active);
        }

        public async UniTask HandleAsync(TokenRemovedFromSelectedTokensEvent gameEvent)
        {
            viewModel.RemoveToken(gameEvent.CurrentSelectedTokens);
            await UniTask.WaitUntil(() => viewModel.State.Value == ChoosenBoardTokenPanelState.Active);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent gameEvent)
        {
            await OpenPanel(gameEvent.ResourceType);
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent gameEvent)
        {
            await ClosePanel();
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}