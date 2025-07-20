using System;
using Models;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using UI.SelectTokenWindow.SelectBoardTokenPanel;
using UnityEngine;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>
    {
        private readonly SelectTokenWindowView view;
        private readonly SelectTokenWindowViewModel viewModel;
        private readonly IGameModelReader gameModelReader;
        private IDisposable disposables;

        private SelectBoardTokenPanelPresenter selectBoardTokenPanelPresenter;

        public SelectTokenWindowPresenter(SelectTokenWindowView view, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new SelectTokenWindowViewModel();
            this.gameModelReader = gameModelReader;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private async UniTask CloseWindow()
        {
            // TODO: Close window animation
            await UniTask.CompletedTask;
        }



        /* ---- */

        private void InitializeChildMVP()
        {
            selectBoardTokenPanelPresenter = new SelectBoardTokenPanelPresenter(view.SelectBoardTokenPanelView, gameModelReader);
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
                case SelectTokenWindowState.Closed:
                    view.OnCloseWindow();
                    break;
                case SelectTokenWindowState.DuringOpenAnimation:
                    await OpenWindow();
                    Debug.Log("OpenWindowFinished");
                    viewModel.OnOpenWindowFinished();
                    break;
                case SelectTokenWindowState.Active:
                    view.OnOpenWindow();
                    break;
            }
        }

        private async UniTask OpenWindow()
        {
            // TODO: Open window animation
            await selectBoardTokenPanelPresenter.OpenWindow();
        }


        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().Forget()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            
            await UniTask.CompletedTask;
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent tokenDetailsPanelOpenedEvent)
        {
            Debug.Log("TokenDetailsPanelOpenedEvent");
            viewModel.OpenWindow();
            await UniTask.WaitUntil(() => viewModel.State.Value == SelectTokenWindowState.Active);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}