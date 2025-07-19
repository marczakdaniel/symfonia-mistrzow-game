using System;
using Cysharp.Threading.Tasks;
using Events;
using R3;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>
    {
        private readonly SelectTokenWindowView view;
        private readonly SelectTokenWindowViewModel viewModel;
        private IDisposable disposables;

        public SelectTokenWindowPresenter(SelectTokenWindowView view, SelectTokenWindowViewModel viewModel)
        {
            this.view = view;
            this.viewModel = new SelectTokenWindowViewModel();

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
            viewModel.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(ref d);
        }

        private async UniTask HandleStateChange(SelectTokenWindowState state)
        {
            switch (state)
            {
                case SelectTokenWindowState.Disabled:
                    break;
                case SelectTokenWindowState.DuringEntryAnimation:
                    break;
                case SelectTokenWindowState.Active:
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent tokenDetailsPanelOpenedEvent)
        {
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}