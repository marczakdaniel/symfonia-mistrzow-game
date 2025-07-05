using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Events
{
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

            // Complete any waiting operations
            CompleteWaitingOperations(gameEvent.EventId);
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

        private void CompleteWaitingOperations(string eventId)
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
} 