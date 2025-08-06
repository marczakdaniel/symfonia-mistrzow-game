using System;
using DefaultNamespace.Data;
using Models;
using Services;

namespace Command
{
    public class CommandFactory
    {
        private readonly GameModel gameModel;
        private readonly TurnService turnService;
        private readonly BoardService boardService;
        private readonly PlayerService playerService;
        private readonly ConfigService configService;
        private readonly GameService gameService;
        private readonly ConcertCardService concertCardService;

        public CommandFactory(GameModel gameModel, TurnService turnService, BoardService boardService, PlayerService playerService, ConfigService configService, GameService gameService, ConcertCardService concertCardService)
        {
            this.gameModel = gameModel;
            this.turnService = turnService;
            this.boardService = boardService;
            this.playerService = playerService;
            this.configService = configService;
            this.gameService = gameService;
            this.concertCardService = concertCardService;
        }

        // Game Flow Commands

        public StartGameCommand CreateStartGameCommand()
        {
            return new StartGameCommand(gameService, turnService, boardService, playerService, configService, concertCardService);
        }

        // Token Action Commands

        public AddTokenToSelectedTokensCommand CreateAddTokenToSelectedTokensCommand(ResourceType token)
        {
            return new AddTokenToSelectedTokensCommand(token, turnService, boardService);
        }

        public RemoveTokenFromSelectedTokensCommand CreateRemoveTokenFromSelectedTokensCommand(ResourceType token)
        {
            return new RemoveTokenFromSelectedTokensCommand(token, turnService, boardService);
        }

        // UI Commands
        
        public OpenMusicCardDetailsPanelCommand CreateOpenMusicCardDetailsPanelCommand(string musicCardId, int level, int position)
        {
            return new OpenMusicCardDetailsPanelCommand(musicCardId, level, position, turnService, boardService);
        }

        public CloseMusicCardDetailsPanelCommand CreateCloseMusicCardDetailsPanelCommand(string musicCardId)
        {
            return new CloseMusicCardDetailsPanelCommand(musicCardId, turnService);
        }

        public OpenTokenDetailsPanelCommand CreateOpenTokenDetailsPanelCommand(ResourceType resourceType)
        {
            return new OpenTokenDetailsPanelCommand(resourceType, turnService, boardService);
        }

        public CloseTokenDetailsPanelCommand CreateCloseTokenDetailsPanelCommand()
        {
            return new CloseTokenDetailsPanelCommand(turnService);
        }

        public AcceptSelectedTokensCommand CreateAcceptSelectedTokensCommand()
        {
            return new AcceptSelectedTokensCommand(turnService, boardService);
        }

        public EndPlayerTurnCommand CreateEndPlayerTurnCommand()
        {
            return new EndPlayerTurnCommand(gameModel, turnService);
        }

        public StartPlayerTurnCommand CreateStartPlayerTurnCommand()
        {
            return new StartPlayerTurnCommand(gameModel, turnService, boardService);
        }

        // Return token action commands

        public AddTokenToReturnTokensCommand CreateAddTokenToReturnTokensCommand(ResourceType token)
        {
            return new AddTokenToReturnTokensCommand(token, turnService);
        }

        public RemoveTokenFromReturnTokensCommand CreateRemoveTokenFromReturnTokensCommand(ResourceType token)
        {
            return new RemoveTokenFromReturnTokensCommand(token, turnService);
        }

        public ConfirmReturnTokensCommand CreateConfirmReturnTokensCommand()
        {
            return new ConfirmReturnTokensCommand(turnService, boardService);
        }

        public CloseReturnTokenWindowCommand CreateCloseReturnTokenWindowCommand()
        {
            return new CloseReturnTokenWindowCommand(turnService);
        }

        public ReserveCardCommand CreateReserveCardCommand(string cardId)
        {
            return new ReserveCardCommand(cardId, turnService, boardService);
        }

        // Card purchase action commands

        public AddTokenToCardPurchaseCommand CreateAddTokenToCardPurchaseCommand(string cardId, ResourceType token)
        {
            return new AddTokenToCardPurchaseCommand(cardId, token, turnService);
        }

        public RemoveTokenFromCardPurchaseCommand CreateRemoveTokenFromCardPurchaseCommand(ResourceType token)
        {
            return new RemoveTokenFromCardPurchaseCommand(token, turnService);
        }

        public PurchaseCardCommand CreatePurchaseCardCommand(string cardId)
        {
            return new PurchaseCardCommand(cardId, turnService, boardService, playerService );
        }

        public OpenCardPurchaseWindowCommand CreateOpenCardPurchaseWindowCommandFromMusicCardDetailsPanel(string musicCardId)
        {
            return new OpenCardPurchaseWindowCommand(musicCardId, true, -1, turnService, boardService, playerService);
        }

        public OpenCardPurchaseWindowCommand CreateOpenCardPurchaseWindowCommandFromReserved(string musicCardId, int cardIndex)
        {
            return new OpenCardPurchaseWindowCommand(musicCardId, false, cardIndex, turnService, boardService, playerService);
        }

        public CloseCardPurchaseWindowCommand CreateCloseCardPurchaseWindowCommandFromMusicCardDetailsPanel()
        {
            return new CloseCardPurchaseWindowCommand(true, -1, turnService);
        }

        public CloseCardPurchaseWindowCommand CreateCloseCardPurchaseWindowCommandFromReserved(int cardIndex)
        {
            return new CloseCardPurchaseWindowCommand(false, cardIndex, turnService);
        }

        // Player Resources Window Commands

        public OpenPlayerResourcesWindowCommand CreateOpenPlayerResourcesWindowCommand(string playerId)
        {
            return new OpenPlayerResourcesWindowCommand(playerId, turnService, playerService, boardService);
        }

        public ClosePlayerResourcesWindowCommand CreateClosePlayerResourcesWindowCommand()
        {
            return new ClosePlayerResourcesWindowCommand();
        }

        // Concert Cards Window Commands

        public OpenConcertCardsWindowCommand CreateOpenConcertCardsWindowCommand()
        {
            return new OpenConcertCardsWindowCommand(turnService);
        }

        public CloseConcertCardsWindowCommand CreateCloseConcertCardsWindowCommand()
        {
            return new CloseConcertCardsWindowCommand();
        }

        // Info Window Commands

        public OpenInfoWindowCommand CreateOpenInfoWindowCommand(string description)
        {
            return new OpenInfoWindowCommand(description);
        }

        public CloseInfoWindowCommand CreateCloseInfoWindowCommand()
        {
            return new CloseInfoWindowCommand();
        }

        // Start Page Window Commands

        public OpenStartPageWindowCommand CreateOpenStartPageWindowCommand()
        {
            return new OpenStartPageWindowCommand();
        }

        public CloseStartPageWindowCommand CreateCloseStartPageWindowCommand()
        {
            return new CloseStartPageWindowCommand();
        }

        // Game Creation Window Commands

        public OpenGameCreationWindowCommand CreateOpenGameCreationWindowCommand()
        {
            return new OpenGameCreationWindowCommand();
        }

        public CloseGameCreationWindowCommand CreateCloseGameCreationWindowCommand()
        {
            return new CloseGameCreationWindowCommand(configService);
        }

        // Create Player Window Commands

        public OpenCreatePlayerWindowCommand CreateOpenCreatePlayerWindowCommand()
        {
            return new OpenCreatePlayerWindowCommand(configService);
        }

        public CloseCreatePlayerWindowCommand CreateCloseCreatePlayerWindowCommand()    
        {
            return new CloseCreatePlayerWindowCommand();
        }

        public AddPlayerCommand CreateAddPlayerCommand(string playerName)
        {
            return new AddPlayerCommand(playerName, configService);
        }

        public OpenReserveDeckCardWindowCommand CreateOpenReserveDeckCardWindowCommand(int cardLevel)
        {
            return new OpenReserveDeckCardWindowCommand(cardLevel, boardService);
        }
        
        public CloseReserveDeckCardWindowCommand CreateCloseReserveDeckCardWindowCommand()
        {
            return new CloseReserveDeckCardWindowCommand();
        }

        public ReserveDeckCardCommand CreateReserveDeckCardCommand(int cardLevel)
        {
            return new ReserveDeckCardCommand(cardLevel, turnService, boardService);
        }

        // Deck Card Info Window Commands

        public CloseDeckCardInfoWindowCommand CreateCloseDeckCardInfoWindowCommand()
        {
            return new CloseDeckCardInfoWindowCommand(turnService);
        }   
    }
}