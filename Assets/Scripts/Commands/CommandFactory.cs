using Models;
using Services;

namespace Command
{
    public class CommandFactory
    {
        private readonly IMusicCardService musicCardService;
        private readonly IPlayerService playerService;
        private readonly GameModel gameModel;

        public CommandFactory(MusicCardService musicCardService, GameModel gameModel)
        {
            this.musicCardService = musicCardService;
            this.gameModel = gameModel;
        }
        
        public BuyMusicCardCommand CreateBuyMusicCardCommand(string playerId, string musicCardId)
        {
            return new BuyMusicCardCommand(playerId, musicCardId, gameModel);
        }

        public ReserveMusicCardCommand CreateReserveMusicCardCommand(string playerId, string musicCardId)
        {
            return new ReserveMusicCardCommand(playerId, musicCardId, gameModel);
        }

        public StartGameCommand CreateStartGameCommand()
        {
            return new StartGameCommand(gameModel);
        }
    }
}