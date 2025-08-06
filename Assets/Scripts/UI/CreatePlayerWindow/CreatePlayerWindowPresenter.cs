using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using UnityEngine;

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
        private readonly CreatePlayerWindowViewModel viewModel;
        private IDisposable disposable;

        public CreatePlayerWindowPresenter(CreatePlayerWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.viewModel = new CreatePlayerWindowViewModel();

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
            view.OnAvatarClicked.Subscribe(x => HandleAvatarClicked(x).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAddPlayerButtonClicked(string playerName)
        {
            var command = commandFactory.CreateAddPlayerCommand(playerName, viewModel.PlayerAvatar);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseCreatePlayerWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleAvatarClicked((Sprite, int) x)
        {
            viewModel.SetPlayerAvatar(x.Item1);
            view.ResetSelectedAvatar();
            view.SetSelectedAvatar(x.Item2);
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
            viewModel.ResetPlayerAvatar();
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(CreatePlayerWindowOpenedEvent openEvent)
        {
            view.SetupAvatars(openEvent.AvailablePlayerAvatars);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(CreatePlayerWindowClosedEvent createPlayerWindowClosedEvent)
        {
            viewModel.ResetPlayerAvatar();
            await view.PlayCloseAnimation();
        }
        

        public async UniTask HandleAsync(GameStartedEvent gameStartedEvent)
        {
            viewModel.ResetPlayerAvatar();
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}