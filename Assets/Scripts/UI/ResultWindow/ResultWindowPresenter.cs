using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.ResultWindow
{
    public class ResultWindowPresenter
        : IDisposable,
        IAsyncEventHandler<ResultWindowOpenedEvent>
    {
        private readonly ResultWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public ResultWindowPresenter(ResultWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            Initialize();
            SubscribeEvents();
        }

        private void Initialize()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            
        }

        private void SubscribeEvents()
        {
            AsyncEventBus.Instance.Subscribe<ResultWindowOpenedEvent>(this);
        }

        public async UniTask HandleAsync(ResultWindowOpenedEvent e)
        {
            view.Setup(e.PlayerNames, e.PlayerPoints, e.PlayerAvatars);
            await view.PlayOpenAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}