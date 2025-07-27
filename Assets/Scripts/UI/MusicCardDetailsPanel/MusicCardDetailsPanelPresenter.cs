using Command;
using Cysharp.Threading.Tasks;
using Events;
using DefaultNamespace.Data;
using Models;
using R3;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelPresenter : 
        IAsyncEventHandler<MusicCardDetailsPanelOpenedEvent>, 
        IAsyncEventHandler<MusicCardDetailsPanelClosedEvent>,
        IAsyncEventHandler<CardReservedEvent>
    {
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
            view.OnBuyButtonClick.Subscribe(_ => HandleBuyButtonClick().Forget()).AddTo(subscriptions);
            view.OnReserveButtonClick.Subscribe(_ => HandleReserveButtonClick().Forget()).AddTo(subscriptions);
        }

        private async UniTask HandleStateChange(MusicCardDetailsPanelState state) {
            if (state == MusicCardDetailsPanelState.DuringOpenAnimation) {
                view.EnablePanel();
                await view.PlayOpenAnimation(viewModel.Level - 1, viewModel.Position);
                viewModel.CompleteOpenAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringCloseAnimation) {
                await view.PlayCloseAnimation(viewModel.Level - 1, viewModel.Position);
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
            var command = commandFactory.CreateCloseMusicCardDetailsPanelCommand(viewModel.MusicCardId);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }
        private async UniTask HandleBuyButtonClick() 
        {
            //var command = commandFactory.CreateBuyMusicCardCommand(viewModel.PlayerId, viewModel.MusicCardId);
            //await CommandService.Instance.ExecuteCommandAsync(command);
        }

        private async UniTask HandleReserveButtonClick() {
            var command = commandFactory.CreateReserveCardCommand(viewModel.MusicCardId);
            await CommandService.Instance.ExecuteCommandAsync(command);
        }

        // Event Bus

        private void SubscribeToEvents() {
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelOpenedEvent>(this);
            AsyncEventBus.Instance.Subscribe<MusicCardDetailsPanelClosedEvent>(this, EventPriority.High);
            AsyncEventBus.Instance.Subscribe<CardReservedEvent>(this);
        }


        public async UniTask HandleAsync(MusicCardDetailsPanelOpenedEvent musicCardDetailsPanelOpenedEvent) {
            var musicCardData = MusicCardRepository.Instance.GetCard(musicCardDetailsPanelOpenedEvent.MusicCardId);
            viewModel.OpenCardDetailsPanel(musicCardDetailsPanelOpenedEvent.MusicCardId, musicCardData, musicCardDetailsPanelOpenedEvent.Level, musicCardDetailsPanelOpenedEvent.Position);

            await UniTask.WaitUntil(() => viewModel.State.Value == MusicCardDetailsPanelState.Opened);
        }

        public async UniTask HandleAsync(MusicCardDetailsPanelClosedEvent musicCardDetailsPanelClosedEvent) {  
            viewModel.CloseCardDetailsPanel();

            await UniTask.WaitUntil(() => viewModel.State.Value == MusicCardDetailsPanelState.Closed);
        }

        public async UniTask HandleAsync(CardReservedEvent cardReservedEvent) {
            viewModel.CloseCardDetailsPanel();

            await UniTask.WaitUntil(() => viewModel.State.Value == MusicCardDetailsPanelState.Closed);
        }
    }
}