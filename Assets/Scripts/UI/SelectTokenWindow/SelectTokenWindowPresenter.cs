using System;
using Models;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using UI.SelectTokenWindow.SelectBoardTokenPanel;
using UnityEngine;
using Command;
using UI.SelectTokenWindow.ChoosenBoardTokenPanel;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<TokenDetailsPanelClosedEvent>,
        IAsyncEventHandler<SelectedTokensConfirmedEvent>
    {
        private readonly SelectTokenWindowView view;
        private readonly SelectTokenWindowViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly IGameModelReader gameModelReader;
        private IDisposable disposables;

        private SelectBoardTokenPanelPresenter selectBoardTokenPanelPresenter;
        private ChoosenBoardTokenPanelPresenter choosenBoardTokenPanelPresenter;

        public SelectTokenWindowPresenter(SelectTokenWindowView view, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new SelectTokenWindowViewModel();
            this.gameModelReader = gameModelReader;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        /* ---- */

        private void InitializeChildMVP()
        {
            selectBoardTokenPanelPresenter = new SelectBoardTokenPanelPresenter(view.SelectBoardTokenPanelView, commandFactory, gameModelReader);
            choosenBoardTokenPanelPresenter = new ChoosenBoardTokenPanelPresenter(view.ChoosenBoardTokenPanelView, commandFactory, gameModelReader);
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

        private async UniTask HandleStateChange(SelectTokenWindowState state)
        {
            switch (state)
            {
                case SelectTokenWindowState.Closed:
                    view.OnCloseWindow();
                    break;
                case SelectTokenWindowState.DuringOpenAnimation:
                    viewModel.OnOpenWindowFinished();
                    break;
                case SelectTokenWindowState.DuringCloseAnimation:
                    viewModel.OnCloseWindowFinished();
                    break;
                case SelectTokenWindowState.Active:
                    view.OnOpenWindow();
                    break;
            }
        }


        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().Forget()).AddTo(ref d);
            view.OnAcceptButtonClicked.Subscribe(_ => HandleAcceptButtonClicked().Forget()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseTokenDetailsPanelCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleAcceptButtonClicked()
        {
            var command = commandFactory.CreateAcceptSelectedTokensCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelClosedEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<SelectedTokensConfirmedEvent>(this);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent tokenDetailsPanelOpenedEvent)
        {
            viewModel.OpenWindow(tokenDetailsPanelOpenedEvent.ResourceType);
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectTokenWindowState.Active);
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent tokenDetailsPanelClosedEvent)
        {
            viewModel.CloseWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectTokenWindowState.Closed);
        }

        public async UniTask HandleAsync(SelectedTokensConfirmedEvent selectedTokensConfirmedEvent)
        {
            viewModel.CloseWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectTokenWindowState.Closed);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}