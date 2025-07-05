using Models;
using UnityEngine;
using UI.GameWindow;
using Command;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Services;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {        
        [SerializeField] private GameWindowView gameWindowView;

        private GameWindowPresenter gameWindowPresenter;    
        private CommandFactory commandFactory;

        public bool InitalizeGame(GameConfig gameConfig)
        {
            GameModel.Initialize(gameConfig);
            
            // Initialize services
            var musicCardService = new MusicCardService();
            
            // Initialize CommandFactory
            commandFactory = new CommandFactory(musicCardService, GameModel.Instance);

            CreateGameWindow();
            return true;
        }

        private void CreateGameWindow()
        {
            gameWindowPresenter = new GameWindowPresenter(gameWindowView, commandFactory);
        }

        public async UniTask StartGame()
        {
            var startGameCommand = new StartGameCommand(GameModel.Instance);
            await startGameCommand.Execute();
        }

        private void OnDestroy()
        {
            // Clean up event bus when game manager is destroyed
            AsyncEventBus.Instance.Clear();
        }
    }
}