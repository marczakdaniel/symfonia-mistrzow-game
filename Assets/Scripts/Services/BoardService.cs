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
    }
}