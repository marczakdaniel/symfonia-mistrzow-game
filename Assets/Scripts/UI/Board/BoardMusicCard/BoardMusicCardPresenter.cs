using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using R3;
using System;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class BoardMusicCardPresenter : 
        IDisposable, 
        IAsyncEventHandler<CardReservedEvent>,
        IAsyncEventHandler<PutCardOnBoardEvent>,
        IAsyncEventHandler<GameStartedEvent>,
        IAsyncEventHandler<CardPurchasedFromBoardEvent>,
        IAsyncEventHandler<MusicCardDetailsPanelOpenedEvent>,
        IAsyncEventHandler<MusicCardDetailsPanelClosedEvent>,
        IAsyncEventHandler<TurnStartedEvent>,
        IAsyncEventHandler<CardPurchasedFromReserveEvent>,
        IAsyncEventHandler<SelectedTokensConfirmedEvent>,
        IAsyncEventHandler<DeckCardReservedEvent>
    {
        private readonly BoardMusicCardView view;
        private readonly BoardMusicCardViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public BoardMusicCardPresenter(BoardMusicCardView view, int level, int position, CommandFactory commandFactory)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            this.viewModel = new BoardMusicCardViewModel(level, position);
            
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
        }

        private void ConnectView()
        {
            view.OnCardClicked.Subscribe(_ => HandleCardClick().ToObservable()).AddTo(subscriptions);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<PutCardOnBoardEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelClosedEvent>(this, EventPriority.Low);
            AsyncEventBus.Instance.Subscribe<TurnStartedEvent>(this);
            AsyncEventBus.Instance.Subscribe<CardPurchasedFromReserveEvent>(this);
            AsyncEventBus.Instance.Subscribe<SelectedTokensConfirmedEvent>(this);
            AsyncEventBus.Instance.Subscribe<DeckCardReservedEvent>(this);
        }

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            view.SetCanBePurchased(false);
            if (cardReservedEvent.CardId != viewModel.MusicCardId)
            {
                return;
            }
            viewModel.HideCard();
            await view.PlayHideAnimation();

            if (viewModel.CardCanBeDisabled)
            {
                viewModel.SetCardDisabled();
            }
        }

        public async UniTask HandleAsync(CardPurchasedFromBoardEvent cardPurchasedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            view.SetCanBePurchased(false);
            if (cardPurchasedEvent.CardId != viewModel.MusicCardId)
            {
                return;
            }
            viewModel.HideCard();
            await view.PlayHideAnimation();

            if (viewModel.CardCanBeDisabled)
            {
                viewModel.SetCardDisabled();
            }
        }

        public async UniTask HandleAsync(PutCardOnBoardEvent putCardOnBoardEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            if (putCardOnBoardEvent.Level == viewModel.Level && putCardOnBoardEvent.IsDeckCardEmpty)
            {
                viewModel.SetCardCanBeDisabled();
            }

            if (putCardOnBoardEvent.Level != viewModel.Level || putCardOnBoardEvent.Position != viewModel.Position)
            {
                return;
            }
            
            viewModel.RevealCard(putCardOnBoardEvent.MusicCardData.Id);
            view.Setup(putCardOnBoardEvent.MusicCardData);
            await view.PlayRevealAnimation();
        }

        public async UniTask HandleAsync(GameStartedEvent gameStartedEvent)
        {
            var boardCards = gameStartedEvent.BoardCards;
            var musicCardData = boardCards[viewModel.Level][viewModel.Position];
            viewModel.RevealCard(musicCardData.Id);
            view.Setup(musicCardData);
            await view.PlayRevealAnimation();
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelOpenedEvent musicCardDetailsPanelOpenedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            if (musicCardDetailsPanelOpenedEvent.Level != viewModel.Level || musicCardDetailsPanelOpenedEvent.Position != viewModel.Position)
            {
                return;
            }
            await view.PlayHideAnimation(50);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelClosedEvent musicCardDetailsPanelClosedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            await view.PlaySimpleShowAnimation();
        }

        public async UniTask HandleAsync(TurnStartedEvent turnStartedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            view.SetCanBePurchased(turnStartedEvent.MusicCardIdsThatCanBePurchased.Contains(viewModel.MusicCardId));
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(CardPurchasedFromReserveEvent cardPurchasedFromReserveEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            view.SetCanBePurchased(false);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(SelectedTokensConfirmedEvent selectedTokensConfirmedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            view.SetCanBePurchased(false);
            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(DeckCardReservedEvent deckCardReservedEvent)
        {
            if (viewModel.CardDisabled)
            {
                return;
            }

            if (deckCardReservedEvent.MusicCardData.Level == viewModel.Level && deckCardReservedEvent.IsDeckCardEmpty)
            {
                viewModel.SetCardCanBeDisabled();
            }

            await UniTask.CompletedTask;
        }

        // Input -> Command
        private async UniTask HandleCardClick()
        {
            var command = commandFactory.CreateOpenMusicCardDetailsPanelCommand(viewModel.MusicCardId, viewModel.Level, viewModel.Position);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}