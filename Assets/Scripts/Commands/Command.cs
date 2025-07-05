using Models;
using Services;
using Cysharp.Threading.Tasks;
using Events;

namespace Command
{
    public class BoardMusicCardClickedCommand : BaseCommand
    {
        public override string CommandType => "BoardMusicCardClicked";

        public BoardMusicCardClickedCommand() : base()
        {
            
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await UniTask.Delay(1000);
            return true;
        }
    }

    public class BuyMusicCardCommand : BasePlayerActionCommand
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

        public override async UniTask<bool> Execute()
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

    public class ReserveMusicCardCommand : BasePlayerActionCommand
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

        public override async UniTask<bool> Execute()
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

    public class StartGameCommand : BaseGameFlowCommand
    {
        public override string CommandType => "StartGame";

        private readonly GameModel gameModel;

        public StartGameCommand(GameModel gameModel) : base()
        {
            this.gameModel = gameModel;
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var startGameEvent = new StartGameEvent();
            await AsyncGameEventPublisher.PublishAsync(startGameEvent);
            return true;
        }
    }
}