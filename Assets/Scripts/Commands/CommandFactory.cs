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

        public CommandFactory(GameModel gameModel, TurnService turnService)
        {
            this.gameModel = gameModel;
            this.turnService = turnService;
        }

        // Game Flow Commands

        public StartGameCommand CreateStartGameCommand()
        {
            return new StartGameCommand(gameModel, turnService);
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
            return new AddTokenToSelectedTokensCommand(token, gameModel, turnService);
        }

        public RemoveTokenFromSelectedTokensCommand CreateRemoveTokenFromSelectedTokensCommand(ResourceType token)
        {
            return new RemoveTokenFromSelectedTokensCommand(token, gameModel);
        }

        // UI Commands
        
        public OpenMusicCardDetailsPanelCommand CreateOpenMusicCardDetailsPanelCommand(string musicCardId, int level, int position)
        {
            return new OpenMusicCardDetailsPanelCommand(musicCardId, level, position, gameModel);
        }

        public CloseMusicCardDetailsPanelCommand CreateCloseMusicCardDetailsPanelCommand(string musicCardId)
        {
            return new CloseMusicCardDetailsPanelCommand(musicCardId, gameModel);
        }

        public OpenTokenDetailsPanelCommand CreateOpenTokenDetailsPanelCommand(ResourceType resourceType)
        {
            return new OpenTokenDetailsPanelCommand(resourceType, gameModel, turnService);
        }

        public CloseTokenDetailsPanelCommand CreateCloseTokenDetailsPanelCommand()
        {
            return new CloseTokenDetailsPanelCommand(gameModel);
        }

        public AcceptSelectedTokensCommand CreateAcceptSelectedTokensCommand()
        {
            return new AcceptSelectedTokensCommand(gameModel, turnService);
        }

        public EndPlayerTurnCommand CreateEndPlayerTurnCommand()
        {
            return new EndPlayerTurnCommand(gameModel, turnService);
        }

        public StartPlayerTurnCommand CreateStartPlayerTurnCommand()
        {
            return new StartPlayerTurnCommand(gameModel, turnService);
        }
    }
}