namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckPresenter
    {
        private readonly BoardCardDeckView view;
        private readonly BoardCardDeckViewModel viewModel = new BoardCardDeckViewModel();

        public BoardCardDeckPresenter(BoardCardDeckView view)
        {
            this.view = view;

            InitializeMVP();
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