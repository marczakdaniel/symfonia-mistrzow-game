using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using Services;

namespace Command
{
    public class OpenMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }
        private readonly TurnService turnService;

        public OpenMusicCardDetailsPanelCommand(string musicCardId, int level, int position, TurnService turnService) : base()
        {
            MusicCardId = musicCardId;
            Level = level;
            Position = position;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }   

        public override async UniTask<bool> Execute()
        {
            var canCardBePurchased = true;
            var canCardBeReserved = true;

            turnService.StartSelectingMusicCard();

            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelOpenedEvent(MusicCardId, Level, Position, canCardBePurchased, canCardBeReserved));
            return true;
        }
    }

    public class CloseMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "CloseMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }
        private readonly TurnService turnService;

        public CloseMusicCardDetailsPanelCommand(string musicCardId, TurnService turnService) : base()
        {
            MusicCardId = musicCardId;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.StopSelectingMusicCard();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelClosedEvent(MusicCardId));
            return true;
        }
    }  

    public class OpenTokenDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenTokenDetailsPanel";
        public ResourceType ResourceType { get; private set; }
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public OpenTokenDetailsPanelCommand(ResourceType resourceType, TurnService turnService, BoardService boardService) : base()
        {
            ResourceType = resourceType;
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.StartSelectingTokens();
            var currentTokenCounts = boardService.GetAllBoardResources();
            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            if (!turnService.CanAddTokenToSelectedTokens(ResourceType))
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelOpenedEvent(null, currentTokenCounts, currentPlayerTokens));
                return false;
            }
            turnService.AddTokenToSelectedTokens(ResourceType);

            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelOpenedEvent(ResourceType, currentTokenCounts, currentPlayerTokens));
            return true;
        }
    }

    public class CloseTokenDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "CloseTokenDetailsPanel";
        private readonly TurnService turnService;
        public CloseTokenDetailsPanelCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.EndSelectingTokensWithNoConfirmation();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelClosedEvent());
            
            return true;
        }
    }

    public class OpenCardPurchaseWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenCardPurchaseWindow";
        private readonly TurnService turnService;
        private readonly string musicCardId;
        private readonly BoardService boardService;
        public OpenCardPurchaseWindowCommand(string musicCardId, TurnService turnService, BoardService boardService) : base()
        {
            this.turnService = turnService;
            this.musicCardId = musicCardId;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var musicCardData = MusicCardRepository.Instance.GetCard(musicCardId);

            var openEvent = new CardPurchaseWindowOpenedEvent(musicCardData, currentPlayerTokens, 0);
            await AsyncEventBus.Instance.PublishAndWaitAsync(openEvent);

            return true;
        }
    }   

    public class CloseCardPurchaseWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseCardPurchaseWindow";
        private readonly TurnService turnService;
        public CloseCardPurchaseWindowCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.ClearCardPurchaseTokens();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchaseWindowClosedEvent());
            return true;
        }
    }

    // Player Resources Window

    public class OpenPlayerResourcesWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenPlayerResourcesWindow";
        private readonly string playerId;
        private readonly TurnService turnService;
        private readonly PlayerService playerService;
        public OpenPlayerResourcesWindowCommand(string playerId, TurnService turnService, PlayerService playerService) : base()
        {
            this.playerId = playerId;
            this.turnService = turnService;
            this.playerService = playerService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var isCurrentPlayer = turnService.GetCurrentPlayerId() == playerId;
            var player = playerService.GetPlayer(playerId);
            var numberOfPoints = player.Points;
            var playerName = player.PlayerName;
            var currentPlayerTokens = player.Tokens.GetAllResources();
            var currentPlayerCards = player.PurchasedCards.GetAllResourceCollection().GetAllResources();
            var reservedMusicCards = player.ReservedCards.GetAllCards().ToList();

            var openEvent = new PlayerResourcesWindowOpenedEvent(isCurrentPlayer, playerName, numberOfPoints, currentPlayerTokens, currentPlayerCards, reservedMusicCards);
            await AsyncEventBus.Instance.PublishAndWaitAsync(openEvent);
            return true;
        }
    }

    public class ClosePlayerResourcesWindowCommand : BaseUICommand
    {
        public override string CommandType => "ClosePlayerResourcesWindow";

        public ClosePlayerResourcesWindowCommand() : base()
        {
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesWindowClosedEvent());
            return true;
        }
    }
    /*
    public enum GameWindowType
    {
        MusicCardDetailsPanel,
    }

    public class GameWindowParameters
    {
        public GameWindowType WindowType { get; private set; }
        public GameWindowParameters(GameWindowType windowType)
        {
            WindowType = windowType;
    }



    public class OpenGameWindowTypeCommand : BaseUICommand
    {
        public override string CommandType => "OpenGameWindowType";
        public GameWindowType WindowType { get; private set; }

        public OpenGameWindowTypeCommand(GameWindowType windowType,  GameModel gameModel) : base(gameModel)
        {
            WindowType = windowType;
        }

        public override bool Validate()
        {
            return ValidateWindowType(WindowType);
        }

        public override async UniTask<bool> Execute()
        {
            return await ExecuteWindowType(WindowType);
        }

        private bool ValidateWindowType(GameWindowType windowType)
        {
            switch (windowType)
            {
                case GameWindowType.MusicCardDetailsPanel:
                    return true;
                default:
                    return false;
            }
        }

        public UniTask<bool> ExecuteWindowType(GameWindowType windowType)
        {
            switch (windowType)
            {
                case GameWindowType.MusicCardDetailsPanel:
                    return ExecuteMusicCardDetailsPanel();
                default:
                    return UniTask.FromResult(false);
            }
        }

        public async UniTask<bool> ExecuteMusicCardDetailsPanel()
        {
            Debug.Log("Open Music Card Details Panel");
            await UniTask.CompletedTask;
            return true;
        }

    }*/
}