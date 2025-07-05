using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Interface for async event handlers
    /// </summary>
    public interface IAsyncEventHandler<in T> where T : IGameEvent
    {
        UniTask HandleAsync(T eventData);
    }

    /// <summary>
    /// Async event subscription for UniTask-based handlers
    /// </summary>
    public interface IAsyncEventSubscription
    {
        Type EventType { get; }
        bool IsValid { get; }
        UniTask InvokeAsync(object eventData);
        void Unsubscribe();
    }

    /// <summary>
    /// Async event subscription implementation
    /// </summary>
    public class AsyncEventSubscription<T> : IAsyncEventSubscription where T : IGameEvent
    {
        private readonly Func<T, UniTask> _handler;
        private readonly AsyncEventBus _eventBus;
        private bool _isValid = true;

        public Type EventType => typeof(T);
        public bool IsValid => _isValid;

        public AsyncEventSubscription(Func<T, UniTask> handler, AsyncEventBus eventBus)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async UniTask InvokeAsync(object eventData)
        {
            if (!_isValid) return;

            if (eventData is T typedEvent)
            {
                await _handler(typedEvent);
            }
            else
            {
                Debug.LogError($"[AsyncEventBus] Invalid event type. Expected {typeof(T).Name}, got {eventData?.GetType().Name}");
            }
        }

        public void Unsubscribe()
        {
            if (_isValid)
            {
                _isValid = false;
                _eventBus.Unsubscribe(this);
            }
        }
    }

    /// <summary>
    /// Thread-safe Async EventBus for game events using UniTask
    /// </summary>
    public class AsyncEventBus
    {
        private static AsyncEventBus _instance;
        private readonly Dictionary<Type, List<IAsyncEventSubscription>> _subscriptions = new();
        private readonly object _lock = new object();

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

        public static void Initialize()
        {
            _instance = new AsyncEventBus();
        }

        private AsyncEventBus() { }

        /// <summary>
        /// Subscribe to an event type with async handler
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="handler">Async event handler</param>
        /// <returns>Subscription token for unsubscribing</returns>
        public IAsyncEventSubscription Subscribe<T>(Func<T, UniTask> handler) where T : IGameEvent
        {
            if (handler == null)
            {
                Debug.LogWarning("[AsyncEventBus] Null handler provided for subscription");
                return null;
            }

            lock (_lock)
            {
                var eventType = typeof(T);
                var subscription = new AsyncEventSubscription<T>(handler, this);

                if (!_subscriptions.ContainsKey(eventType))
                {
                    _subscriptions[eventType] = new List<IAsyncEventSubscription>();
                }

                _subscriptions[eventType].Add(subscription);
                
                Debug.Log($"[AsyncEventBus] Subscribed to {eventType.Name}. Total subscribers: {_subscriptions[eventType].Count}");
                return subscription;
            }
        }

        /// <summary>
        /// Subscribe to an event type with async handler class
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="handler">Async event handler class</param>
        /// <returns>Subscription token for unsubscribing</returns>
        public IAsyncEventSubscription Subscribe<T>(IAsyncEventHandler<T> handler) where T : IGameEvent
        {
            if (handler == null)
            {
                Debug.LogWarning("[AsyncEventBus] Null handler provided for subscription");
                return null;
            }

            return Subscribe<T>(handler.HandleAsync);
        }

        /// <summary>
        /// Publish an event to all subscribers asynchronously
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data</param>
        /// <returns>UniTask that completes when all handlers finish</returns>
        public async UniTask PublishAsync<T>(T eventData) where T : IGameEvent
        {
            if (eventData == null)
            {
                Debug.LogWarning("[AsyncEventBus] Null event data provided for publishing");
                return;
            }

            List<IAsyncEventSubscription> subscribers;
            lock (_lock)
            {
                var eventType = typeof(T);
                if (!_subscriptions.ContainsKey(eventType))
                {
                    Debug.Log($"[AsyncEventBus] No subscribers for {eventType.Name}");
                    return;
                }

                // Copy list to avoid modification during iteration
                subscribers = new List<IAsyncEventSubscription>(_subscriptions[eventType]);
            }

            Debug.Log($"[AsyncEventBus] Publishing {typeof(T).Name} to {subscribers.Count} subscribers");

            // Create list of tasks to run in parallel
            var tasks = new List<UniTask>();
            
            foreach (var subscription in subscribers)
            {
                if (subscription.IsValid)
                {
                    // Wrap each handler in try-catch to prevent one failure from stopping others
                    tasks.Add(SafeInvokeAsync(subscription, eventData, typeof(T).Name));
                }
            }

            // Wait for all handlers to complete
            if (tasks.Count > 0)
            {
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Publish an event and wait for all handlers to complete, but continue execution without blocking
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data</param>
        /// <returns>UniTask that can be fire-and-forget</returns>
        public UniTask PublishAndForgetAsync<T>(T eventData) where T : IGameEvent
        {
            return PublishAsync(eventData).SuppressCancellationThrow();
        }

        /// <summary>
        /// Publish multiple events in sequence
        /// </summary>
        /// <param name="events">Events to publish</param>
        /// <returns>UniTask that completes when all events are published</returns>
        public async UniTask PublishMultipleAsync(params IGameEvent[] events)
        {
            if (events == null || events.Length == 0) return;

            foreach (var eventData in events)
            {
                if (eventData != null)
                {
                    await PublishAsyncInternal(eventData);
                }
            }
        }

        /// <summary>
        /// Publish multiple events in parallel
        /// </summary>
        /// <param name="events">Events to publish</param>
        /// <returns>UniTask that completes when all events are published</returns>
        public async UniTask PublishMultipleParallelAsync(params IGameEvent[] events)
        {
            if (events == null || events.Length == 0) return;

            var tasks = events
                .Where(e => e != null)
                .Select(PublishAsyncInternal)
                .ToArray();

            if (tasks.Length > 0)
            {
                await UniTask.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Safely invoke handler with error handling
        /// </summary>
        private async UniTask SafeInvokeAsync(IAsyncEventSubscription subscription, object eventData, string eventTypeName)
        {
            try
            {
                await subscription.InvokeAsync(eventData);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AsyncEventBus] Error invoking handler for {eventTypeName}: {e.Message}");
            }
        }

        /// <summary>
        /// Internal method to publish event of unknown type
        /// </summary>
        private async UniTask PublishAsyncInternal(IGameEvent eventData)
        {
            var eventType = eventData.GetType();
            var method = typeof(AsyncEventBus).GetMethod(nameof(PublishAsync));
            var genericMethod = method.MakeGenericMethod(eventType);
            
            var task = (UniTask)genericMethod.Invoke(this, new object[] { eventData });
            await task;
        }

        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <param name="subscription">Subscription to remove</param>
        public void Unsubscribe(IAsyncEventSubscription subscription)
        {
            if (subscription == null) return;

            lock (_lock)
            {
                var eventType = subscription.EventType;
                if (_subscriptions.ContainsKey(eventType))
                {
                    _subscriptions[eventType].Remove(subscription);
                    
                    if (_subscriptions[eventType].Count == 0)
                    {
                        _subscriptions.Remove(eventType);
                    }
                    
                    Debug.Log($"[AsyncEventBus] Unsubscribed from {eventType.Name}");
                }
            }
        }

        /// <summary>
        /// Clear all subscriptions
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _subscriptions.Clear();
                Debug.Log("[AsyncEventBus] All subscriptions cleared");
            }
        }

        /// <summary>
        /// Get subscriber count for event type
        /// </summary>
        public int GetSubscriberCount<T>() where T : IGameEvent
        {
            lock (_lock)
            {
                var eventType = typeof(T);
                return _subscriptions.ContainsKey(eventType) ? _subscriptions[eventType].Count : 0;
            }
        }

        /// <summary>
        /// Get all registered event types
        /// </summary>
        public Type[] GetRegisteredEventTypes()
        {
            lock (_lock)
            {
                return _subscriptions.Keys.ToArray();
            }
        }
    }

    /// <summary>
    /// Async event subscription manager for easier lifecycle management
    /// </summary>
    public class AsyncEventSubscriptionManager
    {
        private readonly List<IAsyncEventSubscription> _subscriptions = new();

        /// <summary>
        /// Subscribe to an event and track the subscription
        /// </summary>
        public void Subscribe<T>(Func<T, UniTask> handler) where T : IGameEvent
        {
            var subscription = AsyncEventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
        }

        /// <summary>
        /// Subscribe to an event with handler class and track the subscription
        /// </summary>
        public void Subscribe<T>(IAsyncEventHandler<T> handler) where T : IGameEvent
        {
            var subscription = AsyncEventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
        }

        /// <summary>
        /// Unsubscribe from all tracked subscriptions
        /// </summary>
        public void UnsubscribeAll()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription?.Unsubscribe();
            }
            _subscriptions.Clear();
        }

        /// <summary>
        /// Get count of active subscriptions
        /// </summary>
        public int SubscriptionCount => _subscriptions.Count(s => s?.IsValid == true);
    }

    /// <summary>
    /// Static helper for publishing async events
    /// </summary>
    public static class AsyncGameEventPublisher
    {
        /// <summary>
        /// Publish an event asynchronously
        /// </summary>
        public static async UniTask PublishAsync<T>(T eventData) where T : IGameEvent
        {
            await AsyncEventBus.Instance.PublishAsync(eventData);
        }

        /// <summary>
        /// Publish an event and forget (fire-and-forget)
        /// </summary>
        public static UniTask PublishAndForgetAsync<T>(T eventData) where T : IGameEvent
        {
            return AsyncEventBus.Instance.PublishAndForgetAsync(eventData);
        }

        /// <summary>
        /// Publish multiple events sequentially
        /// </summary>
        public static async UniTask PublishMultipleAsync(params IGameEvent[] events)
        {
            await AsyncEventBus.Instance.PublishMultipleAsync(events);
        }

        /// <summary>
        /// Publish multiple events in parallel
        /// </summary>
        public static async UniTask PublishMultipleParallelAsync(params IGameEvent[] events)
        {
            await AsyncEventBus.Instance.PublishMultipleParallelAsync(events);
        }
    }

    /// <summary>
    /// Extension methods for MonoBehaviour to work with AsyncEventBus
    /// </summary>
    public static class AsyncEventBusExtensions
    {
        /// <summary>
        /// Subscribe to an event with automatic cleanup when MonoBehaviour is destroyed
        /// </summary>
        public static IAsyncEventSubscription SubscribeAsyncWithLifetime<T>(this MonoBehaviour monoBehaviour, Func<T, UniTask> handler) 
            where T : IGameEvent
        {
            var subscription = AsyncEventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                monoBehaviour.StartCoroutine(UnsubscribeOnDestroy(subscription, monoBehaviour));
            }
            return subscription;
        }

        private static System.Collections.IEnumerator UnsubscribeOnDestroy(IAsyncEventSubscription subscription, MonoBehaviour monoBehaviour)
        {
            yield return new WaitUntil(() => monoBehaviour == null);
            subscription?.Unsubscribe();
        }
    }
}