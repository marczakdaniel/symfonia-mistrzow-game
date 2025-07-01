using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Views;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelPresenter : IDisposable {
        private readonly MusicCardDetailsPanelModel model;
        private readonly MusicCardDetailsPanelView view;

        public MusicCardDetailsPanelPresenter(MusicCardDetailsPanelModel model, MusicCardDetailsPanelView view) {
            this.model = model;
            this.view = view;

            ValidateModel();

            InitializeMVP();
        }

        private void ValidateModel() {
            if (model == null) {
                Debug.LogError("Model is null");
                return;
            }
        }

        private void InitializeMVP() {
            ConnectModel();
            ConnectView();
        }
        
        private void ConnectModel() {
            model.State.Subscribe(HandleStateChange);
        }

        private void HandleStateChange(MusicCardDetailsPanelState state) {
            // TODO : Handle state change
            Debug.Log($"State changed to: {state}");

            if (state == MusicCardDetailsPanelState.Inactive) {
                ActivatePanel().Forget();
            }
            else {
                DeactivatePanel().Forget();
            }
        }

        private async UniTask ActivatePanel() {
            // TODO : Activate the panel with animation
            view.SetCardDetails(model.MusicCardModel);
            await view.PlayOpenAnimation();
        }
        private async UniTask DeactivatePanel() {
            // TODO : Deactivate the panel with animation
            await view.PlayCloseAnimation();
        }

        private void ConnectView() {
            view.OnCloseButtonClick.Subscribe(CloseButtonClicked);
            view.OnBuyButtonClick.Subscribe(BuyButtonClicked);
            view.OnReserveButtonClick.Subscribe(ReserveButtonClicked);
        }

        private void CloseButtonClicked(Unit unit) {
            // TODO : Close the panel with animation
            Debug.Log("Close button clicked");
            model.DeactivatePanel();
        }

        private void BuyButtonClicked(Unit unit) {
            // TODO : Buy the card
            Debug.Log("Buy button clicked");

            // TODO : Buy the card

            model.DeactivatePanel();
        }

        private void ReserveButtonClicked(Unit unit) {
            // TODO : Reserve the card
            Debug.Log("Reserve button clicked");

            // TODO : Reserve the card

            model.DeactivatePanel();
        }

        public void Dispose() {
            view.OnCloseButtonClick.Dispose();
            view.OnBuyButtonClick.Dispose();
            view.OnReserveButtonClick.Dispose();
        }
    }
}