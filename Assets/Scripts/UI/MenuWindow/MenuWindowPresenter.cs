using System;
using Command;
using R3;
using UI.CreateGameWindow;
using UI.CreatePlayerWindow;
using UI.StartPageWindow;

namespace UI.MenuWindow
{
    public class MenuWindowPresenter
    {
        private readonly MenuWindowView view;
        private readonly CommandFactory commandFactory;
        private StartPageWindowPresenter startPageWindowPresenter;
        private CreateGameWindowPresenter createGameWindowPresenter;
        private CreatePlayerWindowPresenter createPlayerWindowPresenter;
        private IDisposable disposable;

        public MenuWindowPresenter(MenuWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;
            
            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeChildMVP()
        {
            startPageWindowPresenter = new StartPageWindowPresenter(view.StartPageWindowView, commandFactory);
            createGameWindowPresenter = new CreateGameWindowPresenter(view.CreateGameWindowView, commandFactory);
            createPlayerWindowPresenter = new CreatePlayerWindowPresenter(view.CreatePlayerWindowView, commandFactory);
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
        }

        private void SubscribeToEvents()
        {
        }
    }
}