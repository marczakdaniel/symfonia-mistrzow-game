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
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<SelectedTokensConfirmedEvent>,
        IAsyncEventHandler<TokenDetailsPanelClosedEvent>,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>,
        IAsyncEventHandler<ReturnTokensConfirmedEvent>,
        IAsyncEventHandler<CardReservedEvent>
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
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelClosedEvent>(this);
            AsyncEventBus.Instance.Subscribe<SelectedTokensConfirmedEvent>(this);
                        
            AsyncEventBus.Instance.Subscribe<ReturnTokensConfirmedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokenWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
        }

        public async UniTask HandleAsync(SelectedTokensConfirmedEvent selectedTokensConfirmedEvent)
        {
            viewModel.OnConfirmSelectedTokens(selectedTokensConfirmedEvent.BoardTokens[viewModel.ResourceType]);
            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.Active);
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent tokenDetailsPanelClosedEvent)
        {
            viewModel.CloseTokenDetailsPanel();
            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.Active);
        }

        public async UniTask HandleAsync(ReturnTokensConfirmedEvent returnTokensConfirmedEvent)
        {
            viewModel.OnReturnTokensConfirmed(returnTokensConfirmedEvent.BoardTokens[viewModel.ResourceType]);
            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.Active);
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent returnTokenWindowOpenedEvent)
        {
            viewModel.OpenReturnTokensPanel();
            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.DuringReturnTokensPanelOpen);
        }

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent)
        {
            if (viewModel.ResourceType != ResourceType.Inspiration)
            {
                return;
            }

            UnityEngine.Debug.Log($"[BoardTokenPresenter] Card reserved event: {cardReservedEvent.InspirationTokensOnBoard}");

            viewModel.SetTokenCount(cardReservedEvent.InspirationTokensOnBoard);

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardTokenState.Active);
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
                case BoardTokenState.DuringReturnTokensPanelOpen:
                    await UniTask.CompletedTask;
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
            viewModel.OpenTokenDetailsPanel();
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