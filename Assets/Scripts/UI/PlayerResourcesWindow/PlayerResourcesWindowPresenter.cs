using System;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using Command;

namespace UI.PlayerResourcesWindow
{
    public class PlayerResourcesWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<PlayerResourcesWindowOpenedEvent>,
        IAsyncEventHandler<PlayerResourcesWindowClosedEvent>
    {
        private readonly PlayerResourcesWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;
        
        public PlayerResourcesWindowPresenter(PlayerResourcesWindowView view, CommandFactory commandFactory)
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
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<PlayerResourcesWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<PlayerResourcesWindowClosedEvent>(this);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateClosePlayerResourcesWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        public async UniTask HandleAsync(PlayerResourcesWindowOpenedEvent playerResourcesWindowOpenedEvent)
        {
            view.Initialize(playerResourcesWindowOpenedEvent.PlayerName, playerResourcesWindowOpenedEvent.NumberOfPoints, playerResourcesWindowOpenedEvent.CurrentPlayerTokens, playerResourcesWindowOpenedEvent.CurrentPlayerCards, playerResourcesWindowOpenedEvent.ReservedMusicCards);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(PlayerResourcesWindowClosedEvent playerResourcesWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}