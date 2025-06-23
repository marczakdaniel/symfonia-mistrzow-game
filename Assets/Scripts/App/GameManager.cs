using UnityEngine;

namespace SymfoniaMistrzow.App
{
    /// <summary>
    /// Manages the main game loop and game states (e.g., MainMenu, InGame, Paused, GameOver).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Singleton pattern for easy access
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Methods to control game state would go here
        // e.g., StartGame(), EndGame(), etc.
    }
} 