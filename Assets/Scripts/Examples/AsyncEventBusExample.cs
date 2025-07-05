using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Events;

namespace Examples
{
    /// <summary>
    /// Example showing how to use AsyncEventBus with UniTask
    /// </summary>
    public class AsyncEventBusExample : MonoBehaviour
    {
        private AsyncEventSubscriptionManager _subscriptionManager;

        private void Start()
        {
            // Initialize subscription manager for easy cleanup
            _subscriptionManager = new AsyncEventSubscriptionManager();
            
            // Subscribe to various events
            SubscribeToEvents();
            
            // Example: Publish some events
            PublishExampleEvents().Forget();
        }

        private void SubscribeToEvents()
        {
            // Subscribe to MusicCardBoughtEvent
            _subscriptionManager.Subscribe<MusicCardBoughtEvent>(OnMusicCardBought);
            
            // Subscribe to PlayerTurnChangedEvent
            _subscriptionManager.Subscribe<PlayerTurnChangedEvent>(OnPlayerTurnChanged);
            
            // Subscribe to AnimationRequestEvent
            _subscriptionManager.Subscribe<AnimationRequestEvent>(OnAnimationRequest);
            
            // Subscribe using the extension method with automatic cleanup
            this.SubscribeAsyncWithLifetime<GameStateChangedEvent>(OnGameStateChanged);
        }

        private async UniTask OnMusicCardBought(MusicCardBoughtEvent eventData)
        {
            Debug.Log($"Player {eventData.PlayerId} bought card {eventData.MusicCardId}");
            
            // Simulate some async processing
            await UniTask.Delay(500);
            
            // Trigger UI update
            var uiUpdateEvent = new UIUpdateRequestEvent("PlayerResources", "Update", new Dictionary<string, object>
            {
                { "PlayerId", eventData.PlayerId },
                { "CostPaid", eventData.CostPaid }
            });
            
            await AsyncGameEventPublisher.PublishAsync(uiUpdateEvent);
            
            // Trigger animation
            var animationEvent = new AnimationRequestEvent("CardBought", eventData.MusicCardId);
            await AsyncGameEventPublisher.PublishAsync(animationEvent);
        }

        private async UniTask OnPlayerTurnChanged(PlayerTurnChangedEvent eventData)
        {
            Debug.Log($"Turn changed from {eventData.PreviousPlayerId} to {eventData.NewCurrentPlayerId}");
            
            // Simulate turn transition animation
            await UniTask.Delay(1000);
            
            // Update UI
            var uiUpdateEvent = new UIUpdateRequestEvent("CurrentPlayer", "TurnChanged", new Dictionary<string, object>
            {
                { "NewPlayerId", eventData.NewCurrentPlayerId },
                { "TurnNumber", eventData.TurnNumber }
            });
            
            await AsyncGameEventPublisher.PublishAsync(uiUpdateEvent);
        }

        private async UniTask OnAnimationRequest(AnimationRequestEvent eventData)
        {
            Debug.Log($"Playing animation: {eventData.AnimationType} on {eventData.TargetId}");
            
            // Simulate animation duration
            await UniTask.Delay(2000);
            
            Debug.Log($"Animation {eventData.AnimationType} completed");
        }

        private async UniTask OnGameStateChanged(GameStateChangedEvent eventData)
        {
            Debug.Log($"Game state changed from {eventData.PreviousState} to {eventData.NewState}");
            
            // Handle state transitions
            switch (eventData.NewState)
            {
                case "Playing":
                    await HandleGameStart();
                    break;
                case "Ended":
                    await HandleGameEnd();
                    break;
            }
        }

        private async UniTask HandleGameStart()
        {
            Debug.Log("Game started - initializing UI");
            await UniTask.Delay(1000);
        }

        private async UniTask HandleGameEnd()
        {
            Debug.Log("Game ended - showing results");
            await UniTask.Delay(2000);
        }

        private async UniTask PublishExampleEvents()
        {
            // Wait a bit before publishing
            await UniTask.Delay(2000);
            
            // Publish game state change
            var gameStateEvent = new GameStateChangedEvent("Playing", "Lobby");
            await AsyncGameEventPublisher.PublishAsync(gameStateEvent);
            
            await UniTask.Delay(1000);
            
            // Publish player turn change
            var turnEvent = new PlayerTurnChangedEvent("Player1", "Player2", 5);
            await AsyncGameEventPublisher.PublishAsync(turnEvent);
            
            await UniTask.Delay(1000);
            
            // Publish card bought event
            var cardBoughtEvent = new MusicCardBoughtEvent("Player1", "Card123", new Dictionary<string, int>
            {
                { "Blue", 2 },
                { "Red", 1 }
            });
            await AsyncGameEventPublisher.PublishAsync(cardBoughtEvent);
            
            // Example of publishing multiple events in parallel
            var events = new IGameEvent[]
            {
                new MusicCardAddedToBoardEvent("Card456", 0, 1),
                new PlayerResourcesChangedEvent("Player1", new Dictionary<string, int> { { "Blue", -2 } }, new Dictionary<string, int> { { "Blue", 3 } }),
                new UIUpdateRequestEvent("Board", "RefreshCards", null)
            };
            
            await AsyncGameEventPublisher.PublishMultipleParallelAsync(events);
        }

        private void OnDestroy()
        {
            // Clean up subscriptions
            _subscriptionManager?.UnsubscribeAll();
        }
    }

    /// <summary>
    /// Example async event handler as a separate class
    /// </summary>
    public class UIAnimationHandler : IAsyncEventHandler<AnimationRequestEvent>
    {
        public async UniTask HandleAsync(AnimationRequestEvent eventData)
        {
            Debug.Log($"UIAnimationHandler: Starting {eventData.AnimationType} animation");
            
            // Simulate complex animation logic
            await UniTask.Delay(1500);
            
            Debug.Log($"UIAnimationHandler: {eventData.AnimationType} animation completed");
        }
    }

    /// <summary>
    /// Example service that uses AsyncEventBus
    /// </summary>
    public class GameEventService
    {
        private readonly AsyncEventSubscriptionManager _subscriptionManager;
        
        public GameEventService()
        {
            _subscriptionManager = new AsyncEventSubscriptionManager();
            InitializeSubscriptions();
        }

        private void InitializeSubscriptions()
        {
            // Subscribe to all game events that this service needs to handle
            _subscriptionManager.Subscribe<MusicCardBoughtEvent>(HandleMusicCardBought);
            _subscriptionManager.Subscribe<MusicCardReservedEvent>(HandleMusicCardReserved);
            _subscriptionManager.Subscribe<PlayerTurnChangedEvent>(HandlePlayerTurnChanged);
            _subscriptionManager.Subscribe<GameEndedEvent>(HandleGameEnded);
        }

        private async UniTask HandleMusicCardBought(MusicCardBoughtEvent eventData)
        {
            Debug.Log($"[GameEventService] Processing card purchase: {eventData.MusicCardId}");
            
            // Update game statistics
            await UpdateGameStatistics(eventData.PlayerId, "card_bought");
            
            // Check for achievements
            await CheckAchievements(eventData.PlayerId);
            
            // Save game state
            await SaveGameState();
        }

        private async UniTask HandleMusicCardReserved(MusicCardReservedEvent eventData)
        {
            Debug.Log($"[GameEventService] Processing card reservation: {eventData.MusicCardId}");
            
            await UpdateGameStatistics(eventData.PlayerId, "card_reserved");
            
            if (eventData.ReceivedToken)
            {
                // Player received a token, update UI
                var uiEvent = new UIUpdateRequestEvent("PlayerTokens", "TokenAdded", new Dictionary<string, object>
                {
                    { "PlayerId", eventData.PlayerId }
                });
                await AsyncGameEventPublisher.PublishAsync(uiEvent);
            }
        }

        private async UniTask HandlePlayerTurnChanged(PlayerTurnChangedEvent eventData)
        {
            Debug.Log($"[GameEventService] Turn {eventData.TurnNumber}: {eventData.NewCurrentPlayerId}");
            
            // Auto-save every few turns
            if (eventData.TurnNumber % 3 == 0)
            {
                await SaveGameState();
            }
        }

        private async UniTask HandleGameEnded(GameEndedEvent eventData)
        {
            Debug.Log($"[GameEventService] Game ended. Winner: {eventData.WinnerId}");
            
            // Process final scores
            await ProcessFinalScores(eventData.FinalScores);
            
            // Update player statistics
            foreach (var playerId in eventData.PlayerRanking)
            {
                await UpdatePlayerRating(playerId, eventData.PlayerRanking.IndexOf(playerId));
            }
            
            // Clean up
            await CleanupGameSession();
        }

        private async UniTask UpdateGameStatistics(string playerId, string action)
        {
            // Simulate database update
            await UniTask.Delay(100);
            Debug.Log($"Statistics updated for {playerId}: {action}");
        }

        private async UniTask CheckAchievements(string playerId)
        {
            // Simulate achievement checking
            await UniTask.Delay(50);
            Debug.Log($"Achievements checked for {playerId}");
        }

        private async UniTask SaveGameState()
        {
            // Simulate saving game state
            await UniTask.Delay(200);
            Debug.Log("Game state saved");
        }

        private async UniTask ProcessFinalScores(Dictionary<string, int> finalScores)
        {
            await UniTask.Delay(300);
            Debug.Log($"Final scores processed for {finalScores.Count} players");
        }

        private async UniTask UpdatePlayerRating(string playerId, int position)
        {
            await UniTask.Delay(150);
            Debug.Log($"Player {playerId} rating updated (position: {position})");
        }

        private async UniTask CleanupGameSession()
        {
            await UniTask.Delay(100);
            Debug.Log("Game session cleaned up");
        }

        public void Dispose()
        {
            _subscriptionManager?.UnsubscribeAll();
        }
    }
} 