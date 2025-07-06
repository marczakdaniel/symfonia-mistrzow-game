using Models;
using UnityEngine;
using UI.GameWindow;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Events;
using Command;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {        
        [SerializeField] private GameWindowView gameWindowView;

        private GameWindowPresenter gameWindowPresenter;    
        private CommandFactory commandFactory;
        private GameModel gameModel;

        public bool InitalizeGame(GameConfig gameConfig)
        {
            // Initialize MusicCardRepository
            MusicCardRepository.Instance.Initialize(gameConfig.musicCardDatas);
            // Initialize and create GameModel
            gameModel = new GameModel();
            gameModel.Initialize(gameConfig);

            
            // Initialize CommandFactory
            commandFactory = new CommandFactory(gameModel);
            CommandService.Instance.Initialize(commandFactory);

            CreateGameWindow();
            return true;
        }

        private void CreateGameWindow()
        {
            gameWindowPresenter = new GameWindowPresenter(gameWindowView, commandFactory, gameModel);
        }

        public async UniTask StartGame()
        {
            // TODO: Proper command execution
            var startGameCommand = commandFactory.CreateStartGameCommand();
            await CommandService.Instance.ExecuteCommandAsync(startGameCommand);

            // TODO: Start turn for first player
        }

        private void OnDestroy()
        {
            // Clean up event bus when game manager is destroyed
            AsyncEventBus.Instance.Clear();
        }
    }
}