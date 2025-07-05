using Command;
using Cysharp.Threading.Tasks;
using UI.Board.BoardMusicCardPanel.BoardCardDeck;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using Models;
using DefaultNamespace.Data;
using Events;

namespace UI.Board.BoardMusicCardPanel
{
    public class BoardMusicCardPanelPresenter
    {
        private readonly BoardMusicCardPanelView view;
        private readonly BoardMusicCardPanelViewModel viewModel = new BoardMusicCardPanelViewModel();
        private readonly IGameModelReader gameModelReader;
        private BoardMusicCardPresenter[] level1CardPresenters;
        private BoardMusicCardPresenter[] level2CardPresenters;
        private BoardMusicCardPresenter[] level3CardPresenters;
        private BoardCardDeckPresenter level1CardDeckPresenter;
        private BoardCardDeckPresenter level2CardDeckPresenter;
        private BoardCardDeckPresenter level3CardDeckPresenter;
        private CommandFactory commandFactory;

        public BoardMusicCardPanelPresenter(BoardMusicCardPanelView view, 
                                          CommandFactory commandFactory,
                                          IGameModelReader gameModelReader = null)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            this.gameModelReader = gameModelReader ?? GameModel.Instance;

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

        public void SubscribeToEvents()
        {
            // Hybrid approach: Light events as notifications + model reading
            AsyncEventBus.Instance.Subscribe<StartGameEvent>(async (eventData) =>
            {
                // Read current state from model instead of using event data
                var boardCards = gameModelReader.GetCurrentBoardCards();
                await InitializeBoard(boardCards);
            });

            // Example of other events that would trigger board updates
            // AsyncEventBus.Instance.Subscribe<CardRemovedFromBoardEvent>(async (eventData) =>
            // {
            //     var boardCards = gameModelReader.GetCurrentBoardCards();
            //     await UpdateBoard(boardCards);
            // });
        }

        public void StartGame()
        {
            
        }

        public async UniTask InitializeBoard(IMusicCardDataReader[,] boardCards)
        {
            // Get current board cards state

            // Put cards on board with animations - level by level with delays
            await PutCardsOnBoardWithAnimations(boardCards);

            // Reveal cards - for now we'll skip this step
            // TODO: Implement card reveal animations if needed
        }

        private async UniTask PutCardsOnBoardWithAnimations(IMusicCardDataReader[,] boardCards)
        {
            const int delayBetweenCards = 200; // milliseconds
            
            // Level 1 cards (index 0 in boardCards)
            for (int i = 0; i < level1CardPresenters.Length; i++)
            {
                var cardData = boardCards[0, i]; // Level 1 is at index 0
                if (cardData != null)
                {
                    await level1CardPresenters[i].PutCardOnBoard(cardData.Id, cardData);
                    await UniTask.Delay(delayBetweenCards);
                }
            }

            // Level 2 cards (index 1 in boardCards)
            for (int i = 0; i < level2CardPresenters.Length; i++)
            {
                var cardData = boardCards[1, i]; // Level 2 is at index 1
                if (cardData != null)
                {
                    await level2CardPresenters[i].PutCardOnBoard(cardData.Id, cardData);
                    await UniTask.Delay(delayBetweenCards);
                }
            }

            // Level 3 cards (index 2 in boardCards)
            for (int i = 0; i < level3CardPresenters.Length; i++)
            {
                var cardData = boardCards[2, i]; // Level 3 is at index 2
                if (cardData != null)
                {
                    await level3CardPresenters[i].PutCardOnBoard(cardData.Id, cardData);
                    await UniTask.Delay(delayBetweenCards);
                }
            }
        }
    }
}