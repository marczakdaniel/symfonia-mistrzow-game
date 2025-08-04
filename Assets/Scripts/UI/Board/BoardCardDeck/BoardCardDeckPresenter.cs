using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Events;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;

namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckPresenter : 
        IAsyncEventHandler<PutCardOnBoardEvent>
    {
        private readonly BoardCardDeckView view;
        private readonly BoardCardDeckViewModel viewModel;

        public BoardCardDeckPresenter(BoardCardDeckView view, int level)
        {
            this.view = view;
            this.viewModel = new BoardCardDeckViewModel(level);

            view.SetLevel(level);

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

        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<PutCardOnBoardEvent>(this, EventPriority.High);
        }

        public async UniTask HandleAsync(PutCardOnBoardEvent eventData)
        {
            if (viewModel.Level != eventData.Level)
            {
                return;
            }

            await view.PlayPutCardOnBoardAnimationWithHide(eventData.Position, 50);
        }
    }
}