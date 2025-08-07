using System;
using Command;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using Events;
using R3;

namespace UI.Board.BoardPlayerPanel.BoardPlayerPanelSingleResource
{
    public class BoardPlayerPanelSingleResourcePresenter 
        : IDisposable, 
        IAsyncEventHandler<PlayerResourcesUpdatedEvent>, 
        IAsyncEventHandler<GameStartedEvent>
    {
        private BoardPlayerPanelSingleResourceView view;
        private BoardPlayerPanelSingleResourceViewModel viewModel;
        private IDisposable disposable;
        private CommandFactory commandFactory;

        public BoardPlayerPanelSingleResourcePresenter(int index, ResourceType resourceType, BoardPlayerPanelSingleResourceView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.viewModel = new BoardPlayerPanelSingleResourceViewModel(index, resourceType);
            this.commandFactory = commandFactory;

            Initialize();
            SubscribeToEvents();
        }

        private void Initialize()
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
            AsyncEventBus.Instance.Subscribe<PlayerResourcesUpdatedEvent>(this);
            AsyncEventBus.Instance.Subscribe<GameStartedEvent>(this);
        }

        public async UniTask HandleAsync(PlayerResourcesUpdatedEvent eventData)
        {
            if (eventData.PlayerId != viewModel.PlayerId)
            {
                return;
            }

            if (viewModel.CurrentTokenCount != eventData.CurrentPlayerTokens[viewModel.ResourceType])
            {
                viewModel.SetCurrentTokenCount(eventData.CurrentPlayerTokens[viewModel.ResourceType]);
                view.UpdateTokenValue(eventData.CurrentPlayerTokens[viewModel.ResourceType]);
            }

            if (viewModel.CurrentCardCount != eventData.CurrentPlayerCards[viewModel.ResourceType])
            {
                viewModel.SetCurrentCardCount(eventData.CurrentPlayerCards[viewModel.ResourceType]);
                view.UpdateCardValue(eventData.CurrentPlayerCards[viewModel.ResourceType]);
            }

            await UniTask.CompletedTask;
        }

        public async UniTask HandleAsync(GameStartedEvent eventData)
        {
            if (eventData.PlayerIds.Length <= viewModel.Index)
            {
                return;
            }
            viewModel.SetCurrentTokenCount(0);
            viewModel.SetCurrentCardCount(0);
            viewModel.SetPlayerId(eventData.PlayerIds[viewModel.Index]);
            view.Initialize(viewModel.ResourceType, 0, 0);
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}