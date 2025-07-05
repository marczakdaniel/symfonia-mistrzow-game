using Events;
using Models;
using UnityEngine;
using UI.GameWindow;
using Command;

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
            EventBus.Initialize();

            CreateGameWindow();
            return true;
        }

        private void CreateGameWindow()
        {
            gameWindowPresenter = new GameWindowPresenter(gameWindowView, commandFactory);
        }

        public void StartGame()
        {
            gameWindowPresenter.StartGame();
        }
    }
}