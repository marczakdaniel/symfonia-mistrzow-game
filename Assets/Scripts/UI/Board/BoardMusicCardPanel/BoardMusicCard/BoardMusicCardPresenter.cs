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
        IAsyncEventHandler<MusicCardDetailsPanelOpenedEvent>, 
        IAsyncEventHandler<MusicCardDetailsPanelClosedEvent>
        //IAsyncEventHandler<CardReservedEvent>, 
        //IAsyncEventHandler<CardPurchasedEvent>
    {
        private readonly BoardMusicCardView view;
        private readonly BoardMusicCardViewModel viewModel;
        private readonly CommandFactory commandFactory;
        private readonly IGameModelReader gameModelReader;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public BoardMusicCardPresenter(BoardMusicCardView view, int level, int position, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            this.gameModelReader = gameModelReader ?? throw new ArgumentNullException(nameof(gameModelReader));
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
            viewModel.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(subscriptions);
            viewModel.MusicCardData.Where(data => data != null).Subscribe(data => view.Setup(data)).AddTo(subscriptions);
        }

        private void ConnectView()
        {
            view.OnCardClicked.Subscribe(_ => HandleCardClick().Forget()).AddTo(subscriptions);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelClosedEvent>(this);
            //AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
            //AsyncEventBus.Instance.Subscribe<CardPurchasedEvent>(this);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelOpenedEvent musicCardDetailsPanelOpenedEvent)
        {
            if (musicCardDetailsPanelOpenedEvent.MusicCardId != viewModel.MusicCardData.Value.Id) {
                return;
            }

            if (!viewModel.DisableCardWhenOpenMusicCardDetailsPanel()) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.DuringOpenMusicCardDetailsPanel);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelClosedEvent musicCardDetailsPanelClosedEvent)
        {
            if (musicCardDetailsPanelClosedEvent.MusicCardId != viewModel.MusicCardData.Value.Id) {
                return;
            }

            if (!viewModel.AfterCloseMusicCardDetailsPanel()) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.Visible);
        }

        /*

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent)
        {
            if (!viewModel.AfterReserveMusicCard()) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.Disabled);
        }

        public async UniTask HandleAsync(CardPurchasedEvent cardPurchasedEvent)
        {
            if (!viewModel.AfterBuyMusicCard()) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.Disabled);
        }

        */

        // Model -> View
        private async UniTask HandleStateChange(BoardMusicCardState state)
        {
            Debug.Log($"HandleStateChange: {state}");

            if (state == BoardMusicCardState.DuringPutOnBoardAnimation) {
                await view.PlayPutOnBoardAnimation();
                viewModel.CompletePutOnBoardAnimation();
            }
            else if (state == BoardMusicCardState.DuringRevealAnimation) {
                await view.PlayRevealAnimation();
                viewModel.CompleteRevealAnimation();
            }
            else if (state == BoardMusicCardState.DuringMovingToPlayerResources) {
                await view.PlayMovingToPlayerResourcesAnimation();
                viewModel.CompleteMovingToPlayerResources();
            }
            else if (state == BoardMusicCardState.Hidden) {
            }
            else if (state == BoardMusicCardState.Visible) {
                view.EnableCard();
            }
            else if (state == BoardMusicCardState.Disabled) {
                view.DisableCard();
            }
            else if (state == BoardMusicCardState.DuringOpenMusicCardDetailsPanel) {
                view.DisableCard();
            }
            else {
                Debug.LogError($"Unknown state: {state}");
            }
        }

        
        // Input -> Command
        private async UniTask HandleCardClick()
        {
            var command = commandFactory.CreateOpenMusicCardDetailsPanelCommand(viewModel.MusicCardData.Value.Id, viewModel.Level, viewModel.Position);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        // Event Bus
        
        // Public methods

        public async UniTask PutCardOnBoard()
        {
            var musicCardSlot = gameModelReader.GetBoardSlot(viewModel.Level, viewModel.Position);

            if (!viewModel.PutCardOnBoard(musicCardSlot.CardId, musicCardSlot.GetMusicCardData())) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.Hidden);
        }

        public async UniTask RevealCard()
        {
            if (!viewModel.RevealCard()) {
                return;
            }

            await UniTask.WaitUntil(() => viewModel.State.Value == BoardMusicCardState.Visible);
        }



        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}