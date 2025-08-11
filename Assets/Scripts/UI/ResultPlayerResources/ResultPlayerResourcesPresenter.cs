using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.ResultPlayerResources
{
    public class ResultPlayerResourcesPresenter
        : IDisposable
        , IAsyncEventHandler<ResultPlayerResourcesWindowOpenedEvent>
    {
        private readonly ResultPlayerResourcesView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposable;

        public ResultPlayerResourcesPresenter(ResultPlayerResourcesView view, CommandFactory commandFactory)
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
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseResultPlayerResourcesWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<ResultPlayerResourcesWindowOpenedEvent>(this);
        }

        public async UniTask HandleAsync(ResultPlayerResourcesWindowOpenedEvent openedEvent)
        {
            view.Setup(openedEvent.PlayerMusicCardDatas);
            await view.PlayOpenAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}