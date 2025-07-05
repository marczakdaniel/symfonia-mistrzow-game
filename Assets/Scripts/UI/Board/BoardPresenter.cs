using Command;
using UI.Board.BoardMusicCardPanel;

namespace UI.Board
{
    public class BoardPresenter
    {
        private readonly BoardView view;
        private readonly BoardViewModel viewModel = new BoardViewModel();
        private BoardMusicCardPanelPresenter boardMusicCardPanelPresenter;
        private CommandFactory commandFactory;

        public BoardPresenter(BoardView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            InitializeChildMCP();
            InitializeMVP();
        }

        private void InitializeChildMCP()
        {
            boardMusicCardPanelPresenter = new BoardMusicCardPanelPresenter(view.BoardMusicCardPanelView, commandFactory);
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
    }
}