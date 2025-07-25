using Models;
using UnityEngine;

/*
namespace Command
{
    /// <summary>
    /// Opcjonalny MonoBehaviour component do debugowania CommandService w Inspector
    /// Można dodać do sceny jeśli chcemy monitorować stan komend w runtime
    /// </summary>
    public class CommandServiceDebugger : MonoBehaviour
    {
        [Header("Command Service Debug")]
        [SerializeField] private bool showDebugInfo = true;
        [SerializeField] private bool showOnGUI = true;
        [SerializeField] private bool autoRefresh = true;
        [SerializeField] private float refreshInterval = 1f;
        
        [Space]
        [Header("Status Info")]
        [SerializeField] private bool isInitialized;
        [SerializeField] private bool isExecuting;
        [SerializeField] private int queuedCommands;
        [SerializeField] private bool isHealthy;
        [SerializeField] private float successRate;
        [SerializeField] private int totalExecuted;
        
        private float lastRefreshTime;

        private void Start()
        {
            Debug.Log("[CommandServiceDebugger] Started - monitoring CommandService");
        }

        private void Update()
        {
            if (autoRefresh && Time.time - lastRefreshTime > refreshInterval)
            {
                RefreshStatus();
                lastRefreshTime = Time.time;
            }
        }

        private void RefreshStatus()
        {
            if (CommandService.Instance.IsInitialized)
            {
                isInitialized = CommandService.Instance.IsInitialized;
                isExecuting = CommandService.Instance.IsExecuting;
                queuedCommands = CommandService.Instance.QueuedCommandsCount;
                isHealthy = CommandService.Instance.IsHealthy();
                
                var stats = CommandService.Instance.GetExecutionStats();
                successRate = (float)stats.SuccessRate;
                totalExecuted = stats.TotalExecuted;
            }
            else
            {
                isInitialized = false;
                isExecuting = false;
                queuedCommands = 0;
                isHealthy = false;
                successRate = 0f;
                totalExecuted = 0;
            }
        }

        private void OnGUI()
        {
            if (!showDebugInfo || !showOnGUI)
                return;

            GUILayout.BeginArea(new Rect(10, 10, 300, 250));
            
            GUILayout.Label("Command Service Status", GUIStyle.none);
            GUILayout.Space(5);
            
            GUILayout.Label($"Initialized: {(isInitialized ? "✓" : "✗")}");
            GUILayout.Label($"Executing: {(isExecuting ? "Yes" : "No")}");
            GUILayout.Label($"Queued: {queuedCommands}");
            GUILayout.Label($"Healthy: {(isHealthy ? "✓" : "✗")}");
            GUILayout.Label($"Success Rate: {successRate:F1}%");
            GUILayout.Label($"Total Executed: {totalExecuted}");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Refresh Now"))
            {
                RefreshStatus();
            }
            
            if (GUILayout.Button("Show Detailed Status"))
            {
                if (CommandService.Instance.IsInitialized)
                {
                    Debug.Log(CommandService.Instance.GetDetailedStatus());
                }
                else
                {
                    Debug.Log("[CommandServiceDebugger] CommandService not initialized");
                }
            }
            
            if (GUILayout.Button("Show Command History"))
            {
                ShowCommandHistory();
            }
            
            if (GUILayout.Button("Cancel All Pending"))
            {
                CommandService.Instance.CancelAllPending();
                Debug.Log("[CommandServiceDebugger] Cancelled all pending commands");
            }
            
            GUILayout.EndArea();
        }

        private void ShowCommandHistory()
        {
            if (!CommandService.Instance.IsInitialized)
            {
                Debug.Log("[CommandServiceDebugger] CommandService not initialized");
                return;
            }

            var history = CommandService.Instance.GetExecutionHistory();
            Debug.Log($"[CommandServiceDebugger] Command History ({history.Count} entries):");
            
            foreach (var cmd in history)
            {
                var duration = cmd.CompletedAt.HasValue && cmd.StartedAt.HasValue 
                    ? (cmd.CompletedAt.Value - cmd.StartedAt.Value).TotalMilliseconds
                    : 0;
                    
                Debug.Log($"  {cmd.Command.CommandType}: {cmd.Status} ({duration:F0}ms)");
            }
        }

        [ContextMenu("Force Refresh")]
        public void ForceRefresh()
        {
            RefreshStatus();
            Debug.Log("[CommandServiceDebugger] Status refreshed manually");
        }

        [ContextMenu("Initialize CommandService")]
        public void InitializeCommandService()
        {
            if (CommandService.Instance.IsInitialized)
            {
                Debug.LogWarning("[CommandServiceDebugger] CommandService already initialized");
                return;
            }

            // Próba automatycznej inicjalizacji dla debugowania
            var gameModel = new GameModel();
            var commandFactory = new CommandFactory(gameModel);
            CommandService.Instance.Initialize(commandFactory);
            
            Debug.Log("[CommandServiceDebugger] CommandService initialized for debugging");
        }

        [ContextMenu("Clear CommandService")]
        public void ClearCommandService()
        {
            CommandService.Instance.Clear();
            RefreshStatus();
            Debug.Log("[CommandServiceDebugger] CommandService cleared");
        }

        private void OnDestroy()
        {
            Debug.Log("[CommandServiceDebugger] Destroyed");
        }

        /*
         * INSTRUKCJE UŻYCIA:
         * 
         * 1. Dodaj ten component do GameObject w scenie
         * 2. W Inspectorze możesz:
         *    - Włączyć/wyłączyć debug info
         *    - Ustawić auto refresh
         *    - Kliknąć prawym na component → Initialize CommandService (do testów)
         *    - Kliknąć prawym na component → Force Refresh
         * 3. W runtime zobaczysz status w OnGUI
         * 4. Używaj przycisków do interakcji z CommandService
         * 
         * ALTERNATIVE: Można też używać CommandService.Instance.GetDetailedStatus() 
         * w swoich własnych debug skryptach
         */
    //}
//} 