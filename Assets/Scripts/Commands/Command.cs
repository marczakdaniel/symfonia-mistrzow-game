using Models;
using Services;

namespace Command
{
    public class BoardMusicCardClickedCommand : BaseCommand
    {
        public override string CommandType => "BoardMusicCardClicked";

        private readonly IMusicCardService musicCardService;

        public BoardMusicCardClickedCommand(string playerId, IMusicCardService musicCardService) : base(playerId)
        {
            this.musicCardService = musicCardService;
        }

        public override bool Validate()
        {
            return true;
        }

        public override bool Execute()
        {
            return true;
        }
    }

    public class BuyMusicCardCommand : BaseCommand
    {
        public override string CommandType => "BuyMusicCard";
        public string MusicCardId { get; private set; }

        private readonly GameModel gameModel;

        public BuyMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId)
        {
            this.MusicCardId = musicCardId;
            this.gameModel = gameModel;
        }

        public override bool Validate()
        {
            // TODO: Check if 
            // - Game is active
            // - Player exists
            // - Player is current player
            // - Player has enough tokens
            // - Music card is on board
            return true;
        }

        public override bool Execute()
        {
            // TODO: Execute:
            // - Remove tokens from player
            // - Add music card to player
            // - End turn
            // - Add new card to board
            // - Change current player

            // Change model
            //var result = gameModel.PurchaseCard(PlayerId, MusicCardId);

            //if (!result)
            //{
            //    return false;
            //}

            // TODO: Inform UI by EventBus

            return true;
        }
    }

    public class ReserveMusicCardCommand : BaseCommand
    {
        public override string CommandType => "ReserveMusicCard";
        public string MusicCardId { get; private set; }
        private readonly GameModel gameModel;

        public ReserveMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId)
        {
            MusicCardId = musicCardId;
            this.gameModel = gameModel;
        }

        public override bool Validate()
        {
            return true;
        }

        public override bool Execute()
        {
            var result = gameModel.ReserveCard(PlayerId, MusicCardId);

            if (!result)
            {
                return false;
            }

            // TODO: Inform UI by EventBus
            return true;
        }
    }
}