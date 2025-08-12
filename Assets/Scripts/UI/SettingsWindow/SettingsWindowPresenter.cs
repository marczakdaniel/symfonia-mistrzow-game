using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.SettingsWindow
{
    public class SettingsWindowPresenter
        : IDisposable, 
        IAsyncEventHandler<SettingsWindowOpenedEvent>,
        IAsyncEventHandler<SettingsWindowClosedEvent>
    {
        private readonly SettingsWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public SettingsWindowPresenter(SettingsWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            Initialize();
            SubscribeToEvents();
        }

        private void Initialize()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
            view.OnRestartButtonClicked.Subscribe(_ => HandleRestartButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseSettingsWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleRestartButtonClicked()
        {
            var command = commandFactory.CreateRestartGameCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<SettingsWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<SettingsWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(SettingsWindowOpenedEvent openedEvent)
        {
            await view.PlayOpenedAnimation();
        }

        public async UniTask HandleAsync(SettingsWindowClosedEvent closedEvent)
        {
            await view.PlayClosedAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}