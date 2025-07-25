using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Models;


/*

namespace Command
{
    /// <summary>
    /// Przykład jak używać systemu CommandQueue w prezenterach
    /// </summary>
    public class CommandQueueUsageExample : MonoBehaviour
    {
        private CommandExecutor commandExecutor;
        private CommandFactory commandFactory;

        private void Start()
        {
            // Initialize the command system
            InitializeCommandSystem();
            
            // Show examples
            _ = RunExamples();
        }

        private void InitializeCommandSystem()
        {
            // Create command factory
            commandFactory = new CommandFactory(new GameModel());
            
            // Create command executor
            commandExecutor = new CommandExecutor();
            
            // Register command types for statistics
            commandExecutor.RegisterCommandType<StartGameCommand>();
            //commandExecutor.RegisterCommandType<BuyMusicCardCommand>();
            //commandExecutor.RegisterCommandType<ReserveMusicCardCommand>();
            
            Debug.Log("[Example] Command system initialized");
        }

        private async UniTask RunExamples()
        {
            await UniTask.Delay(1000); // Wait a bit
            
            // Example 1: Execute single command
            await ExecuteSingleCommandExample();
            
            await UniTask.Delay(500);
            
            // Example 2: Execute multiple commands in sequence  
            await ExecuteMultipleCommandsExample();
            
            await UniTask.Delay(500);
            
            // Example 3: Fire and forget commands
            ExecuteFireAndForgetExample();
            
            await UniTask.Delay(1000);
            
            // Example 4: Show statistics
            ShowStatistics();
        }

        /// <summary>
        /// Przykład 1: Wykonanie pojedynczej komendy
        /// </summary>
        private async UniTask ExecuteSingleCommandExample()
        {
            Debug.Log("[Example] === Executing Single Command ===");
            
            var startGameCommand = commandFactory.CreateStartGameCommand();
            
            // Execute and wait for completion
            bool success = await commandExecutor.ExecuteAsync(startGameCommand);
            
            Debug.Log($"[Example] Start game command completed: {success}");
        }

        /// <summary>
        /// Przykład 2: Wykonanie wielu komend w kolejności
        /// </summary>
        private async UniTask ExecuteMultipleCommandsExample()
        {
            Debug.Log("[Example] === Executing Multiple Commands ===");
            
            var commands = new List<ICommand>
            {
                //commandFactory.CreateBuyMusicCardCommand("player1", "card1"),
                //commandFactory.CreateReserveMusicCardCommand("player1", "card2"),
                //commandFactory.CreateBuyMusicCardCommand("player1", "card3")
            };
            
            // Execute all commands in sequence
            bool allSuccess = await commandExecutor.ExecuteSequenceAsync(commands);
            
            Debug.Log($"[Example] All commands completed successfully: {allSuccess}");
        }

        /// <summary>
        /// Przykład 3: Fire and forget commands
        /// </summary>
        private void ExecuteFireAndForgetExample()
        {
            Debug.Log("[Example] === Fire and Forget Commands ===");
            
            // These commands will execute in background without waiting
            //var command1 = commandFactory.CreateBuyMusicCardCommand("player2", "card4");
            //var command2 = commandFactory.CreateBuyMusicCardCommand("player2", "card5");
            
            //string id1 = commandExecutor.ExecuteFireAndForget(command1);
            //string id2 = commandExecutor.ExecuteFireAndForget(command2);
            
            //Debug.Log($"[Example] Queued commands: {id1}, {id2}");
        }

        /// <summary>
        /// Przykład 4: Pokazanie statystyk
        /// </summary>
        private void ShowStatistics()
        {
            Debug.Log("[Example] === Command Statistics ===");
            
            var stats = commandExecutor.GetExecutionStats();
            var typeStats = commandExecutor.GetCommandTypeStats();
            
            Debug.Log($"[Example] Total executed: {stats.TotalExecuted}");
            Debug.Log($"[Example] Success rate: {stats.SuccessRate:F1}%");
            Debug.Log($"[Example] Average execution time: {stats.AverageExecutionTime.TotalMilliseconds:F0}ms");
            
            foreach (var kvp in typeStats)
            {
                Debug.Log($"[Example] {kvp.Key}: {kvp.Value} executions");
            }
            
            Debug.Log($"[Example] Queue status: {commandExecutor.QueuedCommandsCount} queued, {commandExecutor.IsExecuting} executing");
        }

        /*
         * === JAK UŻYWAĆ W PRAWDZIWYCH PREZENTERACH ===

        /// <summary>
        /// Przykład użycia w Presenter - sposób 1: Bezpośrednie użycie CommandExecutor
        /// </summary>
        public class ExamplePresenterWithDirectExecutor
        {
            private CommandExecutor commandExecutor;
            private CommandFactory commandFactory;

            public ExamplePresenterWithDirectExecutor(CommandFactory factory)
            {
                commandFactory = factory;
                commandExecutor = new CommandExecutor();
            }

            public async UniTask HandleBuyCardClick(string cardId)
            {
                Debug.Log($"[Presenter] User clicked buy card: {cardId}");
                
                //var command = commandFactory.CreateBuyMusicCardCommand("currentPlayer", cardId);
                //bool success = await commandExecutor.ExecuteAsync(command);

                bool success = false;
                
                if (success)
                {
                    Debug.Log("[Presenter] Card purchased successfully!");
                }
                else
                {
                    Debug.Log("[Presenter] Failed to purchase card!");
                }
            }
        }

        /// <summary>
        /// Przykład użycia w Presenter - sposób 2: Globalny CommandService (RECOMMENDED)
        /// </summary>
        public class ExamplePresenterWithGlobalService
        {
            // Ten przykład używa nowego CommandService - to jest preferowany sposób!
            
            public async UniTask HandleReserveCardClick(string cardId)
            {
                Debug.Log($"[Presenter] User clicked reserve card: {cardId}");
                
                // NOWY SPOSÓB - używaj CommandService.Instance:
                //bool success = await CommandService.Instance.ReserveMusicCardAsync("currentPlayer", cardId);
                
                bool success = false;
                
                if (success)
                {
                    Debug.Log("[Presenter] Card reserved successfully!");
                }
                else
                {
                    Debug.Log("[Presenter] Failed to reserve card!");
                }
            }
            
            // Więcej przykładów z CommandService:
            public async UniTask HandleBuyCardClick(string cardId)
            {
                //bool success = await CommandService.Instance.BuyMusicCardAsync("currentPlayer", cardId);
                bool success = false;
                Debug.Log($"[Presenter] Buy card result: {success}");
            }
            
            public async UniTask HandleStartGame()
            {
                bool success = await CommandService.Instance.StartGameAsync();
                Debug.Log($"[Presenter] Start game result: {success}");
            }
        }

        /// <summary>
        /// Przykład użycia w Presenter - sposób 3: Z dependency injection
        /// </summary>
        public class ExamplePresenterWithDI
        {
            private readonly CommandExecutor commandExecutor;
            private readonly CommandFactory commandFactory;

            // Dependency injection through constructor
            public ExamplePresenterWithDI(CommandExecutor executor, CommandFactory factory)
            {
                commandExecutor = executor;
                commandFactory = factory;
            }

            public async UniTask HandlePlayerTurn(string playerId)
            {
                Debug.Log($"[Presenter] Starting turn for player: {playerId}");
                
                // Execute multiple commands for turn setup
                var turnCommands = new List<ICommand>
                {
                    // Add turn-related commands here
                };
                
                await commandExecutor.ExecuteSequenceAsync(turnCommands);
                
                Debug.Log($"[Presenter] Turn setup completed for: {playerId}");
            }
        }

        private void OnDestroy()
        {
            // Clean up command executor
            commandExecutor?.Clear();
        }

        /*
         * === ZALETY SYSTEMU KOLEJKI KOMEND ===
         * 
         * 1. SEKWENCYJNA KOLEJNOŚĆ: Komendy wykonują się w kolejności otrzymywania
         * 2. THREAD SAFETY: Bezpieczne dla wielu wątków
         * 3. ASYNC/AWAIT: Pełne wsparcie UniTask
         * 4. STATYSTYKI: Tracking wykonanych komend
         * 5. ERROR HANDLING: Graceful handling błędów
         * 6. CANCELLATION: Możliwość anulowania oczekujących komend
         * 7. FIRE & FORGET: Opcjonalne wykonanie bez oczekiwania
         * 8. HISTORY: Historia wykonanych komend do debugowania
         * 9. VALIDATION: Automatyczna walidacja przed wykonaniem
         * 10. HEALTH MONITORING: Monitoring stanu systemu
         * 
         * === PRZEPŁYW W GRZE ===
         * 
         * 1. User kliknie przycisk w UI
         * 2. Presenter odbiera event
         * 3. Presenter tworzy Command przez CommandFactory
         * 4. Presenter wykonuje Command przez CommandExecutor
         * 5. CommandExecutor dodaje Command do CommandQueue
         * 6. CommandQueue wykonuje Command sekwencyjnie
         * 7. Command aktualizuje Model i publikuje Events
         * 8. EventBus informuje wszystkie Presentery o zmianach
         * 9. Presentery aktualizują UI na podstawie nowego stanu Model
         * 10. Command kończy się, Presenter otrzymuje wynik
    }
} 

*/
