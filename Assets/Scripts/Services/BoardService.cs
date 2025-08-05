using System.Collections.Generic;
using DefaultNamespace.Data;
using Models;

namespace Services
{
    public class BoardService
    {
        private readonly GameModel gameModel;
        private BoardModel boardModel => gameModel.Board;

        public BoardService(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void StartGame()
        {
            boardModel.StartGame();
        }

        public Dictionary<ResourceType, int> GetAllBoardResources()
        {
            return boardModel.TokenResources.GetAllResources();
        }

        public int GetBoardTokenCount(ResourceType resourceType)
        {
            return boardModel.TokenResources.GetCount(resourceType);
        }

        public List<BoardSlot> GetEmptySlots()
        {
            return boardModel.GetEmptySlots();
        }

        public BoardSlot GetSlotWithCard(string cardId)
        {
            return boardModel.GetSlotWithCard(cardId);
        }

        public void RefillSlot(int level, int position)
        {
            boardModel.RefillSlot(level, position);
        }

        public Dictionary<int, MusicCardData[]> GetBoardCards()
        {
            var boardCards = new Dictionary<int, MusicCardData[]>();
            for (int i = 0; i < boardModel.Levels.Length; i++)
            {
                boardCards[boardModel.Levels[i].Level] = boardModel.Levels[i].GetAllCards();
            }
            return boardCards;
        }

        public List<string> GetMusicCardIdsFromBoardThatCanBePurchased(PlayerModel playerModel)
        {
            var playerResources = playerModel.Tokens.CombineCollections(playerModel.GetPurchasedAllResourceCollection());

            var result = new List<string>();
            foreach (var level in boardModel.Levels)
            {
                foreach (var card in level.GetAllCards())
                {
                    if (CanBePurchased(card, playerResources))
                    {
                        result.Add(card.Id);
                    }
                }
            }
            return result;
        }
        public bool CanBePurchased(MusicCardData card, ResourceCollectionModel playerResources)
        {
            var cardCost = card.cost.GetResourceCollectionModel();  
            var needToAdd = playerResources.HowManychNeedToAddToHaveAll(cardCost);
            var numberOfInspirationTokens = playerResources.GetCount(ResourceType.Inspiration);
            return needToAdd <= numberOfInspirationTokens;
        }

        public bool GetRandomCardFromDeck(int cardLevel, out string cardId)
        {
            return boardModel.GetRandomCardFromDeck(cardLevel, out cardId);
        }

        public bool IsCardDeckEmpty(int cardLevel)
        {
            return boardModel.IsCardDeckEmpty(cardLevel);
        }
    }
}