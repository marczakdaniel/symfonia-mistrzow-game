using Models;
using UnityEngine;
using UI.GameWindow;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Events;
using Command;
using Services;
using UI.MenuWindow;
using DefaultNamespace.Data;
using Assets.Scripts.Data;
using UI.InfoWindow;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {        
        [SerializeField] private GameWindowView gameWindowView;
        [SerializeField] private MenuWindowView menuWindowView;

        [SerializeField] private MusicCardDeckData musicCardDeckData;
        [SerializeField] private ConcertCardDeckData concertCardDeckData;
        [SerializeField] private InfoWindowView infoWindowView;

        private GameWindowPresenter gameWindowPresenter;    
        private MenuWindowPresenter menuWindowPresenter;
        private InfoWindowPresenter infoWindowPresenter;
        private CommandFactory commandFactory;

        private GameModel gameModel;
        private GameConfig gameConfig;

        private TurnService turnService;
        private BoardService boardService;
        private PlayerService playerService;
        private ConfigService configService;
        private GameService gameService;
        private ConcertCardService concertCardService;  
        public void Start()
        {
            InitializeSingletons();

            CreateModels();
            CreateServices();
            CreateCommandFactory();

            CreateMenuWindow();
            CreateGameWindow();
            CreateInfoWindow();

            OpenStartPageWindow().Forget();
        }

        private async UniTask OpenStartPageWindow()
        {
            var command = commandFactory.CreateOpenStartPageWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void InitializeSingletons()
        {
            AsyncEventBus.Instance.Initialize();
            CommandService.Instance.Initialize();
            MusicCardRepository.Instance.Initialize(musicCardDeckData.Cards);
        }
        private void CreateMenuWindow()
        {
            menuWindowPresenter = new MenuWindowPresenter(menuWindowView, commandFactory);
        }

        private void CreateInfoWindow()
        {
            infoWindowPresenter = new InfoWindowPresenter(infoWindowView, commandFactory);
        }

        private void CreateModels()
        {
            gameConfig = new GameConfig(musicCardDeckData.Cards);
            gameModel = new GameModel();
        }

        private void CreateServices()
        {
            configService = new ConfigService(gameConfig);
            turnService = new TurnService(gameModel);
            boardService = new BoardService(gameModel);
            playerService = new PlayerService(gameModel);
            gameService = new GameService(gameModel);
            concertCardService = new ConcertCardService(concertCardDeckData);
        }

        private void CreateCommandFactory()
        {
            commandFactory = new CommandFactory(gameModel, turnService, boardService, playerService, configService, gameService, concertCardService);
        }


        // this should go to command layer

        private void CreateGameWindow()
        {
            gameWindowPresenter = new GameWindowPresenter(gameWindowView, commandFactory);
        }

        private void OnDestroy()
        {
            AsyncEventBus.Instance.Clear();
            CommandService.Instance.Clear();
        }
    }
}