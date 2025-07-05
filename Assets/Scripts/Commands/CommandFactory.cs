using Services;

namespace Command
{
    public class CommandFactory
    {
        private readonly IMusicCardService musicCardService;
        private readonly IPlayerService playerService;

        public CommandFactory(MusicCardService musicCardService)
        {
            this.musicCardService = musicCardService;
        }
        
        public BuyMusicCardCommand CreateBuyMusicCardCommand(string playerId, string musicCardId)
        {
            return new BuyMusicCardCommand(playerId, musicCardId, musicCardService, playerService);
        }

        public ReserveMusicCardCommand CreateReserveMusicCardCommand(string playerId, string musicCardId)
        {
            return new ReserveMusicCardCommand(playerId, musicCardId);
        }
    }
}