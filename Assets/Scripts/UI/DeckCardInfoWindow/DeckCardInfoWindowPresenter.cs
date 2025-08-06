using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.DeckCardInfoWindow
{
    public class DeckCardInfoWindowPresenter 
        : IDisposable,
        IAsyncEventHandler<DeckCardReservedEvent>,
        IAsyncEventHandler<DeckCardInfoWindowClosedEvent>
    {
        private readonly DeckCardInfoWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public DeckCardInfoWindowPresenter(DeckCardInfoWindowView view, CommandFactory commandFactory)
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
            view.OnAcceptButtonClicked.Subscribe(_ => HandleAcceptButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleAcceptButtonClicked()
        {
            var command = commandFactory.CreateCloseDeckCardInfoWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<DeckCardReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<DeckCardInfoWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(DeckCardReservedEvent openedEvent)
        {
            view.Setup(openedEvent.MusicCardData);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(DeckCardInfoWindowClosedEvent closedEvent)
        {
            await view.PlayCloseAnimation(closedEvent.PlayerIndex);
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}