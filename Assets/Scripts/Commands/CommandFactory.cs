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

        public CommandFactory(GameModel gameModel, TurnService turnService, BoardService boardService, PlayerService playerService)
        {
            this.gameModel = gameModel;
            this.turnService = turnService;
            this.boardService = boardService;
            this.playerService = playerService;
        }

        // Game Flow Commands

        public StartGameCommand CreateStartGameCommand()
        {
            return new StartGameCommand(gameModel, turnService, boardService);
        }

        // Player Actions Commands

        /*
        
        public BuyMusicCardCommand CreateBuyMusicCardCommand(string playerId, string musicCardId)
        {
            return new BuyMusicCardCommand(playerId, musicCardId, gameModel);
        }

        public ReserveMusicCardCommand CreateReserveMusicCardCommand(string playerId, string musicCardId)
        {
            return new ReserveMusicCardCommand(playerId, musicCardId, gameModel);
        }

        */

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
            return new OpenMusicCardDetailsPanelCommand(musicCardId, level, position, turnService);
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
            return new StartPlayerTurnCommand(gameModel, turnService);
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

        public OpenCardPurchaseWindowCommand CreateOpenCardPurchaseWindowCommand(string musicCardId)
        {       
            return new OpenCardPurchaseWindowCommand(musicCardId, turnService, boardService);
        }

        public CloseCardPurchaseWindowCommand CreateCloseCardPurchaseWindowCommand()
        {
            return new CloseCardPurchaseWindowCommand(turnService);
        }

        // Player Resources Window Commands

        public OpenPlayerResourcesWindowCommand CreateOpenPlayerResourcesWindowCommand(string playerId)
        {
            return new OpenPlayerResourcesWindowCommand(playerId, turnService, playerService);
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
    }
}