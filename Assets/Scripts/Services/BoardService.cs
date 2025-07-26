using System.Collections.Generic;
using DefaultNamespace.Data;
using Models;

namespace Services
{
    public class BoardService
    {
        private readonly GameModel gameModel;
        private BoardModel boardModel => gameModel.board;

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
    }
}