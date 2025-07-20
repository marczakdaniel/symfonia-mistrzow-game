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

    public enum EventPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
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

        // Pre-allocated lists for each priority to avoid allocations during publish
        private readonly Dictionary<Type, HandlerInfo[]> _handlersByType = new();
        private readonly Dictionary<string, List<UniTaskCompletionSource>> _eventCompletionSources = new();
        
        // Object pool for frequently allocated task lists
        private readonly Stack<List<UniTask>> _taskListPool = new();

        private AsyncEventBus() { }

        public void Subscribe<T>(IAsyncEventHandler<T> handler, EventPriority priority = EventPriority.Normal) where T : IGameEvent
        {
            var eventType = typeof(T);
            
            if (!_handlersByType.ContainsKey(eventType))
            {
                _handlersByType[eventType] = new HandlerInfo[0];
            }
            
            var currentHandlers = _handlersByType[eventType];
            var newHandlers = new HandlerInfo[currentHandlers.Length + 1];
            
            // Copy existing handlers
            Array.Copy(currentHandlers, newHandlers, currentHandlers.Length);
            
            // Add new handler
            newHandlers[currentHandlers.Length] = new HandlerInfo(handler, priority);
            
            // Sort by priority (highest first) - only when adding new handler
            Array.Sort(newHandlers, (a, b) => b.Priority.CompareTo(a.Priority));
            
            _handlersByType[eventType] = newHandlers;
        }

        public void Unsubscribe<T>(IAsyncEventHandler<T> handler) where T : IGameEvent
        {
            var eventType = typeof(T);
            if (_handlersByType.ContainsKey(eventType))
            {
                var currentHandlers = _handlersByType[eventType];
                var newHandlers = new List<HandlerInfo>();
                
                // Filter out the handler to remove
                for (int i = 0; i < currentHandlers.Length; i++)
                {
                    if (currentHandlers[i].Handler != handler)
                    {
                        newHandlers.Add(currentHandlers[i]);
                    }
                }
                
                if (newHandlers.Count == 0)
                {
                    _handlersByType.Remove(eventType);
                }
                else
                {
                    _handlersByType[eventType] = newHandlers.ToArray();
                }
            }
        }

        public async UniTask PublishAsync<T>(T gameEvent) where T : IGameEvent
        {
            var eventType = typeof(T);
            
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[AsyncEventBus] Publishing event: {eventType.Name} (ID: {gameEvent.EventId})");
            #endif

            if (!_handlersByType.ContainsKey(eventType))
            {
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[AsyncEventBus] No handlers registered for event: {eventType.Name}");
                #endif
                return;
            }

            var handlers = _handlersByType[eventType];
            if (handlers.Length == 0) return;

            // Get pooled list for tasks
            var tasks = GetTaskList();
            
            try
            {
                // Execute handlers by priority groups without LINQ allocations
                var currentPriority = handlers[0].Priority;
                var priorityStartIndex = 0;
                
                for (int i = 0; i <= handlers.Length; i++)
                {
                    // Check if we need to execute current priority group
                    bool shouldExecute = i == handlers.Length || 
                                       (i < handlers.Length && handlers[i].Priority != currentPriority);
                    
                                         if (shouldExecute && priorityStartIndex < i)
                     {
                         // Execute all handlers of current priority
                         await ExecutePriorityGroup(handlers, priorityStartIndex, i, tasks, currentPriority, gameEvent);
                         priorityStartIndex = i;
                     }
                    
                    if (i < handlers.Length)
                    {
                        currentPriority = handlers[i].Priority;
                    }
                }
            }
            finally
            {
                // Return list to pool
                ReturnTaskList(tasks);
            }

            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[AsyncEventBus] All handlers completed for event: {eventType.Name}");
            #endif

            // Complete any waiting operations
            CompleteWaitingOperations(gameEvent.EventId);
        }

        private async UniTask ExecutePriorityGroup<T>(HandlerInfo[] handlers, int startIndex, int endIndex, 
            List<UniTask> tasks, EventPriority priority, T gameEvent) where T : IGameEvent
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[AsyncEventBus] Executing {endIndex - startIndex} handlers with priority: {priority}");
            #endif
            
            tasks.Clear();
            
            // Execute all handlers of current priority
            for (int i = startIndex; i < endIndex; i++)
            {
                if (handlers[i].Handler is IAsyncEventHandler<T> typedHandler)
                {
                    tasks.Add(typedHandler.HandleAsync(gameEvent));
                }
            }
            
            // Wait for all handlers of current priority to complete
            if (tasks.Count > 0)
            {
                await UniTask.WhenAll(tasks);
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[AsyncEventBus] All handlers with priority {priority} completed");
                #endif
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
            CompleteWaitingOperations(eventId);
        }

        private void CompleteWaitingOperations(string eventId)
        {
            if (_eventCompletionSources.TryGetValue(eventId, out var completionSources))
            {
                foreach (var source in completionSources)
                {
                    source.TrySetResult();
                }
                _eventCompletionSources.Remove(eventId);
            }
        }

        public void Clear()
        {
            _handlersByType.Clear();
            _eventCompletionSources.Clear();
        }

        // Object pooling methods
        private List<UniTask> GetTaskList()
        {
            if (_taskListPool.Count > 0)
            {
                return _taskListPool.Pop();
            }
            return new List<UniTask>();
        }

        private void ReturnTaskList(List<UniTask> list)
        {
            list.Clear();
            _taskListPool.Push(list);
        }

        private class HandlerInfo
        {
            public IAsyncEventHandler Handler { get; }
            public EventPriority Priority { get; }

            public HandlerInfo(IAsyncEventHandler handler, EventPriority priority)
            {
                Handler = handler;
                Priority = priority;
            }
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