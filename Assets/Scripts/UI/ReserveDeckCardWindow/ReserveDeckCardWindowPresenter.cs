using System;
using Cysharp.Threading.Tasks;
using Command;
using R3;
using Events;

namespace UI.ReserveDeckCardWindow {
    public class ReserveDeckCardWindowPresenter 
        : IDisposable,
        IAsyncEventHandler<ReserveDeckCardWindowOpenedEvent>,
        IAsyncEventHandler<ReserveDeckCardWindowClosedEvent>,
        IAsyncEventHandler<DeckCardReservedEvent>
    {
        private readonly ReserveDeckCardWindowView view;
        private readonly CommandFactory commandFactory;
        private readonly ReserveDeckCardWindowViewModel viewModel;
        private IDisposable disposable;

        public ReserveDeckCardWindowPresenter(ReserveDeckCardWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.viewModel = new ReserveDeckCardWindowViewModel();

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
            view.OnCloseButtonClick.Subscribe(_ => HandleCloseButtonClick().ToObservable()).AddTo(ref d);
            view.OnReserveButtonClick.Subscribe(_ => HandleReserveButtonClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClick()
        {
            UnityEngine.Debug.Log("HandleCloseButtonClick");
            var command = commandFactory.CreateCloseReserveDeckCardWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }   

        private async UniTask HandleReserveButtonClick()
        {
            var command = commandFactory.CreateReserveDeckCardCommand(viewModel.CardLevel);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<ReserveDeckCardWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReserveDeckCardWindowClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<DeckCardReservedEvent>(this);
        }

        public async UniTask HandleAsync(ReserveDeckCardWindowOpenedEvent openedEvent)
        {
            viewModel.SetCardLevel(openedEvent.CardLevel);
            view.SetCardLevel(openedEvent.CardLevel);
            await view.PlayOpenAnimation(openedEvent.CardLevel);
        }

        public async UniTask HandleAsync(ReserveDeckCardWindowClosedEvent closedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public async UniTask HandleAsync(DeckCardReservedEvent reservedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }

    }
}