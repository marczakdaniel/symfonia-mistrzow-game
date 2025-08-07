using System;
using Models;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using Command;
using UI.SelectTokenWindow.ChoosenBoardTokenPanel;
using UI.SelectTokenWindow.SelectBoardToken;
using DefaultNamespace.Data;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowPresenter : 
        IDisposable, 
        IAsyncEventHandler<TokenDetailsPanelOpenedEvent>,
        IAsyncEventHandler<TokenDetailsPanelClosedEvent>,
        IAsyncEventHandler<SelectedTokensConfirmedEvent>
    {
        private readonly SelectTokenWindowView view;
        private readonly CommandFactory commandFactory;
        private IDisposable disposables;

        private SelectBoardTokenPresenter[] selectBoardTokenPresenters = new SelectBoardTokenPresenter[5];
        private ChoosenBoardTokenPanelPresenter choosenBoardTokenPanelPresenter;

        public SelectTokenWindowPresenter(SelectTokenWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeChildMVP();
            InitializeMVP();
            SubscribeToEvents();
        }

        /* ---- */

        private void InitializeChildMVP()
        {
            choosenBoardTokenPanelPresenter = new ChoosenBoardTokenPanelPresenter(view.ChoosenBoardTokenPanelView, commandFactory);

            for (int i = 0; i < selectBoardTokenPresenters.Length; i++)
            {
                selectBoardTokenPresenters[i] = new SelectBoardTokenPresenter(view.SelectBoardTokens[i], (ResourceType)i, commandFactory);
            }
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposables = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().Forget()).AddTo(ref d);
            view.OnAcceptButtonClicked.Subscribe(_ => HandleAcceptButtonClicked().Forget()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseTokenDetailsPanelCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleAcceptButtonClicked()
        {
            var command = commandFactory.CreateAcceptSelectedTokensCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<TokenDetailsPanelClosedEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<SelectedTokensConfirmedEvent>(this);
        }

        public async UniTask HandleAsync(TokenDetailsPanelOpenedEvent openedEvent)
        {
            view.InitializePlayerTokens(openedEvent.CurrentPlayerTokens, openedEvent.CurrentPlayerCards);
            await view.OnOpenWindow();
        }

        public async UniTask HandleAsync(TokenDetailsPanelClosedEvent tokenDetailsPanelClosedEvent)
        {
            await view.OnCloseWindow();
        }

        public async UniTask HandleAsync(SelectedTokensConfirmedEvent selectedTokensConfirmedEvent)
        {
            await view.OnCloseWindow();
        }

        public void Dispose()
        {
            disposables?.Dispose();
        }
    }
}