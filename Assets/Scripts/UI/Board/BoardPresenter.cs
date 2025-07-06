using Command;
using UI.Board.BoardMusicCardPanel;
using Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Events;

namespace UI.Board
{
    public class BoardPresenter
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private readonly IGameModelReader gameModelReader;
        private BoardMusicCardPanelPresenter boardMusicCardPanelPresenter;
        private CommandFactory commandFactory;

        public BoardPresenter(BoardView view, CommandFactory commandFactory, IGameModelReader gameModelReader)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader;
            InitializeChildMCP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMCP()
        {
            boardMusicCardPanelPresenter = new BoardMusicCardPanelPresenter(view.BoardMusicCardPanelView, commandFactory, gameModelReader);
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
            
        }

        // Event Handlers
        

    
    }
}