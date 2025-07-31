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
    }
}