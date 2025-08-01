using System;
using Command;
using Cysharp.Threading.Tasks;
using Events;
using R3;
using UnityEngine;

namespace Assets.Scripts.UI.ConcertCardsWindow
{
    public class ConcertCardsWindowPresenter : 
        IDisposable,
        IAsyncEventHandler<ConcertCardsWindowOpenedEvent>,
        IAsyncEventHandler<ConcertCardsWindowClosedEvent>
    {
        private readonly ConcertCardsWindowView view;
        private readonly CommandFactory commandFactory;

        private IDisposable disposable;

        public ConcertCardsWindowPresenter(ConcertCardsWindowView view, CommandFactory commandFactory)
        {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP()
        {
            var d = Disposable.CreateBuilder();

            ConnectView(d);

            disposable = d.Build();
        }

        private void ConnectView(DisposableBuilder d)
        {
            view.OnCloseButtonClicked.Subscribe(_ => HandleCloseButtonClicked().ToObservable()).AddTo(ref d);
        }

        private async UniTask HandleCloseButtonClicked()
        {
            var command = commandFactory.CreateCloseConcertCardsWindowCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void SubscribeToEvents()
        {
            AsyncEventBus.Instance.Subscribe<ConcertCardsWindowOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<ConcertCardsWindowClosedEvent>(this);
        }

        public async UniTask HandleAsync(ConcertCardsWindowOpenedEvent gameEvent)
        {
            view.Initialize(gameEvent.ConcertCards, gameEvent.CardStates);
            await view.PlayOpenAnimation();
        }

        public async UniTask HandleAsync(ConcertCardsWindowClosedEvent gameEvent)
        {
            await view.PlayCloseAnimation();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}