using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.ResultWindow
{
    public class ResultWindowPresenter
        : IDisposable,
        IAsyncEventHandler<ResultWindowOpenedEvent>
    {
        private readonly ResultWindowView view;
        private readonly CommandFactory commandFactory;
        private readonly ResultWindowViewModel viewModel;
        private IDisposable disposable;

        public ResultWindowPresenter(ResultWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            viewModel = new ResultWindowViewModel();

            Initialize();
            SubscribeEvents();
        }

        private void Initialize()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnPlayerClicked.Subscribe(playerIndex => HandlePlayerClicked(playerIndex).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandlePlayerClicked(int playerIndex)
        {
            var command = commandFactory.CreateOpenResultPlayerResourcesWindowCommand(viewModel.PlayerIds[playerIndex]);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeEvents()
        {
            AsyncEventBus.Instance.Subscribe<ResultWindowOpenedEvent>(this);
        }

        public async UniTask HandleAsync(ResultWindowOpenedEvent e)
        {
            viewModel.SetPlayerIds(e.PlayerIds);
            view.Setup(e.PlayerNames, e.PlayerPoints, e.PlayerAvatars);
            await view.PlayOpenAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}