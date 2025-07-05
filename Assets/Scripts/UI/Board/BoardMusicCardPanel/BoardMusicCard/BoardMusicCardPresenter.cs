using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Manager;
using R3;
using System;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class BoardMusicCardPresenter : IDisposable
    {
        private readonly BoardMusicCardView view;
        private readonly BoardMusicCardViewModel viewModel = new BoardMusicCardViewModel();
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public BoardMusicCardPresenter(BoardMusicCardView view, CommandFactory commandFactory)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));

            InitializeMVP();
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            viewModel.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(subscriptions);
            viewModel.MusicCardData.Subscribe(data => view.Setup(data)).AddTo(subscriptions);
        }

        private void ConnectView()
        {
            view.OnCardClicked.Subscribe(HandleCardClick).AddTo(subscriptions);
        }

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
                
            }
            else if (state == BoardMusicCardState.Disabled) {
                
            }
            else {
                Debug.LogError($"Unknown state: {state}");
            }
        }

        
        // Input -> Command
        private void HandleCardClick(Unit unit)
        {
            
            Debug.Log("Card clicked");
        }

        // Event Bus
        
        // Public methods

        public async UniTask PutCardOnBoard(string musicCardId, IMusicCardDataReader musicCardData)
        {
            if (!viewModel.PutCardOnBoard(musicCardId, musicCardData)) {
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