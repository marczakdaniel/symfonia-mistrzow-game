using System;
using System.Collections.Generic;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;
using UI.Board.BoardTokenPanel.BoardToken;

namespace UI.Board.BoardTokenPanel
{
    public class BoardTokenPanelPresenter : 
        IDisposable
    {
        private readonly BoardTokenPanelView view;
        private readonly BoardTokenPanelViewModel viewModel;

        private readonly CommandFactory commandFactory;
        private IDisposable disposables;
        private BoardTokenPresenter[] tokenPresenters;
        public BoardTokenPanelPresenter(BoardTokenPanelView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardTokenPanelViewModel();
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
        }

        private void InitializeChildMVP()
        {
            tokenPresenters = new BoardTokenPresenter[Enum.GetValues(typeof(ResourceType)).Length];

            foreach (var resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                tokenPresenters[(int)resourceType] = new BoardTokenPresenter(view.Tokens[(int)resourceType], (ResourceType)resourceType, commandFactory);
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
        }

        private void ConnectView(DisposableBuilder d)
        {
        }


        public void Dispose()
        {
            disposables?.Dispose();
        }

        public async UniTask PutTokensOnBoard()
        {
            var delay = 600;

            var tasks = new List<UniTask>();
            foreach (var tokenPresenter in tokenPresenters)
            {
                tasks.Add(tokenPresenter.PutTokenOnBoard());
                await UniTask.Delay(delay);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}