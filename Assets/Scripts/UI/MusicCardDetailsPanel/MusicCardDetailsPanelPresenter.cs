using Command;
using Cysharp.Threading.Tasks;
using Manager;
using R3;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelPresenter {
        private readonly MusicCardDetailsPanelView view;
        private readonly MusicCardDetailsPanelViewModel model;
        private readonly ICommandManager commandManager;
        private readonly CommandFactory commandFactory;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public MusicCardDetailsPanelPresenter(MusicCardDetailsPanelView view, MusicCardDetailsPanelViewModel model, ICommandManager commandManager, CommandFactory commandFactory) {
            this.view = view;
            this.model = model;
            this.commandManager = commandManager;
            this.commandFactory = commandFactory;
            InitializeMVP();
        }

        private void InitializeMVP() {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel() {
            model.State.Subscribe(state => HandleStateChange(state).Forget()).AddTo(subscriptions);
            model.MusicCardData.Subscribe(data => view.SetCardDetails(data)).AddTo(subscriptions);
        }

        private void ConnectView() {
            view.OnCloseButtonClick.Subscribe(HandleCloseButtonClick).AddTo(subscriptions);
            view.OnBuyButtonClick.Subscribe(HandleBuyButtonClick).AddTo(subscriptions);
            view.OnReserveButtonClick.Subscribe(HandleReserveButtonClick).AddTo(subscriptions);
        }

        private async UniTask HandleStateChange(MusicCardDetailsPanelState state) {
            if (state == MusicCardDetailsPanelState.DuringOpenAnimation) {
                await view.PlayOpenAnimation();
                model.CompleteOpenAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringCloseAnimation) {
                await view.PlayCloseAnimation();
                model.CompleteCloseAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringBuyAnimation) {
                await view.PlayBuyAnimation();
                model.CompleteBuyAnimation();
            }
            else if (state == MusicCardDetailsPanelState.DuringReserveAnimation) {
                await view.PlayReserveAnimation();
                model.CompleteReserveAnimation();
            }
            else if (state == MusicCardDetailsPanelState.Opened) {
            }
            else if (state == MusicCardDetailsPanelState.Closed) {
            }
            else {
                Debug.LogError($"Unknown state: {state}");
            }
        }

        // Commands Actions

        private void HandleCloseButtonClick(Unit unit) {
            
        }

        private void HandleBuyButtonClick(Unit unit) 
        {
            var command = commandFactory.CreateBuyMusicCardCommand(model.PlayerId, model.MusicCardId);
            commandManager.ExecuteCommand(command);
        }

        private void HandleReserveButtonClick(Unit unit) {
            var command = commandFactory.CreateReserveMusicCardCommand(model.PlayerId, model.MusicCardId);
            commandManager.ExecuteCommand(command);
        }
    }
}