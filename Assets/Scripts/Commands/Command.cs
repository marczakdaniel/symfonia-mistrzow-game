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

        private readonly IMusicCardService musicCardService;
        private readonly IPlayerService playerService;

        private readonly GameModel gameModel;

        public BuyMusicCardCommand(string playerId, string musicCardId, IMusicCardService musicCardService, IPlayerService playerService) : base(playerId)
        {
            this.MusicCardId = musicCardId;
            this.musicCardService = musicCardService;
            this.playerService = playerService;
        }

        public override bool Validate()
        {
            // TODO: Check if 
            // - Game is active
            // - Player exists
            // - Player is current player
            // - Player has enough tokens
            // - Music card is on board
            return playerService.IsPlayerTurn(PlayerId) && musicCardService.CanBuyMusicCard(PlayerId, MusicCardId);
        }

        public override bool Execute()
        {
            // TODO: Execute:
            // - Remove tokens from player
            // - Add music card to player
            // - End turn
            // - Add new card to board
            // - Change current player

            var playerModel = gameModel.GetPlayer(PlayerId);
            //gameModel.Board.PurchaseCard(MusicCardId);
            

            //var musicCardModel = musicCardService.GetMusicCard(MusicCardId);

            


            return musicCardService.BuyMusicCard(PlayerId, MusicCardId);
        }
    }

    public class ReserveMusicCardCommand : BaseCommand
    {
        public override string CommandType => "ReserveMusicCard";
        public string MusicCardId { get; private set; }

        public ReserveMusicCardCommand(string playerId, string musicCardId) : base(playerId)
        {
            MusicCardId = musicCardId;
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
}