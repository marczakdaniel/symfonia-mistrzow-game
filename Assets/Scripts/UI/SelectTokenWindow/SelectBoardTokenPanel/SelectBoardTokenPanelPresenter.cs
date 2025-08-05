using System;
using R3;
using Events;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using UI.SelectTokenWindow.SelectSingleToken;
using Models;
using Command;

namespace UI.SelectTokenWindow.SelectBoardTokenPanel
{
    public class SelectBoardTokenPanelPresenter : 
        IDisposable,
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>
    {
        private readonly SelectBoardTokenPanelView view;
        private readonly SelectBoardTokenPanelViewModel viewModel;

        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        private SelectSingleTokenPresenter[] selectSingleTokenPresenters = new SelectSingleTokenPresenter[Enum.GetValues(typeof(ResourceType)).Length - 1];

        public SelectBoardTokenPanelPresenter(SelectBoardTokenPanelView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new SelectBoardTokenPanelViewModel();
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            foreach (var resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                if ((ResourceType)resourceType == ResourceType.Inspiration) {
                    continue;
                }

                var selectSingleTokenView = view.SelectSingleTokenViews[(int)resourceType];
                var selectSingleTokenPresenter = new SelectSingleTokenPresenter(selectSingleTokenView, (ResourceType)resourceType, commandFactory);
                selectSingleTokenPresenters[(int)resourceType] = selectSingleTokenPresenter;
            }
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

        private async UniTask HandleStateChange(SelectBoardTokenPanelState state)
        {
            switch (state)
            {
                case SelectBoardTokenPanelState.Disabled:
                    break;
                case SelectBoardTokenPanelState.DuringOpenAnimation:
                    viewModel.OnOpenWindowFinished();
                    break;
                case SelectBoardTokenPanelState.Active:
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

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent gameEvent)
        {
            
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}