using Cysharp.Threading.Tasks;
using Events;
using R3;
using System;

namespace UI.Board.BoardPlayersPanel.BoardSinglePlayer
{
    public class BoardSinglePlayerPresenter :
        IDisposable,
        IAsyncEventHandler<TurnStartedEvent>
    {
        private readonly BoardSinglePlayerViewModel viewModel;
        private readonly BoardSinglePlayerView view;
        private IDisposable disposables;

        public BoardSinglePlayerPresenter(BoardSinglePlayerView view, string playerId)
        {
            this.view = view;
            this.viewModel = new BoardSinglePlayerViewModel(playerId);

            InitializeMVP();
            SubscribeToEvents();
        }

        public void OnGameStarted()
        {
            viewModel.Enable();
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
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleStateChange(BoardSinglePlayerState state)
        {
            switch (state)
            {
                case BoardSinglePlayerState.Disabled:
                    view.gameObject.SetActive(false);
                    view.SetIsCurrentPlayer(false);
                    break;
                case BoardSinglePlayerState.Enabled:
                    view.gameObject.SetActive(true);
                    view.SetIsCurrentPlayer(false);
                    break;
                case BoardSinglePlayerState.CurrentPlayer:
                    view.SetIsCurrentPlayer(true);
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this);
        }

        public async UniTask HandleAsync(TurnStartedEvent eventData)
        {
            UnityEngine.Debug.LogError($"TurnStartedEvent: {eventData.CurrentPlayerId} {viewModel.PlayerId}");
            if (viewModel.State.Value == BoardSinglePlayerState.CurrentPlayer && viewModel.PlayerId != eventData.CurrentPlayerId)
            {
                viewModel.SetNotCurrentPlayer();
                await UniTask.WaitUntil(() => viewModel.State.Value == BoardSinglePlayerState.Enabled);
            }
            else if (viewModel.PlayerId == eventData.CurrentPlayerId)
            {
                viewModel.SetCurrentPlayer();
                await UniTask.WaitUntil(() => viewModel.State.Value == BoardSinglePlayerState.CurrentPlayer);
            }
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}