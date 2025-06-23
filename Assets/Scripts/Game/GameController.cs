using UnityEngine;
using SplendorGame.Game.Models;
using SplendorGame.Game.Views;
using SplendorGame.Game.Presenters;
using SplendorGame.Game.Data;

namespace SplendorGame.Game
{
    /// <summary>
    /// Main game controller managing the entire game lifecycle
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Game Data")]
        [SerializeField] private CardData[] level1Cards;
        [SerializeField] private CardData[] level2Cards;
        [SerializeField] private CardData[] level3Cards;
        
        [Header("UI References")]
        [SerializeField] private GameBoardView gameBoardView;
        [SerializeField] private Canvas gameCanvas;
        
        [Header("Game Settings")]
        [SerializeField] private bool isMultiplayer = false;
        [SerializeField] private string[] playerNames = { "Player 1", "Player 2" };
        
        private GameStateModel _gameModel;
        private GamePresenter _gamePresenter;
        
        private void Awake()
        {
            InitializeGame();
        }
        
        private void Start()
        {
            SetupPlayers();
            StartGame();
        }
        
        private void InitializeGame()
        {
            // Create game model
            _gameModel = new GameStateModel();
            _gameModel.Initialize();
            
            // Initialize game board view
            if (gameBoardView != null)
            {
                gameBoardView.Initialize();
            }
            
            // Create main game presenter
            _gamePresenter = new GamePresenter(_gameModel, gameBoardView, level1Cards, level2Cards, level3Cards);
            _gamePresenter.Initialize();
        }
        
        private void SetupPlayers()
        {
            foreach (var playerName in playerNames)
            {
                _gamePresenter.AddPlayer(playerName);
            }
        }
        
        private void StartGame()
        {
            _gamePresenter.StartGame();
        }
        
        // Public methods for external control (e.g., from UI or multiplayer)
        public void AddPlayer(string playerName)
        {
            _gamePresenter.AddPlayer(playerName);
        }
        
        public void SetMultiplayerMode(bool multiplayer)
        {
            isMultiplayer = multiplayer;
        }
        
        public GameStateModel GetGameModel()
        {
            return _gameModel;
        }
        
        public void RestartGame()
        {
            _gamePresenter?.Dispose();
            _gameModel?.Dispose();
            
            InitializeGame();
            SetupPlayers();
            StartGame();
        }
        
        private void OnDestroy()
        {
            _gamePresenter?.Dispose();
            _gameModel?.Dispose();
        }
        
        #if UNITY_EDITOR
        [ContextMenu("Test Add Player")]
        private void TestAddPlayer()
        {
            AddPlayer($"Player {Random.Range(1000, 9999)}");
        }
        
        [ContextMenu("Test Restart Game")]
        private void TestRestartGame()
        {
            RestartGame();
        }
        #endif
    }
} 