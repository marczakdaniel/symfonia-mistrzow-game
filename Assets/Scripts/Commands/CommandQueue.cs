using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// Reprezentuje status komendy w kolejce
    /// </summary>
    public enum CommandStatus
    {
        Pending,
        Executing,
        Completed,
        Failed,
        Cancelled
    }

    /// <summary>
    /// Wrapper dla komendy z dodatkowym statusem i metadanymi
    /// </summary>
    public class QueuedCommand
    {
        public string Id { get; }
        public ICommand Command { get; }
        public CommandStatus Status { get; set; }
        public DateTime QueuedAt { get; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string ErrorMessage { get; set; }
        public UniTaskCompletionSource<bool> CompletionSource { get; }

        public QueuedCommand(ICommand command)
        {
            Id = Guid.NewGuid().ToString();
            Command = command;
            Status = CommandStatus.Pending;
            QueuedAt = DateTime.Now;
            CompletionSource = new UniTaskCompletionSource<bool>();
        }

        public void MarkAsStarted()
        {
            Status = CommandStatus.Executing;
            StartedAt = DateTime.Now;
        }

        public void MarkAsCompleted(bool success, string errorMessage = null)
        {
            Status = success ? CommandStatus.Completed : CommandStatus.Failed;
            CompletedAt = DateTime.Now;
            ErrorMessage = errorMessage;
            CompletionSource.TrySetResult(success);
        }

        public void MarkAsCancelled()
        {
            Status = CommandStatus.Cancelled;
            CompletedAt = DateTime.Now;
            CompletionSource.TrySetResult(false);
        }
    }

    /// <summary>
    /// Kolejka komend zapewniająca sekwencyjne wykonywanie
    /// </summary>
    public class CommandQueue
    {
        private readonly Queue<QueuedCommand> _commandQueue = new();
        private readonly List<QueuedCommand> _executionHistory = new();
        private readonly SemaphoreSlim _executionSemaphore = new(1, 1);
        private readonly object _queueLock = new();
        
        private bool _isProcessing = false;
        private bool _cancelNewCommandsOnExecution = true;
        private CancellationTokenSource _cancellationTokenSource = new();

        public int QueuedCount 
        { 
            get 
            { 
                lock (_queueLock) 
                { 
                    return _commandQueue.Count; 
                } 
            } 
        }

        public bool IsProcessing => _isProcessing;

        public bool CancelNewCommandsOnExecution 
        { 
            get => _cancelNewCommandsOnExecution; 
            set 
            {
                _cancelNewCommandsOnExecution = value;
                // Jeśli włączamy opcję podczas gdy jakaś komenda się wykonuje,
                // anuluj wszystkie oczekujące komendy
                if (value && _isProcessing)
                {
                    CancelAllPending();
                    Debug.Log("[CommandQueue] Anulowano wszystkie oczekujące komendy - włączono opcję CancelOnExecution podczas wykonywania");
                }
            }
        }

        /// <summary>
        /// Dodaje komendę do kolejki i zwraca UniTask do oczekiwania na jej wykonanie
        /// </summary>
        public async UniTask<bool> EnqueueAsync(ICommand command)
        {
            // Sprawdź czy należy anulować nowe komendy podczas wykonywania
            if (_cancelNewCommandsOnExecution && _isProcessing)
            {
                Debug.LogWarning($"[CommandQueue] Anulowano komendę {command.CommandType} - inna komenda jest w trakcie wykonywania");
                return false;
            }

            var queuedCommand = new QueuedCommand(command);
            
            lock (_queueLock)
            {
                _commandQueue.Enqueue(queuedCommand);
            }

            Debug.Log($"[CommandQueue] Dodano komendę do kolejki: {command.CommandType} (ID: {queuedCommand.Id})");

            // Rozpocznij przetwarzanie kolejki jeśli nie jest już uruchomione
            _ = ProcessQueueAsync();

            // Czekaj na zakończenie tej konkretnej komendy
            return await queuedCommand.CompletionSource.Task;
        }

        /// <summary>
        /// Dodaje komendę do kolejki bez oczekiwania na wykonanie
        /// </summary>
        public string EnqueueFireAndForget(ICommand command)
        {
            // Sprawdź czy należy anulować nowe komendy podczas wykonywania
            if (_cancelNewCommandsOnExecution && _isProcessing)
            {
                Debug.LogWarning($"[CommandQueue] Anulowano komendę (Fire&Forget) {command.CommandType} - inna komenda jest w trakcie wykonywania");
                return null;
            }

            var queuedCommand = new QueuedCommand(command);
            
            lock (_queueLock)
            {
                _commandQueue.Enqueue(queuedCommand);
            }

            Debug.Log($"[CommandQueue] Dodano komendę do kolejki (Fire&Forget): {command.CommandType} (ID: {queuedCommand.Id})");

            // Rozpocznij przetwarzanie kolejki jeśli nie jest już uruchomione
            _ = ProcessQueueAsync();

            return queuedCommand.Id;
        }

        /// <summary>
        /// Główna pętla przetwarzania kolejki
        /// </summary>
        private async UniTask ProcessQueueAsync()
        {
            // Zapobiegaj uruchomieniu wielu równoległych procesów
            await _executionSemaphore.WaitAsync();
            
            try
            {
                if (_isProcessing)
                {
                    return;
                }

                _isProcessing = true;
                Debug.Log("[CommandQueue] Rozpoczynam przetwarzanie kolejki");

                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    QueuedCommand queuedCommand = null;

                    lock (_queueLock)
                    {
                        if (_commandQueue.Count == 0)
                        {
                            break;
                        }
                        queuedCommand = _commandQueue.Dequeue();
                    }

                    if (queuedCommand != null)
                    {
                        await ExecuteCommandAsync(queuedCommand);
                    }
                }

                Debug.Log("[CommandQueue] Zakończono przetwarzanie kolejki");
            }
            finally
            {
                _isProcessing = false;
                _executionSemaphore.Release();
            }
        }

        /// <summary>
        /// Wykonuje pojedynczą komendę
        /// </summary>
        private async UniTask ExecuteCommandAsync(QueuedCommand queuedCommand)
        {
            try
            {
                Debug.Log($"[CommandQueue] Rozpoczynam wykonywanie: {queuedCommand.Command.CommandType} (ID: {queuedCommand.Id})");
                
                queuedCommand.MarkAsStarted();
                
                // Walidacja
                if (!await queuedCommand.Command.Validate())
                {
                    queuedCommand.MarkAsCompleted(false, "Validation failed");
                    Debug.LogWarning($"[CommandQueue] Walidacja nieudana: {queuedCommand.Command.CommandType}");
                    return;
                }

                // Wykonanie
                bool success = await queuedCommand.Command.Execute();
                
                queuedCommand.MarkAsCompleted(success);
                
                Debug.Log($"[CommandQueue] Zakończono wykonywanie: {queuedCommand.Command.CommandType} (Success: {success})");
            }
            catch (Exception ex)
            {
                queuedCommand.MarkAsCompleted(false, ex.Message);
                Debug.LogError($"[CommandQueue] Błąd podczas wykonywania {queuedCommand.Command.CommandType}: {ex.Message}");
                throw ex;
            }
            finally
            {
                // Dodaj do historii
                lock (_queueLock)
                {
                    _executionHistory.Add(queuedCommand);
                    
                    // Ogranicz historię do ostatnich 100 komend
                    if (_executionHistory.Count > 100)
                    {
                        _executionHistory.RemoveAt(0);
                    }
                }
            }
        }

        /// <summary>
        /// Anuluje wszystkie oczekujące komendy
        /// </summary>
        public void CancelAllPending()
        {
            lock (_queueLock)
            {
                while (_commandQueue.Count > 0)
                {
                    var queuedCommand = _commandQueue.Dequeue();
                    queuedCommand.MarkAsCancelled();
                }
            }
            
            Debug.Log("[CommandQueue] Anulowano wszystkie oczekujące komendy");
        }

        /// <summary>
        /// Zatrzymuje przetwarzanie kolejki
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            CancelAllPending();
            Debug.Log("[CommandQueue] Zatrzymano kolejkę komend");
        }

        /// <summary>
        /// Czyści całą kolejkę i historię
        /// </summary>
        public void Clear()
        {
            Stop();
            
            lock (_queueLock)
            {
                _commandQueue.Clear();
                _executionHistory.Clear();
            }
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            
            Debug.Log("[CommandQueue] Wyczyszczono kolejkę komend");
        }

        /// <summary>
        /// Zwraca historię wykonanych komend
        /// </summary>
        public List<QueuedCommand> GetExecutionHistory()
        {
            lock (_queueLock)
            {
                return new List<QueuedCommand>(_executionHistory);
            }
        }

        /// <summary>
        /// Zwraca status kolejki do debugowania
        /// </summary>
        public string GetQueueStatus()
        {
            lock (_queueLock)
            {
                return $"Queued: {_commandQueue.Count}, Processing: {_isProcessing}, CancelOnExecution: {_cancelNewCommandsOnExecution}, History: {_executionHistory.Count}";
            }
        }
    }
} 