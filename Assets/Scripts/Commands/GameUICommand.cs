using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using Models;
using Services;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.SceneManagement;

namespace Command
{
    public class OpenMusicCardDetailsPanelCommand : BaseUICommand
    {
        public override string CommandType => "OpenMusicCardDetailsPanel";
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public OpenMusicCardDetailsPanelCommand(string musicCardId, int level, int position, TurnService turnService, BoardService boardService) : base()
        {
            MusicCardId = musicCardId;
            Level = level;
            Position = position;
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }   

        public override async UniTask<bool> Execute()
        {
            var musicCardData = MusicCardRepository.Instance.GetCard(MusicCardId);
            var currentPlayer = turnService.GetCurrentPlayerModel();
            var currentPlayerResources = currentPlayer.Tokens.CombineCollections(currentPlayer.GetPurchasedAllResourceCollection());
            var canBePurchased = boardService.CanBePurchased(musicCardData, currentPlayerResources);

            await AsyncEventBus.Instance.PublishAndWaitAsync(new MusicCardDetailsPanelOpenedEvent(musicCardData, Level, Position, canBePurchased));
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
            var currentTokenCounts = boardService.GetAllBoardResources();
            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
            if (!turnService.CanAddTokenToSelectedTokens(ResourceType))
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelOpenedEvent(null, currentTokenCounts, currentPlayerTokens, currentPlayerCards));
                return false;
            }
            turnService.AddTokenToSelectedTokens(ResourceType);

            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelOpenedEvent(ResourceType, currentTokenCounts, currentPlayerTokens, currentPlayerCards));
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
            turnService.ClearSelectedTokens();
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
        private readonly bool isFromMusicCardDetailsPanel;
        private readonly int cardIndex;
        private readonly PlayerService playerService;
        public OpenCardPurchaseWindowCommand(string musicCardId, bool isFromMusicCardDetailsPanel, int cardIndex, TurnService turnService, BoardService boardService, PlayerService playerService) : base()
        {
            this.turnService = turnService;
            this.musicCardId = musicCardId;
            this.boardService = boardService;
            this.isFromMusicCardDetailsPanel = isFromMusicCardDetailsPanel;
            this.cardIndex = cardIndex;
            this.playerService = playerService;
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
            var currentCardTokens = currentPlayer.GetPurchasedAllResourceCollection().GetAllResources();  
            var tokensNeededToPurchase = turnService.GetTokensNeededToPurchase(musicCardId);

            if (isFromMusicCardDetailsPanel)
            {
                var playerResources = playerService.GetPlayerResourcesFromCardAndTokens(turnService.GetCurrentPlayerId());
                var canBePurchased = boardService.CanBePurchased(musicCardData, playerResources);
                var openEvent = new CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent(musicCardData, currentPlayerTokens, initialTokens.GetAllResources(), currentCardTokens, tokensNeededToPurchase.GetAllResources(), canBePurchased);
                await AsyncEventBus.Instance.PublishAndWaitAsync(openEvent);
            }
            else
            {
                var playerResources = playerService.GetPlayerResourcesFromCardAndTokens(turnService.GetCurrentPlayerId());
                var canBePurchased = boardService.CanBePurchased(musicCardData, playerResources);
                var openEvent = new CardPurchaseWindowOpenedFromReservedEvent(musicCardData, currentPlayerTokens, initialTokens.GetAllResources(), currentCardTokens, tokensNeededToPurchase.GetAllResources(), cardIndex, canBePurchased);
                await AsyncEventBus.Instance.PublishAndWaitAsync(openEvent);
            }

            return true;
        }
    }   

    public class CloseCardPurchaseWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseCardPurchaseWindow";
        private readonly TurnService turnService;
        private readonly bool isFromMusicCardDetailsPanel;
        private readonly int cardIndex;
        public CloseCardPurchaseWindowCommand(bool isFromMusicCardDetailsPanel, int cardIndex, TurnService turnService) : base()
        {
            this.isFromMusicCardDetailsPanel = isFromMusicCardDetailsPanel;
            this.cardIndex = cardIndex;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.ClearCardPurchaseTokens();
            if (isFromMusicCardDetailsPanel)
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent());
            }
            else
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchaseWindowClosedFromReservedEvent(cardIndex));
            }
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
        private readonly BoardService boardService;
        public OpenPlayerResourcesWindowCommand(string playerId, TurnService turnService, PlayerService playerService, BoardService boardService) : base()
        {
            this.playerId = playerId;
            this.turnService = turnService;
            this.playerService = playerService;
            this.boardService = boardService;
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
            var currentPlayerCards = player.GetPurchasedAllResourceCollection().GetAllResources();
            var reservedMusicCards = player.ReservedCards.GetAllCards().ToList();

            var allResource = playerService.GetPlayerResourcesFromCardAndTokens(playerId);
            var reservedMusicCardsThatCanBePurchased = player.ReservedCards.GetAllCards().Where(card => boardService.CanBePurchased(card, allResource)).Select(card => card.Id).ToList();
            var canPlayerExecuteAction = !turnService.HasActionBeenTaken();
            var playerAvatar = player.PlayerAvatar;
            var openEvent = new PlayerResourcesWindowOpenedEvent(isCurrentPlayer, canPlayerExecuteAction, playerName, numberOfPoints, playerAvatar, currentPlayerTokens, currentPlayerCards, reservedMusicCards, reservedMusicCardsThatCanBePurchased);
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
            turnService.SetConcertCardReadyToClaimState();
            var canClaimAnyConcertCard = turnService.CanClaimAnyConcertCard();

            var cardData = turnService.GetConcertCardsData();
            var cardStates = turnService.GetConcertCardStates();

            if (canClaimAnyConcertCard)
            {
                turnService.ClaimAllConcertCardsReadyToClaim();
            }

            var ownerAvatars = turnService.GetClaimedConcertCardOwnerAvatar();

            await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardsWindowOpenedEvent(cardData, cardStates, ownerAvatars));

            if (canClaimAnyConcertCard)
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardClaimedEvent(turnService.GetCurrentPlayerModel().Points));
            }

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
            if (!MusicManager.Instance.IsPlaying())
            {
                MusicManager.Instance.PlayMusic();
            }
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

            var availablePlayerAvatars = configService.GetAvailablePlayerAvatars();

            await AsyncEventBus.Instance.PublishAndWaitAsync(new CreatePlayerWindowOpenedEvent(availablePlayerAvatars));
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

    public class OpenReserveDeckCardWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenReserveDeckCardWindow";

        private readonly int cardLevel;
        private readonly BoardService boardService;

        public OpenReserveDeckCardWindowCommand(int cardLevel, BoardService boardService) : base()
        {
            this.cardLevel = cardLevel;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var isEmpty = boardService.IsCardDeckEmpty(cardLevel);
            if (isEmpty)
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Nide ma już kart na stosie!"));
                return false;
            }

            await AsyncEventBus.Instance.PublishAndWaitAsync(new ReserveDeckCardWindowOpenedEvent(cardLevel));

            return true;
        }
    }

    public class CloseReserveDeckCardWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseReserveDeckCardWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ReserveDeckCardWindowClosedEvent());
            return true;
        }
    }

    public class CloseDeckCardInfoWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseDeckCardInfoWindow";
        private readonly TurnService turnService;

        public CloseDeckCardInfoWindowCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var playerIndex = turnService.GetCurrentPlayerIndex();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new DeckCardInfoWindowClosedEvent(playerIndex));
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());
            return true;
        }
    }

    public class OpenResultPlayerResourcesWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenResultPlayerResourcesWindow";

        private readonly PlayerService playerService;

        private readonly string playerId;

            public OpenResultPlayerResourcesWindowCommand(string playerId, PlayerService playerService) : base()
        {
            this.playerId = playerId;
            this.playerService = playerService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            var player = playerService.GetPlayer(playerId);
            var playerMusicCardDatas = player.GetPurchasedMusicCardDatas();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ResultPlayerResourcesWindowOpenedEvent(playerMusicCardDatas));
            return true;
        }
    }

    public class CloseResultPlayerResourcesWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseResultPlayerResourcesWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            return true;
        }
    }

    // Settings Window

    public class OpenSettingsWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenSettingsWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new SettingsWindowOpenedEvent());
            return true;
        }
    }

    public class CloseSettingsWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseSettingsWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new SettingsWindowClosedEvent());
            return true;
        }
    }

    public class RestartGameCommand : BaseUICommand
    {
        public override string CommandType => "RestartGame";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return true;
        }
    }

    // Instruction Window

    public class OpenInstructionWindowCommand : BaseUICommand
    {
        public override string CommandType => "OpenInstructionWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new InstructionWindowOpenedEvent());
            return true;
        }
    }

    public class CloseInstructionWindowCommand : BaseUICommand
    {
        public override string CommandType => "CloseInstructionWindow";

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new InstructionWindowClosedEvent());
            return true;
        }
    }
}