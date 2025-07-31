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
                var allPlayerTokensCount = turnService.GetCurrentPlayerModel().Tokens.TotalCount;
                await AsyncEventBus.Instance.PublishAndWaitAsync(new ReturnTokenWindowOpenedEvent(turnService.GetCurrentPlayerModel().Tokens.GetAllResources(), allPlayerTokensCount));
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
            if (!turnService.CanReserveCard())
            {
                return false;
            }

            if (!turnService.CanConfirmReserveMusicCard())
            {
                return false;
            }

            var slot = boardService.GetSlotWithCard(cardId);

            if (slot == null)
            {
                return false;
            }

            turnService.ReserveCard(cardId);


            var inspirationTokens = boardService.GetBoardTokenCount(ResourceType.Inspiration);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new CardReservedEvent(cardId, inspirationTokens));

            boardService.RefillSlot(slot.Level, slot.Position);

            var musicCardData = slot.GetMusicCard();
            var putCardOnBoardEvent = new PutCardOnBoardEvent(slot.Level, slot.Position, cardId, musicCardData);
            await AsyncEventBus.Instance.PublishAndWaitAsync(putCardOnBoardEvent);

            return true;
        }
    }

    // Card purchase action
    public class AddTokenToCardPurchaseCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AddTokenToCardPurchase";
        private readonly ResourceType token;
        private readonly TurnService turnService;

        public AddTokenToCardPurchaseCommand(ResourceType token, TurnService turnService) : base()
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
            if (!turnService.CanAddTokenToCardPurchase(token))
            {
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

        public PurchaseCardCommand(string cardId, TurnService turnService, BoardService boardService) : base()
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
            var selectedTokens = turnService.GetCardPurchaseTokens();

            if (!turnService.CanPurchaseCardWithResources(cardId, selectedTokens))
            {
                UnityEngine.Debug.LogError("Cannot purchase card");
                return false;
            }

            var slot = boardService.GetSlotWithCard(cardId);
            if (slot == null)
            {
                return false;
            }

            

            turnService.PurchaseCard(cardId, selectedTokens);
            await AsyncEventBus.Instance.PublishAndWaitAsync(new CardPurchasedEvent(cardId, boardService.GetAllBoardResources()));

            boardService.RefillSlot(slot.Level, slot.Position);
            var putCardOnBoardEvent = new PutCardOnBoardEvent(slot.Level, slot.Position, cardId, slot.GetMusicCard());
            await AsyncEventBus.Instance.PublishAndWaitAsync(putCardOnBoardEvent);

            UnityEngine.Debug.LogError("Card purchased");
            return true;
        }
    }
}