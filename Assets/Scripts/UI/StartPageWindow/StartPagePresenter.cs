using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;
namespace UI.StartPageWindow
{
    public class StartPageWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<StartPageWindowOpenedEvent>,
        IAsyncEventHandler<StartPageWindowClosedEvent>,
        IAsyncEventHandler<GameStartedEvent>,
        IAsyncEventHandler<GameCreationWindowOpenedEvent>,
        IAsyncEventHandler<GameCreationWindowClosedEvent>
    {
        private readonly StartPageWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public StartPageWindowPresenter(StartPageWindowView view, CommandFactory commandFactory)
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
            view.OnPlayButtonClicked.Subscribe(_ => HandlePlayButtonClicked().ToObservable()).AddTo(ref d);
            view.OnManualButtonClicked.Subscribe(_ => HandleManualButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandlePlayButtonClicked()
        {
            var command = commandFactory.CreateOpenGameCreationWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleManualButtonClicked()
        {
            var command = commandFactory.CreateOpenInstructionWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<StartPageWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<StartPageWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameCreationWindowOpenedEvent>(this, EventPriority.Low);
            AsyncEventBus.Instance.Subscribe<GameCreationWindowClosedEvent>(this, EventPriority.High);
        }

        public async UniTask HandleAsync(StartPageWindowOpenedEvent startPageWindowOpenedEvent)
        {
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(StartPageWindowClosedEvent startPageWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(GameStartedEvent gameStartedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(GameCreationWindowOpenedEvent gameCreationWindowOpenedEvent)
        {
            await view.PlayCloseAnimation();
        }


        public async UniTask HandleAsync(GameCreationWindowClosedEvent gameCreationWindowClosedEvent)
        {
            await view.PlayOpenAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}