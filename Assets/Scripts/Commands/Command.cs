using Models;
using DefaultNamespace.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Events;
using Services;
using Cysharp.Threading.Tasks;
using System.Linq;
using JetBrains.Annotations;

namespace Command
{
    public class StartGameCommand : BasePlayerActionCommand
    {
        public override string CommandType => "StartGame";

        private readonly GameService gameService;
        private readonly TurnService turnService;
        private readonly BoardService boardService;
        private readonly PlayerService playerService;
        private readonly ConfigService configService;
        private readonly ConcertCardService concertCardService;

        public StartGameCommand(GameService gameService, TurnService turnService, BoardService boardService, PlayerService playerService, ConfigService configService, ConcertCardService concertCardService ) : base()
        {
            this.turnService = turnService;
            this.boardService = boardService;
            this.gameService = gameService;
            this.playerService = playerService;
            this.configService = configService;
            this.concertCardService = concertCardService;
        }

        public override async UniTask<bool> Validate()
        {
            // TODO: Check if game can be started
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!configService.CanStartGame())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Musisz dodać co najmniej 2 graczy!"));
                return false;
            }

            var gameConfig = configService.GetGameConfig();
            var boardConfig = configService.GetBoardConfig();
            var concertCards = concertCardService.GetRandomConcertCards(gameConfig.GetPlayerConfigs().Count + 1);
            gameConfig.SetupBoardConfig(boardConfig);
            gameConfig.SetupConcertCardsConfig(concertCards.ToArray());

            if (!gameConfig.IsInitialized)
            {
                return false;
            }
            
            gameService.InitializeGame(gameConfig);

            if (!gameService.CanStartGame())
            {
                return false;
            }

            // model update
            // business logic
            // event publish
            // 1. Model update - preparation
            boardService.StartGame();
            var players = playerService.GetPlayers();
            var playerIds = new string[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerIds[i] = players[i].PlayerId;
            }
            // 2. Event publish and wait for UI to complete
            var boardCards = boardService.GetBoardCards();
            var playerAvatars = playerService.GetPlayerAvatars();
            var gameStartedEvent = new GameStartedEvent(playerIds, playerAvatars, boardService.GetAllBoardResources(), boardCards);
            await AsyncEventBus.Instance.PublishAndWaitAsync(gameStartedEvent);

            turnService.NextPlayerTurn();

            var currentPlayer = turnService.GetCurrentPlayerModel();
            var currentRound = turnService.GetCurrentRound();
            var currentPlayerAvatar = currentPlayer.PlayerAvatar;
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartTurnWindowOpenedEvent(currentPlayer.PlayerName, currentRound, currentPlayerAvatar));

            return true;
        }
    }
    
    // Turn commands
    public class StartPlayerTurnCommand : BasePlayerActionCommand
    {
        public override string CommandType => "StartPlayerTurn";
        private readonly TurnService turnService;       
        private readonly BoardService boardService;

        public StartPlayerTurnCommand(GameModel gameModel, TurnService turnService, BoardService boardService) : base()
        {
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.StartPlayerTurn();
            var musicCardIdsThatCanBePurchased = boardService.GetMusicCardIdsFromBoardThatCanBePurchased(turnService.GetCurrentPlayerModel());
            var turnStartedEvent = new TurnStartedEvent(turnService.GetCurrentPlayerId(), musicCardIdsThatCanBePurchased);
            await AsyncEventBus.Instance.PublishAndWaitAsync(turnStartedEvent);

            return true;
        }
    }

    public class EndPlayerTurnCommand : BasePlayerActionCommand
    {
        public override string CommandType => "EndPlayerTurn";
        private readonly TurnService turnService;
        
        public EndPlayerTurnCommand(GameModel gameModel, TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.HasActionBeenTaken())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Nie wykonałeś żadnej akcji!"));
                return false;
            }

            if (turnService.IsTokenReturnNeeded())
            {
                var allPlayerTokensCount = turnService.GetCurrentPlayerModel().Tokens.TotalCount;
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ReturnTokenWindowOpenedEvent(turnService.GetCurrentPlayerModel().Tokens.GetAllResources(), allPlayerTokensCount));
                return true;
            }

            turnService.SetConcertCardReadyToClaimState();

            if (turnService.CanClaimAnyConcertCard())
            {
                var concertCardData = turnService.GetConcertCardsData();
                var cardStates = turnService.GetConcertCardStates();
                turnService.ClaimAllConcertCardsReadyToClaim();
                var ownerAvatars = turnService.GetClaimedConcertCardOwnerAvatar();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardsWindowOpenedEvent(concertCardData, cardStates, ownerAvatars));
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ConcertCardClaimedEvent(turnService.GetCurrentPlayerModel().Points));
                return true;
            }


            if (turnService.IsGameEnded())
            {
                var ranking = turnService.GetRanking();
                var playerNames = ranking.Select(player => player.PlayerName).ToList();
                var playerPoints = ranking.Select(player => player.Points).ToList();
                var playerAvatars = ranking.Select(player => player.PlayerAvatar).ToList();
                var playerIds = ranking.Select(player => player.PlayerId).ToList();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ResultWindowOpenedEvent(playerIds, playerNames, playerPoints, playerAvatars));
                return true;
            }

            turnService.NextPlayerTurn();

            var currentPlayer = turnService.GetCurrentPlayerModel();
            var currentRound = turnService.GetCurrentRound();
            var currentPlayerAvatar = currentPlayer.PlayerAvatar;
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartTurnWindowOpenedEvent(currentPlayer.PlayerName, currentRound, currentPlayerAvatar));

            return true;
        }
    }

    // Select token action
    public class AddTokenToSelectedTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AddTokenToSelectedTokens";
        private readonly ResourceType token;
        private readonly TurnService turnService;
        private readonly BoardService boardService;
        public AddTokenToSelectedTokensCommand(ResourceType token, TurnService turnService, BoardService boardService) : base()
        {
            this.token = token;
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.CanAddTokenToSelectedTokens(token))
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Nie możesz dodać tego tokenu do wybranych tokenów!"));
                return false;
            }

            turnService.AddTokenToSelectedTokens(token);
            var selectedTokens = turnService.GetSelectedTokens();

            var selectedTokenCount = boardService.GetBoardTokenCount(token) - turnService.GetSelectedTokensCount(token);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenAddedToSelectedTokensEvent(token, selectedTokenCount, selectedTokens));

            return true;
        }
    }

    public class RemoveTokenFromSelectedTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "RemoveTokenFromSelectedTokens";
        private readonly ResourceType token;
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public RemoveTokenFromSelectedTokensCommand(ResourceType token, TurnService turnService, BoardService boardService) : base()
        {
            this.token = token;
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.RemoveTokenFromSelectedTokens(token);
            var selectedTokens = turnService.GetSelectedTokens();

            var selectedTokenCount = boardService.GetBoardTokenCount(token) - turnService.GetSelectedTokensCount(token);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenRemovedFromSelectedTokensEvent(token, selectedTokenCount, selectedTokens));
            return true;
        }
    }

    public class AcceptSelectedTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AcceptSelectedTokens";
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public AcceptSelectedTokensCommand(TurnService turnService, BoardService boardService) : base()
        {
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.CanConfirmSelectedTokens())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Nie możesz potwierdzić wybranych tokenów!"));
                return true;
            }

            turnService.ConfirmSelectedTokens();
            turnService.ClearSelectedTokens();
            var boardTokens = boardService.GetAllBoardResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new SelectedTokensConfirmedEvent(boardTokens));

            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());
            return true;
        }
    }

    // Return token action

    public class AddTokenToReturnTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AddTokenToReturnTokens";
        private readonly ResourceType token;
        private readonly TurnService turnService;

        public AddTokenToReturnTokensCommand(ResourceType token, TurnService turnService) : base()
        {
            this.token = token;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.CanAddTokenToReturnTokens(token))
            {
                return false;
            }

            turnService.AddTokenToReturnTokens(token);

            var currentTokenCount = turnService.GetCurrentPlayerModel().Tokens.GetCount(token) - turnService.GetReturnTokensCount(token);   
            var allPlayerTokensCount = turnService.GetCurrentPlayerModel().Tokens.TotalCount - turnService.GetAllReturnTokensCount();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenAddedToReturnTokensEvent(token, currentTokenCount, allPlayerTokensCount, turnService.GetReturnTokens()));
            return true;
        }
    }

    public class RemoveTokenFromReturnTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "RemoveTokenFromReturnTokens";
        private readonly ResourceType token;
        private readonly TurnService turnService;

        public RemoveTokenFromReturnTokensCommand(ResourceType token, TurnService turnService) : base()
        {
            this.token = token;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.RemoveTokenFromReturnTokens(token);

            var currentTokenCount = turnService.GetCurrentPlayerModel().Tokens.GetCount(token) - turnService.GetReturnTokensCount(token);
            var allPlayerTokensCount = turnService.GetCurrentPlayerModel().Tokens.TotalCount - turnService.GetAllReturnTokensCount();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenRemovedFromReturnTokensEvent(token, currentTokenCount, allPlayerTokensCount, turnService.GetReturnTokens()));
            return true;
        }   
    }

    public class ConfirmReturnTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "ConfirmReturnTokens";
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public ConfirmReturnTokensCommand(TurnService turnService, BoardService boardService) : base()
        {
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.ConfirmReturnTokens();

            var boardTokens = boardService.GetAllBoardResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ReturnTokensConfirmedEvent(boardTokens));

            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));

            return true;
        }  
    }

    // Reserve card action
    public class ReserveCardCommand : BasePlayerActionCommand
    {
        public override string CommandType => "ReserveCard";
        private readonly string cardId;
        private readonly TurnService turnService;
        private readonly BoardService boardService;

        public ReserveCardCommand(string cardId, TurnService turnService, BoardService boardService) : base()
        {
            this.cardId = cardId;
            this.turnService = turnService;
            this.boardService = boardService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (turnService.HasActionBeenTaken())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Wykonałeś już akcję w tej turze!"));
                return false;
            }

            if (!turnService.CanReserveCard())
            {
                var description = "Nie możesz zarezerwować tego karty!\n Możesz zarezerwować maksymalnie 3 karty.";
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent(description));
                return false;
            }

            var slot = boardService.GetSlotWithCard(cardId);

            if (slot == null)
            {
                return false;
            }

            turnService.ReserveCard(cardId);


            var inspirationTokens = boardService.GetBoardTokenCount(ResourceType.Inspiration);
            var playerIndex = turnService.GetCurrentPlayerIndex();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new CardReservedEvent(cardId, inspirationTokens, playerIndex));

            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));

            if (boardService.IsCardDeckEmpty(slot.Level))
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());
                return true;                
            }

            boardService.RefillSlot(slot.Level, slot.Position);
            var musicCardData = slot.GetMusicCard();
            var putCardOnBoardEvent = new PutCardOnBoardEvent(slot.Level, slot.Position, musicCardData, boardService.IsCardDeckEmpty(slot.Level));
            await AsyncEventBus.Instance.PublishAndWaitAsync(putCardOnBoardEvent);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());

            return true;
        }
    }

    // Card purchase action
    public class AddTokenToCardPurchaseCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AddTokenToCardPurchase";
        private readonly ResourceType token;
        private readonly TurnService turnService;
        private readonly string cardId;

        public AddTokenToCardPurchaseCommand(string cardId, ResourceType token, TurnService turnService) : base()
        {
            this.token = token;
            this.cardId = cardId;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.CanAddTokenToCardPurchase(cardId, token))
            {
                var description = "Nie możesz dodać tego tokenu do zakupu karty!";
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent(description));
                return false;
            }

            turnService.AddTokenToCardPurchase(token);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenAddedToCardPurchaseEvent(token, turnService.GetCardPurchaseTokensCount(token)));
            return true;
        }
    }

    public class RemoveTokenFromCardPurchaseCommand : BasePlayerActionCommand
    {
        public override string CommandType => "RemoveTokenFromCardPurchase";
        private readonly ResourceType token;
        private readonly TurnService turnService;

        public RemoveTokenFromCardPurchaseCommand(ResourceType token, TurnService turnService) : base()
        {
            this.token = token;
            this.turnService = turnService;
        }
        
        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (!turnService.CanRemoveTokenFromCardPurchase(token))
            {
                return false;
            }

            turnService.RemoveTokenFromCardPurchase(token);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenRemovedFromCardPurchaseEvent(token, turnService.GetCardPurchaseTokensCount(token)));
            return true;
        }
    }

    
    public class PurchaseCardCommand : BasePlayerActionCommand
    {
        public override string CommandType => "PurchaseCard";
        private readonly string cardId;
        private readonly TurnService turnService;
        private readonly BoardService boardService;
        private readonly PlayerService playerService;

        public PurchaseCardCommand(string cardId, TurnService turnService, BoardService boardService, PlayerService playerService) : base()
        {
            this.cardId = cardId;
            this.turnService = turnService;
            this.boardService = boardService;
            this.playerService = playerService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (turnService.HasActionBeenTaken())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Wykonałeś już akcję w tej turze!"));
                return false;
            }

            var selectedTokens = turnService.GetCardPurchaseTokens();

            if (!turnService.CanPurchaseCardWithResources(cardId, selectedTokens))
            {
                var description = "Zakup się nie powiódł!";
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent(description));
                return false;
            }

            var slot = boardService.GetSlotWithCard(cardId);
            if (slot != null)
            {
                var musicCardData = slot.GetMusicCard();
                turnService.PurchaseCardFromBoard(cardId, selectedTokens);
                var points = playerService.GetPlayer(turnService.GetCurrentPlayerId()).Points;  
                var playerIndex = turnService.GetCurrentPlayerIndex();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchasedFromBoardEvent(cardId, boardService.GetAllBoardResources(), playerIndex, points));

                var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
                var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));

                if (boardService.IsCardDeckEmpty(slot.Level))
                {
                    await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());
                    return true;
                }

                boardService.RefillSlot(slot.Level, slot.Position);
                var putCardOnBoardEvent = new PutCardOnBoardEvent(slot.Level, slot.Position, slot.GetMusicCard(), boardService.IsCardDeckEmpty(slot.Level));
                await AsyncEventBus.Instance.PublishAndWaitAsync(putCardOnBoardEvent);
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());
                return true;
            }

            var currentPlayer = turnService.GetCurrentPlayerModel();
            if (currentPlayer.HasReserveCard(cardId))
            {
                turnService.PurchaseCardFromReserve(cardId, selectedTokens);                
                var reservedCards = currentPlayer.ReservedCards.GetAllCards().ToList();
                var points = playerService.GetPlayer(turnService.GetCurrentPlayerId()).Points;  
                var playerIndex = turnService.GetCurrentPlayerIndex();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchasedFromReserveEvent(cardId, reservedCards, boardService.GetAllBoardResources(), playerIndex, points));

                var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
                var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ShowNextTurnButtonEvent());

                return true;
            }

            UnityEngine.Debug.LogError("Card purchased");
            return true;
        }
    }

    public class AddPlayerCommand : BaseUICommand
    {
        public override string CommandType => "AddPlayer";

        private readonly ConfigService configService;
        private readonly string playerName;
        private readonly Sprite playerAvatar;

        public AddPlayerCommand(string playerName, Sprite playerAvatar, ConfigService configService) : base()
        {
            this.playerName = playerName;
            this.configService = configService;
            this.playerAvatar = playerAvatar;
        }   

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            configService.AddPlayer(playerName, playerAvatar);
            var playerNames = configService.GetPlayerNames();
            var playerAvatars = configService.GetPlayerAvatars();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerAddedEvent(playerNames, playerAvatars));
            return true;
        }
    }

    public class ReserveDeckCardCommand : BaseUICommand
    {
        public override string CommandType => "ReserveDeckCard";

        private readonly TurnService turnService;
        private readonly BoardService boardService;
        private readonly int cardLevel;
        public ReserveDeckCardCommand(int cardLevel, TurnService turnService, BoardService boardService) : base()
        {
            this.turnService = turnService;
            this.boardService = boardService;
            this.cardLevel = cardLevel;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            if (turnService.HasActionBeenTaken())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Wykonałeś już akcję w tej turze!"));
                return false;
            }

            if (!turnService.CanReserveCard())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new InfoWindowOpenedEvent("Nie możesz zarezerwować karty z talii!"));
                return false;
            }

            boardService.GetRandomCardFromDeck(cardLevel, out var cardId);
            var musicCardData = MusicCardRepository.Instance.GetCard(cardId);

            turnService.ReserveDeckCard(cardId);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new DeckCardReservedEvent(musicCardData, boardService.IsCardDeckEmpty(cardLevel)));

            var currentPlayerTokens = turnService.GetCurrentPlayerModel().Tokens.GetAllResources();
            var currentPlayerCards = turnService.GetCurrentPlayerModel().GetPurchasedAllResourceCollection().GetAllResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new PlayerResourcesUpdatedEvent(turnService.GetCurrentPlayerId(), currentPlayerTokens, currentPlayerCards));

            return true;
        }
    }
}