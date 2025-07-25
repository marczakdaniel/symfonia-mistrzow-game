using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Models;
using R3;
using UI.Board.BoardPlayersPanel.BoardSinglePlayer;

namespace UI.Board.BoardPlayersPanel
{
    public class BoardPlayersPanelPresenter : 
        IDisposable
    {
        private readonly BoardPlayersPanelViewModel viewModel;
        private readonly BoardPlayersPanelView view;
        private readonly List<BoardSinglePlayerPresenter> boardSinglePlayerPresenters = new List<BoardSinglePlayerPresenter>();
        private readonly IGameModelReader gameModelReader;
        private IDisposable disposables;

        public BoardPlayersPanelPresenter(BoardPlayersPanelView view, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.viewModel = new BoardPlayersPanelViewModel();
            this.gameModelReader = gameModelReader;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        public async UniTask OnGameStarted()
        {
            foreach (var presenter in boardSinglePlayerPresenters)
            {
                presenter.OnGameStarted();
            }
            await UniTask.CompletedTask;
        }

        private void InitializeChildMVP()
        {
            var players = gameModelReader.GetPlayers();

            for (int i = 0; i < view.BoardSinglePlayerViews.Length; i++)
            {
                if (players.Length > i)
                {
                    boardSinglePlayerPresenters.Add(new BoardSinglePlayerPresenter(view.BoardSinglePlayerViews[i], players[i].PlayerId));
                }
                else 
                {
                    view.BoardSinglePlayerViews[i].gameObject.SetActive(false);
                }
            }
        }

        private void InitializeMVP()
        {
            var d = new DisposableBuilder();

            ConnectModel(d);
            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectModel(DisposableBuilder d)
        {

        }

        private void ConnectView(DisposableBuilder d)
        {

        }

        private void SubscribeToEvents()
        {

        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}