using Models;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Events;

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

        public StartGameCommand(GameModel gameModel) : base(gameModel)
        {
            this.gameModel = gameModel;
        }

        public override bool Validate()
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

            return true;
        }
    }

    // Select token action
    public class AddTokenToSelectedTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "AddTokenToSelectedTokens";
        private readonly ResourceType token;

        private readonly GameModel gameModel;

        public AddTokenToSelectedTokensCommand(ResourceType token, GameModel gameModel) : base("", gameModel)
        {
            this.token = token;
            this.gameModel = gameModel;
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenAddedToSelectedTokensEvent(token, 13, new ResourceType?[] { token, null, null }));

            return true;
        }
    }

    public class RemoveTokenFromSelectedTokensCommand : BasePlayerActionCommand
    {
        public override string CommandType => "RemoveTokenFromSelectedTokens";
        private readonly GameModel gameModel;
        private readonly ResourceType token;

        public RemoveTokenFromSelectedTokensCommand(ResourceType token, GameModel gameModel) : base("", gameModel)
        {
            this.gameModel = gameModel;
            this.token = token;
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await AsyncEventBus.Instance.PublishAndWaitAsync(new TokenRemovedFromSelectedTokensEvent(token, 1, new ResourceType?[] { token, null, null }));
            return true;
        }
    }
}