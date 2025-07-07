using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Command
{
    /// <summary>
    /// Centralizowany service do zarządzania wszystkimi komendami w grze
    /// Analogiczny do AsyncEventBus - zwykła klasa singleton
    /// </summary>
    public class CommandService
    {
        private static CommandService _instance;
        public static CommandService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CommandService();
                }
                return _instance;
            }
        }

        private CommandExecutor _commandExecutor;
        private CommandFactory _commandFactory;
        private bool _isInitialized = false;

        private CommandService() { }

        public bool IsInitialized => _isInitialized;
        public bool IsExecuting => _commandExecutor?.IsExecuting ?? false;
        public int QueuedCommandsCount => _commandExecutor?.QueuedCommandsCount ?? 0;

        /// <summary>
        /// Inicjalizuje CommandService z CommandFactory
        /// </summary>
        public void Initialize(CommandFactory commandFactory)
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[CommandService] Już zainicjalizowany!");
                return;
            }

            _commandFactory = commandFactory;
            _commandExecutor = new CommandExecutor();

            // Zarejestruj wszystkie typy komend
            RegisterCommandTypes();

            _isInitialized = true;
            Debug.Log("[CommandService] Zainicjalizowany pomyślnie");
        }

        /// <summary>
        /// Rejestruje wszystkie dostępne typy komend
        /// </summary>
        private void RegisterCommandTypes()
        {
            _commandExecutor.RegisterCommandType<OpenMusicCardDetailsPanelCommand>();
            _commandExecutor.RegisterCommandType<CloseMusicCardDetailsPanelCommand>();

            _commandExecutor.RegisterCommandType<StartGameCommand>();
            _commandExecutor.RegisterCommandType<BuyMusicCardCommand>();
            _commandExecutor.RegisterCommandType<ReserveMusicCardCommand>();
            
            Debug.Log("[CommandService] Zarejestrowano typy komend");
        }

        /// <summary>
        /// Wykonuje komendę asynchronicznie - główna metoda do użycia przez prezenterów
        /// </summary>
        public async UniTask<bool> ExecuteCommandAsync(ICommand command)
        {
            if (!_isInitialized)
            {
                Debug.LogError("[CommandService] Nie zainicjalizowany! Wywołaj Initialize() najpierw.");
                return false;
            }

            if (command == null)
            {
                Debug.LogError("[CommandService] Nie można wykonać null command");
                return false;
            }

            Debug.Log($"[CommandService] Otrzymano zlecenie wykonania: {command.CommandType}");

            try
            {
                return await _commandExecutor.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CommandService] Błąd podczas wykonywania {command.CommandType}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Wykonuje komendę bez oczekiwania na wynik
        /// </summary>
        public string ExecuteCommandFireAndForget(ICommand command)
        {
            if (!_isInitialized)
            {
                Debug.LogError("[CommandService] Nie zainicjalizowany!");
                return null;
            }

            return _commandExecutor.ExecuteFireAndForget(command);
        }

        /// <summary>
        /// Wygodne metody do tworzenia i wykonywania komend
        /// </summary>
        public async UniTask<bool> StartGameAsync()
        {
            var command = _commandFactory.CreateStartGameCommand();
            return await ExecuteCommandAsync(command);
        }

        public async UniTask<bool> BuyMusicCardAsync(string playerId, string cardId)
        {
            var command = _commandFactory.CreateBuyMusicCardCommand(playerId, cardId);
            return await ExecuteCommandAsync(command);
        }

        public async UniTask<bool> ReserveMusicCardAsync(string playerId, string cardId)
        {
            var command = _commandFactory.CreateReserveMusicCardCommand(playerId, cardId);
            return await ExecuteCommandAsync(command);
        }

        /// <summary>
        /// Wykonuje sekwencję komend w podanej kolejności
        /// </summary>
        public async UniTask<bool> ExecuteSequenceAsync(IEnumerable<ICommand> commands)
        {
            if (!_isInitialized)
            {
                Debug.LogError("[CommandService] Nie zainicjalizowany!");
                return false;
            }

            return await _commandExecutor.ExecuteSequenceAsync(commands);
        }

        /// <summary>
        /// Anuluje wszystkie oczekujące komendy
        /// </summary>
        public void CancelAllPending()
        {
            if (_isInitialized)
            {
                _commandExecutor.CancelAllPending();
            }
        }

        /// <summary>
        /// Zatrzymuje wykonywanie komend
        /// </summary>
        public void Stop()
        {
            if (_isInitialized)
            {
                _commandExecutor.Stop();
            }
        }

        /// <summary>
        /// Czyści cały service i reset do stanu początkowego
        /// </summary>
        public void Clear()
        {
            if (_commandExecutor != null)
            {
                _commandExecutor.Clear();
            }
            
            _commandExecutor = null;
            _commandFactory = null;
            _isInitialized = false;
            
            Debug.Log("[CommandService] Wyczyszczono service");
        }

        /// <summary>
        /// Zwraca statystyki wykonywania komend
        /// </summary>
        public CommandExecutionStats GetExecutionStats()
        {
            if (!_isInitialized)
            {
                return new CommandExecutionStats();
            }

            return _commandExecutor.GetExecutionStats();
        }

        /// <summary>
        /// Zwraca historię wykonanych komend
        /// </summary>
        public List<QueuedCommand> GetExecutionHistory()
        {
            if (!_isInitialized)
            {
                return new List<QueuedCommand>();
            }

            return _commandExecutor.GetExecutionHistory();
        }

        /// <summary>
        /// Zwraca szczegółowe informacje o stanie service
        /// </summary>
        public string GetDetailedStatus()
        {
            if (!_isInitialized)
            {
                return "[CommandService] Nie zainicjalizowany";
            }

            return _commandExecutor.GetDetailedStatus();
        }

        /// <summary>
        /// Sprawdza czy service jest w stanie obsłużyć nowe komendy
        /// </summary>
        public bool IsHealthy()
        {
            if (!_isInitialized)
            {
                return false;
            }

            return _commandExecutor.IsHealthy();
        }

        /// <summary>
        /// Zwraca statystyki pogrupowane według typu komendy
        /// </summary>
        public Dictionary<string, int> GetCommandTypeStats()
        {
            if (!_isInitialized)
            {
                return new Dictionary<string, int>();
            }

            return _commandExecutor.GetCommandTypeStats();
        }

        /// <summary>
        /// Resetuje singleton instance (przydatne do testów)
        /// </summary>
        public static void ResetInstance()
        {
            _instance?.Clear();
            _instance = null;
        }
    }
} 