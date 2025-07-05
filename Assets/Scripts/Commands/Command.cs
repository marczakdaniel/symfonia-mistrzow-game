using Models;
using Services;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Command
{
    // Event interfaces and base classes
    public interface IGameEvent
    {
        string EventId { get; }
        DateTime Timestamp { get; }
    }

    public abstract class GameEvent : IGameEvent
    {
        public string EventId { get; private set; }
        public DateTime Timestamp { get; private set; }

        protected GameEvent()
        {
            EventId = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }
    }

    // Async Event Bus
    public class AsyncEventBus
    {
        private static AsyncEventBus _instance;
        public static AsyncEventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AsyncEventBus();
                }
                return _instance;
            }
        }

        private readonly Dictionary<Type, List<IAsyncEventHandler>> _handlers = new();
        private readonly Dictionary<string, List<UniTaskCompletionSource>> _eventCompletionSources = new();

        private AsyncEventBus() { }

        public void Subscribe<T>(IAsyncEventHandler<T> handler) where T : IGameEvent
        {
            var eventType = typeof(T);
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<IAsyncEventHandler>();
            }
            _handlers[eventType].Add(handler);
        }

        public void Unsubscribe<T>(IAsyncEventHandler<T> handler) where T : IGameEvent
        {
            var eventType = typeof(T);
            if (_handlers.ContainsKey(eventType))
            {
                _handlers[eventType].Remove(handler);
                if (_handlers[eventType].Count == 0)
                {
                    _handlers.Remove(eventType);
                }
            }
        }

        public async UniTask PublishAsync<T>(T gameEvent) where T : IGameEvent
        {
            var eventType = typeof(T);
            
            Debug.Log($"[AsyncEventBus] Publishing event: {eventType.Name} (ID: {gameEvent.EventId})");

            if (!_handlers.ContainsKey(eventType))
            {
                Debug.Log($"[AsyncEventBus] No handlers registered for event: {eventType.Name}");
                return;
            }

            var handlers = _handlers[eventType];
            var tasks = new List<UniTask>();

            foreach (var handler in handlers)
            {
                if (handler is IAsyncEventHandler<T> typedHandler)
                {
                    tasks.Add(typedHandler.HandleAsync(gameEvent));
                }
            }

            if (tasks.Count > 0)
            {
                await UniTask.WhenAll(tasks);
                Debug.Log($"[AsyncEventBus] All handlers completed for event: {eventType.Name}");
            }
        }

        public async UniTask PublishAndWaitAsync<T>(T gameEvent, CancellationToken cancellationToken = default) where T : IGameEvent
        {
            var completionSource = new UniTaskCompletionSource();
            
            if (!_eventCompletionSources.ContainsKey(gameEvent.EventId))
            {
                _eventCompletionSources[gameEvent.EventId] = new List<UniTaskCompletionSource>();
            }
            _eventCompletionSources[gameEvent.EventId].Add(completionSource);

            // Publish the event
            await PublishAsync(gameEvent);

            // Wait for all UI updates to complete
            await completionSource.Task;
        }

        public void CompleteUIUpdate(string eventId)
        {
            if (_eventCompletionSources.ContainsKey(eventId))
            {
                var completionSources = _eventCompletionSources[eventId];
                foreach (var source in completionSources)
                {
                    source.TrySetResult();
                }
                _eventCompletionSources.Remove(eventId);
            }
        }

        public void Clear()
        {
            _handlers.Clear();
            _eventCompletionSources.Clear();
        }
    }

    public interface IAsyncEventHandler
    {
    }

    public interface IAsyncEventHandler<in T> : IAsyncEventHandler where T : IGameEvent
    {
        UniTask HandleAsync(T gameEvent);
    }

    // Game Events
    public class GameStartedEvent : GameEvent
    {
        public GameModel GameModel { get; }
        
        public GameStartedEvent(GameModel gameModel)
        {
            GameModel = gameModel;
        }
    }

    public class CardPurchasedEvent : GameEvent
    {
        public string PlayerId { get; }
        public string CardId { get; }
        public ResourceCollectionModel TokensUsed { get; }
        
        public CardPurchasedEvent(string playerId, string cardId, ResourceCollectionModel tokensUsed)
        {
            PlayerId = playerId;
            CardId = cardId;
            TokensUsed = tokensUsed;
        }
    }

    public class CardReservedEvent : GameEvent
    {
        public string PlayerId { get; }
        public string CardId { get; }
        public int FromLevel { get; }
        public bool FromDeck { get; }
        
        public CardReservedEvent(string playerId, string cardId, int fromLevel, bool fromDeck = false)
        {
            PlayerId = playerId;
            CardId = cardId;
            FromLevel = fromLevel;
            FromDeck = fromDeck;
        }
    }

    public class BoardUpdatedEvent : GameEvent
    {
        public BoardModel Board { get; }
        
        public BoardUpdatedEvent(BoardModel board)
        {
            Board = board;
        }
    }

    public class PlayerTurnStartedEvent : GameEvent
    {
        public string PlayerId { get; }
        
        public PlayerTurnStartedEvent(string playerId)
        {
            PlayerId = playerId;
        }
    }

    public class PlayerTurnEndedEvent : GameEvent
    {
        public string PlayerId { get; }
        
        public PlayerTurnEndedEvent(string playerId)
        {
            PlayerId = playerId;
        }
    }

    public class UIUpdateCompletedEvent : GameEvent
    {
        public string OriginalEventId { get; }
        public string ComponentName { get; }
        
        public UIUpdateCompletedEvent(string originalEventId, string componentName)
        {
            OriginalEventId = originalEventId;
            ComponentName = componentName;
        }
    }
    public class BoardMusicCardClickedCommand : BaseCommand
    {
        public override string CommandType => "BoardMusicCardClicked";

        public BoardMusicCardClickedCommand() : base()
        {
            
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            await UniTask.Delay(1000);
            return true;
        }
    }

    public class BuyMusicCardCommand : BasePlayerActionCommand
    {
        public override string CommandType => "BuyMusicCard";
        public string MusicCardId { get; private set; }

        private readonly GameModel gameModel;

        public BuyMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId)
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

        public ReserveMusicCardCommand(string playerId, string musicCardId, GameModel gameModel) : base(playerId)
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

            // Publish board update event
            var boardUpdatedEvent = new BoardUpdatedEvent(gameModel.board);
            await AsyncEventBus.Instance.PublishAndWaitAsync(boardUpdatedEvent);

            return true;
        }
    }

    public class StartGameCommand : BaseGameFlowCommand
    {
        public override string CommandType => "StartGame";

        private readonly GameModel gameModel;

        public StartGameCommand(GameModel gameModel) : base()
        {
            this.gameModel = gameModel;
        }

        public override bool Validate()
        {
            return true;
        }

        public override async UniTask<bool> Execute()
        {
            // Initialize the game
            gameModel.InitializeBoard();
            
            // Publish game started event and wait for UI to complete
            var gameStartedEvent = new GameStartedEvent(gameModel);
            await AsyncEventBus.Instance.PublishAndWaitAsync(gameStartedEvent);

            return true;
        }
    }
}