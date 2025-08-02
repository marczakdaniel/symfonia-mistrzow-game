using System;
using System.Collections.Generic;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.CreateGameWindow
{
    public class CreateGameWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<GameCreationWindowOpenedEvent>, 
        IAsyncEventHandler<GameCreationWindowClosedEvent>,
        IAsyncEventHandler<PlayerAddedEvent>,
        IAsyncEventHandler<GameStartedEvent>
    {
        private readonly CreateGameWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public CreateGameWindowPresenter(CreateGameWindowView view, CommandFactory commandFactory)
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
            view.OnAddPlayerButtonClicked.Subscribe(_ => HandleAddPlayerButtonClicked().ToObservable()).AddTo(ref d);
            view.OnStartGameButtonClicked.Subscribe(_ => HandleStartGameButtonClicked().ToObservable()).AddTo(ref d);
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAddPlayerButtonClicked()
        {
            var command = commandFactory.CreateOpenCreatePlayerWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleStartGameButtonClicked()
        {
            var command = commandFactory.CreateStartGameCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseGameCreationWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<GameCreationWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameCreationWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<PlayerAddedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
        }

        public async UniTask HandleAsync(GameCreationWindowOpenedEvent gameCreationWindowOpenedEvent)
        {
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(GameCreationWindowClosedEvent gameCreationWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(PlayerAddedEvent playerAddedEvent)
        {
            view.SetPlayers(playerAddedEvent.PlayerNames);
            await UniTask.CompletedTask;
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