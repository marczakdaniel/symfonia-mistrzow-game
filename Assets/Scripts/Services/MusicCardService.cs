namespace Services
{
    /* IDEA FOR FUTEURE
    public interface IMusicCardValidateService
    {
        bool CanBuyMusicCard(string playerId, string musicCardId);
        bool CanReserveMusicCard(string playerId, string musicCardId);
    }

    public interface IMusicCardExecuteService
    {
        bool BuyMusicCard(string playerId, string musicCardId);
        bool ReserveMusicCard(string playerId, string musicCardId);
    }*/

    public interface IMusicCardService
    {
        bool CanBuyMusicCard(string playerId, string musicCardId);
        bool CanReserveMusicCard(string playerId, string musicCardId);
        bool BuyMusicCard(string playerId, string musicCardId);
        bool ReserveMusicCard(string playerId, string musicCardId);
    }

    public class MusicCardService : IMusicCardService
    {
        public bool CanBuyMusicCard(string playerId, string musicCardId)
        {
            return true;
        }

        public bool CanReserveMusicCard(string playerId, string musicCardId)
        {
            return true;
        }

        public bool BuyMusicCard(string playerId, string musicCardId)
        {
            return true;
        }

        public bool ReserveMusicCard(string playerId, string musicCardId)
        {
            return true;
        }
    }
}