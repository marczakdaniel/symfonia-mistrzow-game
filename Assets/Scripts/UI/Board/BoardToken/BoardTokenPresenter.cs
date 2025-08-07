using System;
using Assets.Scripts.UI.Elements;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;

namespace UI.Board.BoardTokenPanel.BoardToken
{
    public class BoardTokenPresenter : IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<SelectedTokensConfirmedEvent>,
        IAsyncEventHandler<TokenDetailsPanelClosedEvent>,
        IAsyncEventHandler<ReturnTokenWindowOpenedEvent>,
        IAsyncEventHandler<ReturnTokensConfirmedEvent>,
        IAsyncEventHandler<CardReservedEvent>,
        IAsyncEventHandler<GameStartedEvent>,   
        IAsyncEventHandler<CardPurchasedFromBoardEvent>,
        IAsyncEventHandler<CardPurchasedFromReserveEvent>
    {
        private readonly UniversalTokenElement view;
        private readonly BoardTokenViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;
        

        public BoardTokenPresenter(UniversalTokenElement view, ResourceType resourceType, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardTokenViewModel(resourceType);
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
            view.OnTokenClicked.Subscribe(_ => HandleTokenClick().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleTokenClick()
        {
            if (viewModel.ResourceType == ResourceType.Inspiration)
            {
                return;
            }
            
            await OpenTokenDetailsPanel();
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this, EventPriority.Critical);
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelClosedEvent>(this, EventPriority.Low);
            AsyncEventBus.Instance.Subscribe<SelectedTokensConfirmedEvent>(this);
                        
            AsyncEventBus.Instance.Subscribe<ReturnTokensConfirmedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ReturnTokenWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);

            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
        }

        public async UniTask HandleAsync(SelectedTokensConfirmedEvent selectedTokensConfirmedEvent)
        {
            if (viewModel.ResourceType == ResourceType.Inspiration)
            {
                return;
            }
            await view.PlayShowAnimation();
            await view.UpdateValue(selectedTokensConfirmedEvent.BoardTokens[viewModel.ResourceType]);
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent tokenDetailsPanelClosedEvent)
        {
            if (viewModel.ResourceType == ResourceType.Inspiration)
            {
                return;
            }
            await view.PlayShowAnimation();
        }

        public async UniTask HandleAsync(ReturnTokensConfirmedEvent returnTokensConfirmedEvent)
        {
            await view.UpdateValue(returnTokensConfirmedEvent.BoardTokens[viewModel.ResourceType]);
        }

        public async UniTask HandleAsync(ReturnTokenWindowOpenedEvent returnTokenWindowOpenedEvent)
        {
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(GameStartedEvent gameStartedEvent)
        {
            view.Initialize(viewModel.ResourceType, gameStartedEvent.BoardTokens[viewModel.ResourceType]);
            await view.PlayShowAnimation();
        }

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent)
        {
            if (viewModel.ResourceType != ResourceType.Inspiration)
            {
                return;
            }

            await view.UpdateValue(cardReservedEvent.InspirationTokensOnBoard);
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent cardPurchasedFromBoardEvent)
        {
            await view.UpdateValue(cardPurchasedFromBoardEvent.BoardTokens[viewModel.ResourceType]);
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent cardPurchasedFromReserveEvent)
        {
            await view.UpdateValue(cardPurchasedFromReserveEvent.BoardTokens[viewModel.ResourceType]);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent tokenDetailsPanelOpenedEvent)
        {
            if (viewModel.ResourceType == ResourceType.Inspiration)
            {
                return;
            }
            HideAnimationWithDelay().Forget();
            await UniTask.CompletedTask;
        }

        private async UniTask HideAnimationWithDelay()
        {
            await UniTask.Delay(50);
            await view.PlayHideAnimation();
        }

        // Element actions
        // Parent/EventBus -> Presenter -> ViewModel

        public async UniTask OpenTokenDetailsPanel()
        {
            var command = commandFactory.CreateOpenTokenDetailsPanelCommand(viewModel.ResourceType);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}