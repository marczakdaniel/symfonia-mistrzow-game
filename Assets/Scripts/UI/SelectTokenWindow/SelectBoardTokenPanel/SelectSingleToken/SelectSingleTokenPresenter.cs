using System;
using R3;
using Events;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Models;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public class SelectSingleTokenPresenter :
        IDisposable
    {
        private readonly SelectSingleTokenView view;
        private readonly SelectSingleTokenViewModel viewModel;
        private readonly IGameModelReader gameModelReader;
        private IDisposable disposables;

        public SelectSingleTokenPresenter(SelectSingleTokenView view, ResourceType resourceType, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new SelectSingleTokenViewModel(resourceType);
            this.gameModelReader = gameModelReader;
            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OnOpenWindow()
        {
            // TODO: Open window animation
            var tokenCount = gameModelReader.GetBoardTokenCount(viewModel.ResourceType);
            viewModel.OnOpenWindow(tokenCount);
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectSingleTokenState.Active);
        }

        public async UniTask OnCloseWindow()
        {
            // TODO: Close window animation
            await UniTask.CompletedTask;
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
            viewModel.State.Subscribe(state => HandleStateChange(state).ToObservable()).AddTo(ref d);
            viewModel.Count.Subscribe(count => view.Setup(viewModel.ResourceType, count)).AddTo(ref d);
        }

        private async UniTask HandleStateChange(SelectSingleTokenState state)
        {
            switch (state)
            {
                case SelectSingleTokenState.Disabled:
                    await view.OnDisabled();
                    break;
                case SelectSingleTokenState.Active:
                    await view.OnActivated();
                    break;
            }
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClicked()
        {
            await UniTask.CompletedTask;
        }

        public void SubscribeToEvents()
        {
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}