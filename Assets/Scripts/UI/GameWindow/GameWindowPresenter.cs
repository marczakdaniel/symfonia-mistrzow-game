using UI.Board;
using Command;
using UI.MusicCardDetailsPanel;
using Cysharp.Threading.Tasks;

namespace UI.GameWindow
{
    public class GameWindowPresenter
    {
        private readonly GameWindowView view;
        private readonly GameWindowViewModel viewModel = new GameWindowViewModel();
        private BoardPresenter boardPresenter;
        private MusicCardDetailsPanelPresenter musicCardDetailsPanelPresenter;
        private CommandFactory commandFactory;

        public GameWindowPresenter(GameWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
        }

        private void InitializeChildMVP()
        {
            boardPresenter = new BoardPresenter(view.BoardView, commandFactory);
            musicCardDetailsPanelPresenter = new MusicCardDetailsPanelPresenter(view.MusicCardDetailsPanelView, commandFactory);
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {

        }

        private void ConnectView()
        {

        }

        public async UniTask StartGame()
        {
            var command = commandFactory.CreateStartGameCommand();
            await command.Execute();
        }
    }
}