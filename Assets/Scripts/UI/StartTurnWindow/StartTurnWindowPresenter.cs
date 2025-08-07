using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.StartTurnWindow
{
    public class StartTurnWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<StartTurnWindowOpenedEvent>,
        IAsyncEventHandler<TurnStartedEvent>
    {
        private readonly StartTurnWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public StartTurnWindowPresenter(StartTurnWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;


            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = new DisposableBuilder();
            ConnectView(d);
            disposables = d.Build();
        }
        private void ConnectView(DisposableBuilder d)
        {
            view.OnStartTurnButtonClick.Subscribe(_ => HandleStartTurnButtonClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleStartTurnButtonClick()
        {
            var command = commandFactory.CreateStartPlayerTurnCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<StartTurnWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this, EventPriority.Critical);
        }

        public async UniTask HandleAsync(StartTurnWindowOpenedEvent eventData)
        {
            view.Setup(eventData.CurrentPlayerName, eventData.CurrentRound, eventData.CurrentPlayerAvatar);
            await view.OpenWindow();
        }

        public async UniTask HandleAsync(TurnStartedEvent eventData)
        {
            await view.CloseWindow();
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}