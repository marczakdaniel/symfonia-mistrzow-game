using Command;
using UI.Board.BoardMusicCardPanel.BoardCardDeck;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;

namespace UI.Board.BoardMusicCardPanel
{
    public class BoardMusicCardPanelPresenter
    {
        private readonly BoardMusicCardPanelView view;
        private readonly BoardMusicCardPanelViewModel viewModel = new BoardMusicCardPanelViewModel();
        private BoardMusicCardPresenter[] level1CardPresenters;
        private BoardMusicCardPresenter[] level2CardPresenters;
        private BoardMusicCardPresenter[] level3CardPresenters;
        private BoardCardDeckPresenter level1CardDeckPresenter;
        private BoardCardDeckPresenter level2CardDeckPresenter;
        private BoardCardDeckPresenter level3CardDeckPresenter;
        private CommandFactory commandFactory;

        public BoardMusicCardPanelPresenter(BoardMusicCardPanelView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMCP();
            InitializeMVP();
        }

        private void InitializeChildMCP()
        {
            level1CardPresenters = new BoardMusicCardPresenter[view.Level1Cards.Length];
            for (int i = 0; i < view.Level1Cards.Length; i++)
            {
                level1CardPresenters[i] = new BoardMusicCardPresenter(view.Level1Cards[i], commandFactory);
            }

            level2CardPresenters = new BoardMusicCardPresenter[view.Level2Cards.Length];
            for (int i = 0; i < view.Level2Cards.Length; i++)
            {
                level2CardPresenters[i] = new BoardMusicCardPresenter(view.Level2Cards[i], commandFactory);
            }

            level3CardPresenters = new BoardMusicCardPresenter[view.Level3Cards.Length];
            for (int i = 0; i < view.Level3Cards.Length; i++)
            {
                level3CardPresenters[i] = new BoardMusicCardPresenter(view.Level3Cards[i], commandFactory);
            }

            level1CardDeckPresenter = new BoardCardDeckPresenter(view.Level1CardDeck);
            level2CardDeckPresenter = new BoardCardDeckPresenter(view.Level2CardDeck);
            level3CardDeckPresenter = new BoardCardDeckPresenter(view.Level3CardDeck);
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