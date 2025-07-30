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
        private readonly StartTurnWindowViewModel viewModel;
        private readonly StartTurnWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        public StartTurnWindowPresenter(StartTurnWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new StartTurnWindowViewModel();
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
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
            viewModel.CurrentPlayerName.Subscribe(name => HandleCurrentPlayerNameChange(name).ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleStateChange(StartTurnWindowState state)
        {
            switch (state)
            {
                case StartTurnWindowState.DuringOpenAnimation:
                    await view.OpenWindow();
                    viewModel.OpenAnimationCompleted();
                    break;
                case StartTurnWindowState.DuringCloseAnimation:
                    await view.CloseWindow();
                    viewModel.CloseAnimationCompleted();
                    break;
            }
        }   

        private async UniTask HandleCurrentPlayerNameChange(string name)
        {
            view.SetCurrentPlayerName(name);
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
            viewModel.Open(eventData.CurrentPlayerName);
            await UniTask.WaitUntil(() => viewModel.State.Value == StartTurnWindowState.Opened);
        }

        public async UniTask HandleAsync(TurnStartedEvent eventData)
        {
            if (viewModel.State.Value == StartTurnWindowState.Opened)
            {
                viewModel.Close();
            }
            await UniTask.WaitUntil(() => viewModel.State.Value == StartTurnWindowState.Closed);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}