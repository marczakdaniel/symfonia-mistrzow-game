using System;
using System.Collections.Generic;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;
using UI.Board.BoardPlayerPanel;
using Unity.VisualScripting;

namespace UI.Board.BoardPlayerPanel
{
    public class BoardPlayerPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<GameStartedEvent>,
        IAsyncEventHandler<TurnStartedEvent>
    {
        private readonly BoardPlayerPanelViewModel viewModel;
        private readonly BoardPlayerPanelView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public BoardPlayerPanelPresenter(BoardPlayerPanelView view, int index, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardPlayerPanelViewModel(index);
            this.commandFactory = commandFactory;
            
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = new DisposableBuilder();

            ConnectModel(d);
            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {
            viewModel.State.Subscribe(state => HandleStateChanged(state).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleStateChanged(BoardPlayerPanelState state)
        {
            switch (state)
            {
                case BoardPlayerPanelState.Disabled:
                    view.gameObject.SetActive(false);
                    break;
                case BoardPlayerPanelState.Enabled:
                    view.SetPlayerImage(viewModel.PlayerImage);
                    view.SetPlayerPoints(viewModel.PlayerPoints.Value);
                    view.SetActivePlayerIndicator(false);
                    view.gameObject.SetActive(true);
                    break;
                case BoardPlayerPanelState.CurrentPlayer:
                    view.SetActivePlayerIndicator(true);
                    view.gameObject.SetActive(true);
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnClick.Subscribe(_ => HandlePlayerResourcesButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandlePlayerResourcesButtonClicked()
        {
            var command = commandFactory.CreateOpenPlayerResourcesWindowCommand(viewModel.PlayerId);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this);
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            UnityEngine.Debug.Log($"GameStartedEvent: {viewModel.Index} {gameEvent.PlayerIds.Length}");
            if (gameEvent.PlayerIds.Length > viewModel.Index)
            {
                viewModel.Initialize(gameEvent.PlayerIds[viewModel.Index], 0, null);
                await UniTask.WaitUntil(() => viewModel.State.Value == BoardPlayerPanelState.Enabled);
            }
        }

        public async UniTask HandleAsync(TurnStartedEvent turnEvent)
        {
            if (turnEvent.CurrentPlayerId == viewModel.PlayerId)
            {
                viewModel.SetCurrentPlayer(true);
                await UniTask.WaitUntil(() => viewModel.State.Value == BoardPlayerPanelState.CurrentPlayer);
            }
            else if (viewModel.State.Value == BoardPlayerPanelState.CurrentPlayer && turnEvent.CurrentPlayerId != viewModel.PlayerId)
            {
                viewModel.SetCurrentPlayer(false);
                await UniTask.WaitUntil(() => viewModel.State.Value == BoardPlayerPanelState.Enabled);
            }
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}