using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.Board.BoardEndTurnButton
{
    public class BoardEndTurnButtonPresenter : 
        IDisposable, 
        IAsyncEventHandler<TurnStartedEvent>,
        IAsyncEventHandler<GameStartedEvent>,
        IAsyncEventHandler<ShowNextTurnButtonEvent>
    {
        private readonly BoardEndTurnButtonView view;
        private readonly CommandFactory commandFactory;
        
        private IDisposable disposables;

        public BoardEndTurnButtonPresenter(BoardEndTurnButtonView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectModel(d);
            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnButtonClicked.Subscribe(_ => HandleButtonClicked().Forget()).AddTo(ref d);
        }

        private async UniTask HandleButtonClicked()
        {
            var command = commandFactory.CreateEndPlayerTurnCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this, EventPriority.Low);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ShowNextTurnButtonEvent>(this);
        }

        public async UniTask HandleAsync(TurnStartedEvent gameEvent)
        {
            await view.PlayDisabledAnimation();
        }

        public async UniTask HandleAsync(GameStartedEvent gameEvent)
        {
            await view.PlayActiveAnimation();
        }

        public async UniTask HandleAsync(ShowNextTurnButtonEvent gameEvent)
        {
            await view.PlayActiveAnimation();
        }


        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}