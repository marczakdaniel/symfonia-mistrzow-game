using Command;
using Cysharp.Threading.Tasks;
using Events;
using DefaultNamespace.Data;
using Models;
using R3;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelPresenter : IAsyncEventHandler<MusicCardDetailsPanelOpenedEvent>, IAsyncEventHandler<MusicCardDetailsPanelClosedEvent> {
        private readonly MusicCardDetailsPanelView view;
        private readonly MusicCardDetailsPanelViewModel viewModel = new MusicCardDetailsPanelViewModel();
        private readonly CommandFactory commandFactory;
        private readonly IGameModelReader gameModelReader;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public MusicCardDetailsPanelPresenter(MusicCardDetailsPanelView view, CommandFactory commandFactory, IGameModelReader gameModelReader) {
            this.view = view;
            this.commandFactory = commandFactory;

            InitializeMVP();
            SubscribeToEvents();
        }

        private void InitializeMVP() {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel() {
            viewModel.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(subscriptions);
            viewModel.MusicCardData.Where(data => data != null).Subscribe(data => view.SetCardDetails(data)).AddTo(subscriptions);
        }

        private void ConnectView() {
            view.OnCloseButtonClick.Subscribe(_ => HandleCloseButtonClick().Forget()).AddTo(subscriptions);
            view.OnBuyButtonClick.Subscribe(HandleBuyButtonClick).AddTo(subscriptions);
            view.OnReserveButtonClick.Subscribe(HandleReserveButtonClick).AddTo(subscriptions);
        }

        private void SubscribeToEvents() {
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelClosedEvent>(this);
        }

        private async UniTask HandleStateChange(MusicCardDetailsPanelState state) {
            if (state == MusicCardDetailsPanelState.DuringOpenAnimation) {
                await view.PlayOpenAnimation();
                viewModel.CompleteOpenAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringCloseAnimation) {
                await view.PlayCloseAnimation();
                viewModel.CompleteCloseAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringBuyAnimation) {
                await view.PlayBuyAnimation();
                viewModel.CompleteBuyAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringReserveAnimation) {
                await view.PlayReserveAnimation();
                viewModel.CompleteReserveAnimation();
            }
            else if (state == MusicCardDetailsPanelState.Opened) {
                view.EnablePanel();
            }
            else if (state == MusicCardDetailsPanelState.Closed) {
                view.DisablePanel();
            }
            else {
                Debug.LogError($"Unknown state: {state}");
            }
        }

        // Commands Actions

        private async UniTask HandleCloseButtonClick() {
            var command = commandFactory.CreateCloseMusicCardDetailsPanelCommand();
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private void HandleBuyButtonClick(Unit unit) 
        {
            var command = commandFactory.CreateBuyMusicCardCommand(viewModel.PlayerId, viewModel.MusicCardId);
        }

        private void HandleReserveButtonClick(Unit unit) {
            var command = commandFactory.CreateReserveMusicCardCommand(viewModel.PlayerId, viewModel.MusicCardId);
        }

        // Event Bus

        public async UniTask HandleAsync(MusicCardDetailsPanelOpenedEvent musicCardDetailsPanelOpenedEvent) {
            var musicCardData = MusicCardRepository.Instance.GetCard(musicCardDetailsPanelOpenedEvent.MusicCardId);
            viewModel.OpenCardDetailsPanel(musicCardDetailsPanelOpenedEvent.MusicCardId, musicCardData);

            await UniTask.WaitUntil(() => viewModel.State.Value == MusicCardDetailsPanelState.Opened);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelClosedEvent musicCardDetailsPanelClosedEvent) {  
            viewModel.CloseCardDetailsPanel();

            await UniTask.WaitUntil(() => viewModel.State.Value == MusicCardDetailsPanelState.Closed);
        }
    }
}