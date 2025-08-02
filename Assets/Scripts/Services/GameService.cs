using Models;

namespace Services
{
    public class GameService
    {
        private readonly GameModel gameModel;
        public GameService(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public void InitializeGame(GameConfig gameConfig)
        {
            gameModel.Initialize(gameConfig);
        }

        public bool CanStartGame()
        {
            return gameModel.CanStartGame();
        }
    }
}