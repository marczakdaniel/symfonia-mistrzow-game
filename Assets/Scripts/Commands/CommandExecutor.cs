using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// Statystyki wykonywania komend
    /// </summary>
    public class CommandExecutionStats
    {
        public int TotalExecuted { get; set; }
        public int TotalSuccessful { get; set; }
        public int TotalFailed { get; set; }
        public int TotalCancelled { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public DateTime LastExecutionTime { get; set; }

        public double SuccessRate => TotalExecuted > 0 ? (double)TotalSuccessful / TotalExecuted * 100 : 0;
    }

    /// <summary>
    /// High-level interfejs do zarządzania wykonywaniem komend
    /// </summary>
    public class CommandExecutor
    {
        private readonly CommandQueue _commandQueue = new();
        private readonly Dictionary<string, Type> _commandTypeRegistry = new();
        private readonly object _registryLock = new();

        /// <summary>
        /// Ile komend czeka w kolejce
        /// </summary>
        public int QueuedCommandsCount => _commandQueue.QueuedCount;

        /// <summary>
        /// Czy executor obecnie przetwarza komendy
        /// </summary>
        public bool IsExecuting => _commandQueue.IsProcessing;

        /// <summary>
        /// Rejestruje typ komendy dla statystyk i debugowania
        /// </summary>
        public void RegisterCommandType<T>() where T : ICommand
        {
            lock (_registryLock)
            {
                var type = typeof(T);
                _commandTypeRegistry[type.Name] = type;
            }
            
            Debug.Log($"[CommandExecutor] Zarejestrowano typ komendy: {typeof(T).Name}");
        }

        /// <summary>
        /// Wykonuje komendę asynchronicznie i czeka na jej zakończenie
        /// </summary>
        /// <param name="command">Komenda do wykonania</param>
        /// <returns>True jeśli komenda została wykonana pomyślnie</returns>
        public async UniTask<bool> ExecuteAsync(ICommand command)
        {
            if (command == null)
            {
                Debug.LogError("[CommandExecutor] Nie można wykonać null command");
                return false;
            }

            Debug.Log($"[CommandExecutor] Zlecenie wykonania komendy: {command.CommandType}");

            try
            {
                return await _commandQueue.EnqueueAsync(command);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CommandExecutor] Błąd podczas wykonywania komendy {command.CommandType}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Wykonuje komendę bez oczekiwania na wynik (Fire & Forget)
        /// </summary>
        /// <param name="command">Komenda do wykonania</param>
        /// <returns>ID komendy w kolejce</returns>
        public string ExecuteFireAndForget(ICommand command)
        {
            if (command == null)
            {
                Debug.LogError("[CommandExecutor] Nie można wykonać null command");
                return null;
            }

            Debug.Log($"[CommandExecutor] Zlecenie wykonania komendy (Fire&Forget): {command.CommandType}");

            try
            {
                return _commandQueue.EnqueueFireAndForget(command);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CommandExecutor] Błąd podczas dodawania komendy do kolejki {command.CommandType}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Wykonuje sekwencję komend w podanej kolejności
        /// </summary>
        /// <param name="commands">Lista komend do wykonania</param>
        /// <returns>True jeśli wszystkie komendy zostały wykonane pomyślnie</returns>
        public async UniTask<bool> ExecuteSequenceAsync(IEnumerable<ICommand> commands)
        {
            if (commands == null)
            {
                Debug.LogError("[CommandExecutor] Nie można wykonać null sequence");
                return false;
            }

            var commandList = commands.ToList();
            if (commandList.Count == 0)
            {
                Debug.LogWarning("[CommandExecutor] Pusta sekwencja komend");
                return true;
            }

            Debug.Log($"[CommandExecutor] Wykonywanie sekwencji {commandList.Count} komend");

            bool allSuccessful = true;
            int successCount = 0;

            foreach (var command in commandList)
            {
                try
                {
                    bool result = await ExecuteAsync(command);
                    if (result)
                    {
                        successCount++;
                    }
                    else
                    {
                        allSuccessful = false;
                        Debug.LogWarning($"[CommandExecutor] Komenda {command.CommandType} nie powiodła się w sekwencji");
                    }
                }
                catch (Exception ex)
                {
                    allSuccessful = false;
                    Debug.LogError($"[CommandExecutor] Błąd podczas wykonywania {command.CommandType} w sekwencji: {ex.Message}");
                }
            }

            Debug.Log($"[CommandExecutor] Sekwencja zakończona: {successCount}/{commandList.Count} pomyślnych");
            return allSuccessful;
        }

        /// <summary>
        /// Anuluje wszystkie oczekujące komendy
        /// </summary>
        public void CancelAllPending()
        {
            Debug.Log("[CommandExecutor] Anulowanie wszystkich oczekujących komend");
            _commandQueue.CancelAllPending();
        }

        /// <summary>
        /// Zatrzymuje executor
        /// </summary>
        public void Stop()
        {
            Debug.Log("[CommandExecutor] Zatrzymywanie executora");
            _commandQueue.Stop();
        }

        /// <summary>
        /// Czyści executor i całą kolejkę
        /// </summary>
        public void Clear()
        {
            Debug.Log("[CommandExecutor] Czyszczenie executora");
            _commandQueue.Clear();
            
            lock (_registryLock)
            {
                _commandTypeRegistry.Clear();
            }
        }

        /// <summary>
        /// Zwraca statystyki wykonywania komend
        /// </summary>
        public CommandExecutionStats GetExecutionStats()
        {
            var history = _commandQueue.GetExecutionHistory();
            
            var stats = new CommandExecutionStats
            {
                TotalExecuted = history.Count,
                TotalSuccessful = history.Count(h => h.Status == CommandStatus.Completed),
                TotalFailed = history.Count(h => h.Status == CommandStatus.Failed),
                TotalCancelled = history.Count(h => h.Status == CommandStatus.Cancelled),
                LastExecutionTime = history.LastOrDefault()?.CompletedAt ?? DateTime.MinValue
            };

            // Oblicz średni czas wykonania
            var completedCommands = history.Where(h => h.Status == CommandStatus.Completed && h.StartedAt.HasValue && h.CompletedAt.HasValue);
            if (completedCommands.Any())
            {
                var totalTime = completedCommands.Sum(c => (c.CompletedAt!.Value - c.StartedAt!.Value).TotalMilliseconds);
                stats.AverageExecutionTime = TimeSpan.FromMilliseconds(totalTime / completedCommands.Count());
            }

            return stats;
        }

        /// <summary>
        /// Zwraca historię wykonanych komend
        /// </summary>
        public List<QueuedCommand> GetExecutionHistory()
        {
            return _commandQueue.GetExecutionHistory();
        }

        /// <summary>
        /// Zwraca szczegółowe informacje o stanie executora
        /// </summary>
        public string GetDetailedStatus()
        {
            var stats = GetExecutionStats();
            var queueStatus = _commandQueue.GetQueueStatus();
            
            return $"CommandExecutor Status:\n" +
                   $"Queue: {queueStatus}\n" +
                   $"Statistics: {stats.TotalExecuted} executed, {stats.SuccessRate:F1}% success rate\n" +
                   $"Average execution time: {stats.AverageExecutionTime.TotalMilliseconds:F0}ms\n" +
                   $"Registered command types: {_commandTypeRegistry.Count}";
        }

        /// <summary>
        /// Zwraca statystyki pogrupowane według typu komendy
        /// </summary>
        public Dictionary<string, int> GetCommandTypeStats()
        {
            var history = _commandQueue.GetExecutionHistory();
            var stats = new Dictionary<string, int>();

            foreach (var command in history)
            {
                var commandType = command.Command.CommandType;
                if (!stats.ContainsKey(commandType))
                {
                    stats[commandType] = 0;
                }
                stats[commandType]++;
            }

            return stats;
        }

        /// <summary>
        /// Sprawdza czy executor jest w stanie obsłużyć nowe komendy
        /// </summary>
        public bool IsHealthy()
        {
            try
            {
                var stats = GetExecutionStats();
                
                // Sprawdź czy nie ma zbyt wielu nieudanych komend
                if (stats.TotalExecuted > 10 && stats.SuccessRate < 50)
                {
                    Debug.LogWarning($"[CommandExecutor] Niska skuteczność: {stats.SuccessRate:F1}%");
                    return false;
                }

                // Sprawdź czy kolejka nie jest przeparowana
                if (QueuedCommandsCount > 50)
                {
                    Debug.LogWarning($"[CommandExecutor] Bardzo długa kolejka: {QueuedCommandsCount}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CommandExecutor] Błąd podczas sprawdzania stanu: {ex.Message}");
                return false;
            }
        }
    }
} 