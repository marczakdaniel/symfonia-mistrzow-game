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
            var currentPlayer = turnService.GetCurrentPlayerModel();
            var currentPlayerTokens = currentPlayer.Tokens.GetAllResources();
            var musicCardData = MusicCardRepository.Instance.GetCard(musicCardId);
            var initialTokens = turnService.GetInitialSelectedTokens(musicCardId);  

            turnService.InitializeCardPurchaseTokens(initialTokens);
            var currentCardTokens = currentPlayer.PurchasedCards.GetAllResourceCollection().GetAllResources();  
            var tokensNeededToPurchase = turnService.GetTokensNeededToPurchase(musicCardId);

            var openEvent = new CardPurchaseWindowOpenedEvent(musicCardData, currentPlayerTokens, initialTokens.GetAllResources(), currentCardTokens, tokensNeededToPurchase.GetAllResources());
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

    // Token Return Window

    public class CloseReturnTokenWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseReturnTokenWindow";
        private readonly TurnService turnService;

        public CloseReturnTokenWindowCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.ClearReturnTokens();

            await AsyncEventBus.Instance.PublishAndWaitAsync(new ReturnTokenWindowClosedEvent());
            return true;
        }
    }

    // Concert Cards Window

    public class OpenConcertCardsWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenConcertCardsWindow";
        private readonly TurnService turnService;

        public OpenConcertCardsWindowCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var concertCards = turnService.GetConcertCards();
            var cardData = concertCards.Select(card => card.ConcertCardData).ToList();
            var cardStates = concertCards.Select(card => card.State).ToList();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardsWindowOpenedEvent(cardData, cardStates));
            return true;
        }
    }

    public class CloseConcertCardsWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseConcertCardsWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }
        
        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardsWindowClosedEvent());
            return true;
        }
    }

    // Info Window

    public class OpenInfoWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenInfoWindow";
        private readonly string description;

        public OpenInfoWindowCommand(string description) : base()
        {
            this.description = description;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent(description));
            return true;
        }
    }

    public class CloseInfoWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseInfoWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowClosedEvent());
            return true;
        }
    }

    // Menu Window

    public class OpenStartPageWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenStartPageWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartPageWindowOpenedEvent());
            return true;
        }
    }

    public class CloseStartPageWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseStartPageWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartPageWindowClosedEvent());
            return true;
        }
    }

    public class OpenGameCreationWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenGameCreationWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new GameCreationWindowOpenedEvent());
            return true;
        }
    }

    public class CloseGameCreationWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseGameCreationWindow";
        private readonly ConfigService configService;

        public CloseGameCreationWindowCommand(ConfigService configService) : base()
        {
            this.configService = configService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            configService.ClearPlayers();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new GameCreationWindowClosedEvent());
            return true;
        }
    }

    public class OpenCreatePlayerWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenCreatePlayerWindow";

        private readonly ConfigService configService;

        public OpenCreatePlayerWindowCommand(ConfigService configService) : base()
        {
            this.configService = configService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!configService.CanAddPlayer())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Maksymalna liczba graczy to 4!"));
                return false;
            }

            await AsyncEventBus.Instance.PublishAndWaitAsync(new CreatePlayerWindowOpenedEvent());
            return true;
        }
    }

    public class CloseCreatePlayerWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseCreatePlayerWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new CreatePlayerWindowClosedEvent());
            return true;
        }
    }
}