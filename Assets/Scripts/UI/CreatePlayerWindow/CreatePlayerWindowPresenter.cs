using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.CreatePlayerWindow
{
    public class CreatePlayerWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<PlayerAddedEvent>,
        IAsyncEventHandler<CreatePlayerWindowOpenedEvent>,
        IAsyncEventHandler<CreatePlayerWindowClosedEvent>,
        IAsyncEventHandler<GameStartedEvent>
    {
        private readonly CreatePlayerWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public CreatePlayerWindowPresenter(CreatePlayerWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnAddPlayerButtonClicked.Subscribe(playerName => HandleAddPlayerButtonClicked(playerName).ToObservable()).AddTo(ref d);
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAddPlayerButtonClicked(string playerName)
        {
            var command = commandFactory.CreateAddPlayerCommand(playerName);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseCreatePlayerWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<PlayerAddedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CreatePlayerWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CreatePlayerWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
        }

        public async UniTask HandleAsync(PlayerAddedEvent playerAddedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(CreatePlayerWindowOpenedEvent createPlayerWindowOpenedEvent)
        {
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(CreatePlayerWindowClosedEvent createPlayerWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(GameStartedEvent gameStartedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}