using Command;
using Cysharp.Threading.Tasks;
using UI.Board.BoardMusicCardPanel.BoardCardDeck;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using Models;
using DefaultNamespace.Data;
using Events;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections.Generic;

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
                                          IGameModelReader gameModelReader)
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
            level1CardPresenters = new BoardMusicCardPresenter[view.Level1Cards.Length];
            for (int i = 0; i < view.Level1Cards.Length; i++)
            {
                level1CardPresenters[i] = new BoardMusicCardPresenter(view.Level1Cards[i], 1, i, commandFactory, gameModelReader);
            }

            level2CardPresenters = new BoardMusicCardPresenter[view.Level2Cards.Length];
            for (int i = 0; i < view.Level2Cards.Length; i++)
            {
                level2CardPresenters[i] = new BoardMusicCardPresenter(view.Level2Cards[i], 2, i, commandFactory, gameModelReader);
            }

            level3CardPresenters = new BoardMusicCardPresenter[view.Level3Cards.Length];
            for (int i = 0; i < view.Level3Cards.Length; i++)
            {
                level3CardPresenters[i] = new BoardMusicCardPresenter(view.Level3Cards[i], 3, i, commandFactory, gameModelReader);
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

        private void SubscribeToEvents()
        {
        }

        public async UniTask InitializeBoard()
        {
            // Get current board cards state

            // Put cards on board with animations - level by level with delays
            await PutCardsOnBoardWithAnimations();

            // Reveal cards - for now we'll skip this step

            await RevealCards();

        }

        private async UniTask PutCardsOnBoardWithAnimations()
        {
            
            // Level 1 cards (index 0 in boardCards)
            for (int i = 0; i < level1CardPresenters.Length; i++)
            {
                await level1CardPresenters[i].PutCardOnBoard();
            }

            // Level 2 cards (index 1 in boardCards)
            for (int i = 0; i < level2CardPresenters.Length; i++)
            {
                await level2CardPresenters[i].PutCardOnBoard();
            }

            // Level 3 cards (index 2 in boardCards)
            for (int i = 0; i < level3CardPresenters.Length; i++)
            {
                await level3CardPresenters[i].PutCardOnBoard();
            }
        }

        public async UniTask RevealCards()
        {
            // Level 1 cards (index 0 in boardCards)

            var task = new List<UniTask>();
            for (int i = 0; i < level1CardPresenters.Length; i++)
            {
                task.Add(level1CardPresenters[i].RevealCard());
            }

            for (int i = 0; i < level2CardPresenters.Length; i++)
            {
                task.Add(level2CardPresenters[i].RevealCard());
            }

            for (int i = 0; i < level3CardPresenters.Length; i++)
            {
                task.Add(level3CardPresenters[i].RevealCard());
            }

            await UniTask.WhenAll(task);
        }
    }
}