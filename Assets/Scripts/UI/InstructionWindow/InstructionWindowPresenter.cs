using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.InstructionWindow
{
    public class InstructionWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<InstructionWindowOpenedEvent>,
        IAsyncEventHandler<InstructionWindowClosedEvent>
    {
        private readonly InstructionWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public InstructionWindowPresenter(InstructionWindowView view, CommandFactory commandFactory)
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
            var command = commandFactory.CreateCloseInstructionWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<InstructionWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<InstructionWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(InstructionWindowOpenedEvent instructionWindowOpenedEvent)
        {
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(InstructionWindowClosedEvent instructionWindowClosedEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}