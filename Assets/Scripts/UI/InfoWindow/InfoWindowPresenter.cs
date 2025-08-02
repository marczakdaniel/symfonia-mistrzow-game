using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.InfoWindow
{
    public class InfoWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<InfoWindowOpenedEvent>,
        IAsyncEventHandler<InfoWindowClosedEvent>
    {
        private readonly InfoWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public InfoWindowPresenter(InfoWindowView view, CommandFactory commandFactory)
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

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseInfoWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<InfoWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<InfoWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(InfoWindowOpenedEvent infoWindowOpenedEvent)
        {
            view.SetDescription(infoWindowOpenedEvent.Description);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(InfoWindowClosedEvent infoWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}