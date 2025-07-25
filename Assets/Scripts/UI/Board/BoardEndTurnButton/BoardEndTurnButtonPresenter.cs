using System;
using Command;
using Cysharp.Threading.Tasks;
using R3;

namespace UI.Board.BoardEndTurnButton
{
    public class BoardEndTurnButtonPresenter : IDisposable
    {
        private readonly BoardEndTurnButtonViewModel viewModel;
        private readonly BoardEndTurnButtonView view;
        private readonly CommandFactory commandFactory;
        
        private IDisposable disposables;

        public BoardEndTurnButtonPresenter(BoardEndTurnButtonView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardEndTurnButtonViewModel();
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OnGameStarted()
        {
            viewModel.SetEnabled();
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

        private async UniTask HandleStateChange(BoardEndTurnButtonState state)
        {
            switch (state)
            {
                case BoardEndTurnButtonState.Disabled:
                    view.gameObject.SetActive(false);
                    break;
                case BoardEndTurnButtonState.Enabled:
                    view.gameObject.SetActive(true);
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnButtonClicked.Subscribe(_ => HandleButtonClicked().Forget()).AddTo(ref d);
        }

        private async UniTask HandleButtonClicked()
        {
            var command = commandFactory.CreateEndPlayerTurnCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {

        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}