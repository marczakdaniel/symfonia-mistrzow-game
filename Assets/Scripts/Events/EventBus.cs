using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// Base interface for all game events
    /// </summary>
    public interface IGameEvent
    {
        DateTime Timestamp { get; }
        string EventId { get; }
    }

    /// <summary>
    /// Base class for all game events
    /// </summary>
    public abstract class GameEvent : IGameEvent
    {
        public DateTime Timestamp { get; }
        public string EventId { get; }

        protected GameEvent()
        {
            Timestamp = DateTime.UtcNow;
            EventId = Guid.NewGuid().ToString();
        }
    }
    /// <summary>
    /// Thread-safe EventBus for game events
    /// </summary>
    public class EventBus
    {
        private static EventBus _instance;
        private readonly Dictionary<Type, List<IEventSubscription>> _subscriptions = new();
        private readonly object _lock = new object();

        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventBus();
                }
                return _instance;
            }
        }

        public static void Initialize()
        {
            _instance = new EventBus();
        }

        private EventBus() { }

        /// <summary>
        /// Subscribe to an event type
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="handler">Event handler</param>
        /// <returns>Subscription token for unsubscribing</returns>
        public IEventSubscription Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            if (handler == null)
            {
                Debug.LogWarning("[EventBus] Null handler provided for subscription");
                return null;
            }

            lock (_lock)
            {
                var eventType = typeof(T);
                var subscription = new EventSubscription<T>(handler, this);

                if (!_subscriptions.ContainsKey(eventType))
                {
                    _subscriptions[eventType] = new List<IEventSubscription>();
                }

                _subscriptions[eventType].Add(subscription);
                
                Debug.Log($"[EventBus] Subscribed to {eventType.Name}. Total subscribers: {_subscriptions[eventType].Count}");
                return subscription;
            }
        }

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="eventData">Event data</param>
        public void Publish<T>(T eventData) where T : IGameEvent
        {
            if (eventData == null)
            {
                Debug.LogWarning("[EventBus] Null event data provided for publishing");
                return;
            }

            List<IEventSubscription> subscribers;
            lock (_lock)
            {
                var eventType = typeof(T);
                if (!_subscriptions.ContainsKey(eventType))
                {
                    Debug.Log($"[EventBus] No subscribers for {eventType.Name}");
                    return;
                }

                // Copy list to avoid modification during iteration
                subscribers = new List<IEventSubscription>(_subscriptions[eventType]);
            }

            Debug.Log($"[EventBus] Publishing {typeof(T).Name} to {subscribers.Count} subscribers");

            // Invoke handlers outside of lock to avoid deadlock
            foreach (var subscription in subscribers)
            {
                try
                {
                    if (subscription.IsValid)
                    {
                        subscription.Invoke(eventData);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[EventBus] Error invoking handler for {typeof(T).Name}: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <param name="subscription">Subscription to remove</param>
        public void Unsubscribe(IEventSubscription subscription)
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
                    
                    Debug.Log($"[EventBus] Unsubscribed from {eventType.Name}");
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
                Debug.Log("[EventBus] All subscriptions cleared");
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
    /// Interface for event subscriptions
    /// </summary>
    public interface IEventSubscription
    {
        Type EventType { get; }
        bool IsValid { get; }
        void Invoke(object eventData);
        void Unsubscribe();
    }

    /// <summary>
    /// Generic event subscription
    /// </summary>
    /// <typeparam name="T">Event type</typeparam>
    public class EventSubscription<T> : IEventSubscription where T : IGameEvent
    {
        private readonly Action<T> _handler;
        private readonly EventBus _eventBus;
        private bool _isValid = true;

        public Type EventType => typeof(T);
        public bool IsValid => _isValid;

        public EventSubscription(Action<T> handler, EventBus eventBus)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void Invoke(object eventData)
        {
            if (!_isValid) return;

            if (eventData is T typedEvent)
            {
                _handler.Invoke(typedEvent);
            }
            else
            {
                Debug.LogWarning($"[EventSubscription] Invalid event data type for {typeof(T).Name}");
            }
        }

        public void Unsubscribe()
        {
            if (!_isValid) return;

            _isValid = false;
            _eventBus.Unsubscribe(this);
        }
    }

    /// <summary>
    /// Helper class for managing multiple subscriptions
    /// </summary>
    public class EventSubscriptionManager
    {
        private readonly List<IEventSubscription> _subscriptions = new();

        /// <summary>
        /// Subscribe to an event and manage subscription
        /// </summary>
        public void Subscribe<T>(Action<T> handler) where T : IGameEvent
        {
            var subscription = EventBus.Instance.Subscribe(handler);
            if (subscription != null)
            {
                _subscriptions.Add(subscription);
            }
        }

        /// <summary>
        /// Unsubscribe from all managed subscriptions
        /// </summary>
        public void UnsubscribeAll()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Unsubscribe();
            }
            _subscriptions.Clear();
        }

        /// <summary>
        /// Get number of managed subscriptions
        /// </summary>
        public int SubscriptionCount => _subscriptions.Count;
    }

    /// <summary>
    /// Static helper for easy event publishing
    /// </summary>
    public static class GameEventPublisher
    {
        /// <summary>
        /// Publish a game event
        /// </summary>
        public static void Publish<T>(T eventData) where T : IGameEvent
        {
            EventBus.Instance.Publish(eventData);
        }

        /// <summary>
        /// Publish multiple events
        /// </summary>
        public static void PublishMultiple(params IGameEvent[] events)
        {
            foreach (var eventData in events)
            {
                var eventType = eventData.GetType();
                var method = typeof(EventBus).GetMethod("Publish");
                var genericMethod = method.MakeGenericMethod(eventType);
                genericMethod.Invoke(EventBus.Instance, new object[] { eventData });
            }
        }
    }

    /// <summary>
    /// Extension methods for easier event handling
    /// </summary>
    public static class EventBusExtensions
    {
        /// <summary>
        /// Subscribe to event with automatic unsubscription on MonoBehaviour destruction
        /// </summary>
        public static IEventSubscription SubscribeWithLifetime<T>(this MonoBehaviour monoBehaviour, Action<T> handler) 
            where T : IGameEvent
        {
            var subscription = EventBus.Instance.Subscribe(handler);
            
            // Automatically unsubscribe when MonoBehaviour is destroyed
            monoBehaviour.StartCoroutine(UnsubscribeOnDestroy(subscription, monoBehaviour));
            
            return subscription;
        }

        private static System.Collections.IEnumerator UnsubscribeOnDestroy(IEventSubscription subscription, MonoBehaviour monoBehaviour)
        {
            yield return new WaitUntil(() => monoBehaviour == null);
            subscription?.Unsubscribe();
        }
    }
}