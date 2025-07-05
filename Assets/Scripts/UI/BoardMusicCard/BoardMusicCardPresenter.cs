using Command;
using Cysharp.Threading.Tasks;
using Manager;
using R3;
using System;
using UnityEngine;

namespace UI.BoardMusicCard
{
    public class BoardMusicCardPresenter : IDisposable
    {
        private readonly BoardMusicCardView view;
        private readonly BoardMusicCardViewModel model;
        private readonly ICommandManager commandManager;
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public BoardMusicCardPresenter(BoardMusicCardView view, BoardMusicCardViewModel model, ICommandManager commandManager, CommandFactory commandFactory)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
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
            model.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(subscriptions);
            model.MusicCardData.Subscribe(data => view.Setup(data)).AddTo(subscriptions);
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
                model.CompletePutOnBoardAnimation();
            }
            else if (state == BoardMusicCardState.DuringRevealAnimation) {
                await view.PlayRevealAnimation();
                model.CompleteRevealAnimation();
            }
            else if (state == BoardMusicCardState.DuringMovingToPlayerResources) {
                await view.PlayMovingToPlayerResourcesAnimation();
                model.CompleteMovingToPlayerResources();
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
        

        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}