using System;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;
using Unity.Android.Gradle.Manifest;

namespace UI.Board.BoardTokenPanel.BoardToken
{
    public class BoardTokenPresenter : IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>
    {
        private readonly BoardTokenView view;
        private readonly BoardTokenViewModel viewModel;
        private readonly IGameModelReader gameModelReader;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;
        

        public BoardTokenPresenter(BoardTokenView view, ResourceType resourceType, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new BoardTokenViewModel(resourceType);
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader;

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
            viewModel.TokenCount.Subscribe(count => view.Setup(viewModel.ResourceType, count)).AddTo(ref d);
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnTokenClicked.Subscribe(_ => HandleTokenClick().Forget()).AddTo(ref d);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
        }

        // ViewModel -> Presenter -> View

        private async UniTask HandleStateChange(BoardTokenState state)
        {
            switch (state) {
                case BoardTokenState.DuringEntryAnimation:
                    // TODO: Play entry animation

                    await view.PlayEntryAnimation();

                    viewModel.CompleteEntryAnimation();

                    break;
                case BoardTokenState.DuringTokenDetailsPanelOpen:
                    await UniTask.CompletedTask;
                    break;
                case BoardTokenState.DuringAddingTokens:
                    await UniTask.CompletedTask;
                    break;
                case BoardTokenState.DuringRemovingTokens:
                    await UniTask.CompletedTask;
                    break;
                case BoardTokenState.Active:
                    await UniTask.CompletedTask;
                    break;
                case BoardTokenState.Disabled:
                    view.DisableElement();
                    break;
                default:
                    break;
            }
        }   

        // View -> Presenter -> Command

        private async UniTask HandleTokenClick()
        {
            if (viewModel.ResourceType == ResourceType.Inspiration)
            {
                return;
            }
            
            await OpenTokenDetailsPanel();
        }

        // Event Bus -> Presenter
        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent tokenDetailsPanelOpenedEvent)
        {
            if (tokenDetailsPanelOpenedEvent.ResourceType != viewModel.ResourceType) {
                return;
            }
        }

        // Element actions
        // Parent/EventBus -> Presenter -> ViewModel

        public async UniTask PutTokenOnBoard()
        {
            var numberOfTokens = gameModelReader.GetBoardTokenCount(viewModel.ResourceType);

            if (!viewModel.OnEntry(numberOfTokens)) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.Active);
        }

        public async UniTask OpenTokenDetailsPanel()
        {
            var command = commandFactory.CreateOpenTokenDetailsPanelCommand(viewModel.ResourceType);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        public async UniTask CloseTokenDetailsPanel()
        {

        }

        public async UniTask AddTokens(int amount)
        {

        }  

        public async UniTask RemoveTokens(int amount)
        {

        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}