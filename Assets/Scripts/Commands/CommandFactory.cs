using Models;

namespace Command
{
    public class CommandFactory
    {
        private readonly GameModel gameModel;

        public CommandFactory(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        // Game Flow Commands

        public StartGameCommand CreateStartGameCommand()
        {
            return new StartGameCommand(gameModel);
        }

        // Player Actions Commands
        
        public BuyMusicCardCommand CreateBuyMusicCardCommand(string playerId, string musicCardId)
        {
            return new BuyMusicCardCommand(playerId, musicCardId, gameModel);
        }

        public ReserveMusicCardCommand CreateReserveMusicCardCommand(string playerId, string musicCardId)
        {
            return new ReserveMusicCardCommand(playerId, musicCardId, gameModel);
        }
    }
}