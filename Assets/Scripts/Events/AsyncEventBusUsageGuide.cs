using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Events;

namespace Events
{
    /// <summary>
    /// Usage guide and examples for AsyncEventBus
    /// </summary>
    public static class AsyncEventBusUsageGuide
    {
        /// <summary>
        /// Example 1: Simple event publishing
        /// </summary>
        public static async UniTask PublishSimpleEvent()
        {
            // Create and publish an event
            var gameStateEvent = new GameStateChangedEvent("Playing", "Lobby");
            await AsyncGameEventPublisher.PublishAsync(gameStateEvent);
        }

        /// <summary>
        /// Example 2: Publishing multiple events in parallel
        /// </summary>
        public static async UniTask PublishMultipleEventsParallel()
        {
            var events = new IGameEvent[]
            {
                new PlayerTurnChangedEvent("Player1", "Player2", 1),
                new MusicCardAddedToBoardEvent("Card123", 0, 1),
                new UIUpdateRequestEvent("GameStatus", "Refresh", null)
            };

            await AsyncGameEventPublisher.PublishMultipleParallelAsync(events);
        }

        /// <summary>
        /// Example 3: Publishing multiple events sequentially
        /// </summary>
        public static async UniTask PublishMultipleEventsSequential()
        {
            var events = new IGameEvent[]
            {
                new PlayerTurnChangedEvent("Player1", "Player2", 1),
                new MusicCardAddedToBoardEvent("Card123", 0, 1),
                new UIUpdateRequestEvent("GameStatus", "Refresh", null)
            };

            await AsyncGameEventPublisher.PublishMultipleAsync(events);
        }

        /// <summary>
        /// Example 4: Fire-and-forget event publishing
        /// </summary>
        public static void PublishAndForget()
        {
            var animationEvent = new AnimationRequestEvent("CardFlip", "Card123");
            AsyncGameEventPublisher.PublishAndForgetAsync(animationEvent).Forget();
        }

        /// <summary>
        /// Example 5: Subscribing to events with lambda
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithLambda()
        {
            return AsyncEventBus.Instance.Subscribe<MusicCardBoughtEvent>(async (eventData) =>
            {
                Debug.Log($"Card bought: {eventData.MusicCardId} by {eventData.PlayerId}");
                await UniTask.Delay(1000); // Simulate processing
            });
        }

        /// <summary>
        /// Example 6: Subscribing to events with method reference
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithMethodReference()
        {
            return AsyncEventBus.Instance.Subscribe<PlayerTurnChangedEvent>(HandlePlayerTurnChanged);
        }

        private static async UniTask HandlePlayerTurnChanged(PlayerTurnChangedEvent eventData)
        {
            Debug.Log($"Turn changed to: {eventData.NewCurrentPlayerId}");
            await UniTask.Delay(500);
        }

        /// <summary>
        /// Example 7: Using AsyncEventSubscriptionManager for easy cleanup
        /// </summary>
        public static AsyncEventSubscriptionManager CreateManagedSubscriptions()
        {
            var manager = new AsyncEventSubscriptionManager();
            
            // Subscribe to multiple events
            manager.Subscribe<MusicCardBoughtEvent>(OnCardBought);
            manager.Subscribe<PlayerTurnChangedEvent>(OnTurnChanged);
            manager.Subscribe<GameEndedEvent>(OnGameEnded);
            
            return manager; // Remember to call UnsubscribeAll() when done
        }

        private static async UniTask OnCardBought(MusicCardBoughtEvent eventData)
        {
            Debug.Log($"Processing card purchase: {eventData.MusicCardId}");
            await UniTask.Delay(200);
        }

        private static async UniTask OnTurnChanged(PlayerTurnChangedEvent eventData)
        {
            Debug.Log($"Processing turn change: {eventData.TurnNumber}");
            await UniTask.Delay(300);
        }

        private static async UniTask OnGameEnded(GameEndedEvent eventData)
        {
            Debug.Log($"Game ended. Winner: {eventData.WinnerId}");
            await UniTask.Delay(1000);
        }

        /// <summary>
        /// Example 8: Using event handler classes
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithHandlerClass()
        {
            var handler = new CustomEventHandler();
            return AsyncEventBus.Instance.Subscribe<AnimationRequestEvent>(handler);
        }

        private class CustomEventHandler : IAsyncEventHandler<AnimationRequestEvent>
        {
            public async UniTask HandleAsync(AnimationRequestEvent eventData)
            {
                Debug.Log($"Custom handler processing: {eventData.AnimationType}");
                await UniTask.Delay(800);
            }
        }

        /// <summary>
        /// Example 9: Error handling in event handlers
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithErrorHandling()
        {
            return AsyncEventBus.Instance.Subscribe<MusicCardBoughtEvent>(async (eventData) =>
            {
                try
                {
                    Debug.Log($"Processing card purchase: {eventData.MusicCardId}");
                    
                    // Simulate potential error
                    if (eventData.MusicCardId == "ErrorCard")
                    {
                        throw new InvalidOperationException("Cannot process ErrorCard");
                    }
                    
                    await UniTask.Delay(500);
                    Debug.Log("Card purchase processed successfully");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error processing card purchase: {ex.Message}");
                    // AsyncEventBus will catch and log this automatically
                }
            });
        }

        /// <summary>
        /// Example 10: Conditional event processing
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithCondition()
        {
            return AsyncEventBus.Instance.Subscribe<PlayerTurnChangedEvent>(async (eventData) =>
            {
                // Only process for specific players
                if (eventData.NewCurrentPlayerId == "Player1")
                {
                    Debug.Log("Processing turn for Player1");
                    await UniTask.Delay(1000);
                }
            });
        }

        /// <summary>
        /// Example 11: Chaining events (publishing new events in response to others)
        /// </summary>
        public static IAsyncEventSubscription SubscribeWithEventChaining()
        {
            return AsyncEventBus.Instance.Subscribe<MusicCardBoughtEvent>(async (eventData) =>
            {
                Debug.Log($"Card bought: {eventData.MusicCardId}");
                
                // Chain with animation request
                var animationEvent = new AnimationRequestEvent("CardBought", eventData.MusicCardId);
                await AsyncGameEventPublisher.PublishAsync(animationEvent);
                
                // Chain with UI update
                var uiEvent = new UIUpdateRequestEvent("PlayerHand", "AddCard", new Dictionary<string, object>
                {
                    { "CardId", eventData.MusicCardId },
                    { "PlayerId", eventData.PlayerId }
                });
                await AsyncGameEventPublisher.PublishAsync(uiEvent);
            });
        }

        /// <summary>
        /// Example 12: Getting information about subscriptions
        /// </summary>
        public static void CheckSubscriptionInfo()
        {
            var eventBus = AsyncEventBus.Instance;
            
            // Check subscriber count for specific event
            var subscriberCount = eventBus.GetSubscriberCount<MusicCardBoughtEvent>();
            Debug.Log($"MusicCardBoughtEvent has {subscriberCount} subscribers");
            
            // Get all registered event types
            var registeredTypes = eventBus.GetRegisteredEventTypes();
            Debug.Log($"Registered event types: {string.Join(", ", registeredTypes.Select(t => t.Name))}");
        }

        /// <summary>
        /// Example 13: Manual subscription management
        /// </summary>
        public static void ManualSubscriptionManagement()
        {
            // Subscribe
            var subscription = AsyncEventBus.Instance.Subscribe<GameEndedEvent>(async (eventData) =>
            {
                Debug.Log($"Game ended manually handled: {eventData.WinnerId}");
                await UniTask.Delay(1000);
            });
            
            // Later, unsubscribe manually
            subscription.Unsubscribe();
        }

        /// <summary>
        /// Example 14: Using extension methods for MonoBehaviour
        /// </summary>
        public static void UseExtensionMethods(MonoBehaviour monoBehaviour)
        {
            // This will automatically unsubscribe when the MonoBehaviour is destroyed
            monoBehaviour.SubscribeAsyncWithLifetime<MusicCardBoughtEvent>(async (eventData) =>
            {
                Debug.Log($"MonoBehaviour handling card purchase: {eventData.MusicCardId}");
                await UniTask.Delay(500);
            });
        }
    }
}

/*
USAGE PATTERNS:

1. SIMPLE PUBLISHING:
   await AsyncGameEventPublisher.PublishAsync(new GameStateChangedEvent("Playing", "Lobby"));

2. FIRE-AND-FORGET:
   AsyncGameEventPublisher.PublishAndForgetAsync(new AnimationRequestEvent("CardFlip", "Card123")).Forget();

3. MULTIPLE EVENTS (PARALLEL):
   await AsyncGameEventPublisher.PublishMultipleParallelAsync(events);

4. MULTIPLE EVENTS (SEQUENTIAL):
   await AsyncGameEventPublisher.PublishMultipleAsync(events);

5. SUBSCRIBING WITH LAMBDA:
   var subscription = AsyncEventBus.Instance.Subscribe<MusicCardBoughtEvent>(async (e) => {  handle  });

6. SUBSCRIBING WITH METHOD:
   var subscription = AsyncEventBus.Instance.Subscribe<MusicCardBoughtEvent>(HandleCardBought);

7. USING SUBSCRIPTION MANAGER:
   var manager = new AsyncEventSubscriptionManager();
   manager.Subscribe<MusicCardBoughtEvent>(HandleCardBought);
   // Later: manager.UnsubscribeAll();

8. USING HANDLER CLASSES:
   var handler = new MyEventHandler();
   var subscription = AsyncEventBus.Instance.Subscribe<SomeEvent>(handler);

9. MONOBEHAVIOUR INTEGRATION:
   this.SubscribeAsyncWithLifetime<MusicCardBoughtEvent>(HandleCardBought);

10. CHECKING SUBSCRIPTION INFO:
    var count = AsyncEventBus.Instance.GetSubscriberCount<MusicCardBoughtEvent>();
    var types = AsyncEventBus.Instance.GetRegisteredEventTypes();

BEST PRACTICES:
- Use AsyncEventSubscriptionManager for easy cleanup
- Use extension methods for MonoBehaviour integration
- Handle errors in your event handlers
- Consider using fire-and-forget for non-critical events
- Use parallel publishing when event order doesn't matter
- Always clean up subscriptions to prevent memory leaks
*/ 