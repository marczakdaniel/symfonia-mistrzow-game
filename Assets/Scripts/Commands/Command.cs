using Models;
using DefaultNamespace.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Events;
using Services;
using Cysharp.Threading.Tasks;

namespace Command
{
    /*
    public class BuyMusicCardCommand : BasePlayerActionCommand
    {
        public override string CommandType => "BuyMusicCard";
        public string MusicCardId { get; private set; }

        private readonly GameModel gameModel;

        public BuyMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId, gameModel)
        {
            this.MusicCardId = musicCardId;
            this.gameModel = gameModel;
        }

        public override async UniTask<bool> Validate()
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
            // Change model
            var result = gameModel.PurchaseCard(PlayerId, MusicCardId, new ResourceCollectionModel());

            if (!result)
            {
                return false;
            }

            // Publish event and wait for UI to complete
            var cardPurchasedEvent = new CardPurchasedEvent(PlayerId, MusicCardId, new ResourceCollectionModel());
            await AsyncEventBus.Instance.PublishAndWaitAsync(cardPurchasedEvent);

            // Optionally publish board update event
            var boardUpdatedEvent = new BoardUpdatedEvent(gameModel.board);
            await AsyncEventBus.Instance.PublishAndWaitAsync(boardUpdatedEvent);

            return true;
        }
    }

    public class ReserveMusicCardCommand : BasePlayerActionCommand
    {
        public override string CommandType => "ReserveMusicCard";
        public string MusicCardId { get; private set; }
        private readonly GameModel gameModel;

        public ReserveMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId, gameModel)
        {
            MusicCardId = musicCardId;
            this.gameModel = gameModel;
        }

        public override async UniTask<bool> Validate()
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

            // Publish event and wait for UI to complete
            var cardReservedEvent = new CardReservedEvent(PlayerId, MusicCardId, 1);
            await AsyncEventBus.Instance.PublishAndWaitAsync(cardReservedEvent);

            return true;
        }
    }
    */

    public class StartGameCommand : BaseGameFlowCommand
    {
        public override string CommandType => "StartGame";

        private readonly GameModel gameModel;
        private readonly TurnService turnService;

        public StartGameCommand(GameModel gameModel, TurnService turnService) : base(gameModel)
        {
            this.gameModel = gameModel;
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            // TODO: Check if game can be started
            return gameModel.CanStartGame();
        }

        public override async UniTask<bool> Execute()
        {
            // model update
            // business logic
            // event publish
            // 1. Model update - preparation
            gameModel.StartGame();
            
            // 2. Event publish and wait for UI to complete
            var gameStartedEvent = new GameStartedEvent();
            await AsyncEventBus.Instance.PublishAndWaitAsync(gameStartedEvent);

            turnService.NextPlayerTurn();

            var currentPlayer = gameModel.GetPlayer(gameModel.GetTurnModel().CurrentPlayerId);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartTurnWindowOpenedEvent(currentPlayer.PlayerId, currentPlayer.PlayerName));

            return true;
        }
    }
    
    // Turn commands
    public class StartPlayerTurnCommand : BasePlayerActionCommand
    {
        public override string CommandType => "StartPlayerTurn";
        private readonly TurnService turnService;

        public StartPlayerTurnCommand(GameModel gameModel, TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            turnService.StartPlayerTurn();
            var turnStartedEvent = new TurnStartedEvent(turnService.GetCurrentPlayerId());
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
            if (turnService.IsTokenReturnNeeded())
            {
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ReturnTokenWindowOpenedEvent(turnService.GetCurrentPlayerModel().Tokens.GetAllResources()));
                return true;
            }

            turnService.EndPlayerTurn();
            turnService.NextPlayerTurn();

            var currentPlayer = turnService.GetCurrentPlayerModel();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new StartTurnWindowOpenedEvent(currentPlayer.PlayerId, currentPlayer.PlayerName));

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
                turnService.ClearSelectedTokens();
                await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenDetailsPanelClosedEvent());
                return true;
            }

            turnService.ConfirmSelectedTokens();
            turnService.ClearSelectedTokens();
            var boardTokens = boardService.GetAllBoardResources();
            await AsyncEventBus.Instance.PublishAndWaitAsync(new SelectedTokensConfirmedEvent(boardTokens));
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
            return true;
        }   
    }

    public class ConfirmReturnTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "ConfirmReturnTokens";
        private readonly TurnService turnService;

        public ConfirmReturnTokensCommand(TurnService turnService) : base()
        {
            this.turnService = turnService;
        }

        public override async UniTask<bool> Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            return true;
        }   
    }
}