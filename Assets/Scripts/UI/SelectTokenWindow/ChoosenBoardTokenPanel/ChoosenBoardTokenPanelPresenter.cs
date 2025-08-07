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

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposables = d.Build();
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
            await view.OnAddingTokenAnimation(viewModel.SelectedTokens);
        }

        public async UniTask HandleAsync(TokenRemovedFromSelectedTokensEvent gameEvent)
        {
            viewModel.RemoveToken(gameEvent.CurrentSelectedTokens);
            await view.OnRemovingTokenAnimation(viewModel.SelectedTokens);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent gameEvent)
        {
            viewModel.OnOpenAnimation(gameEvent.ResourceType);
            await view.OnOpenAnimation(viewModel.SelectedTokens);
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent gameEvent)
        {
            viewModel.OnCloseAnimation();
            await view.OnCloseAnimation();
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}