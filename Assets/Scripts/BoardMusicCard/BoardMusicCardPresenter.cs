using DefaultNamespace.Models;
using DefaultNamespace.Views;
using R3;
using System;
using UnityEngine;

namespace DefaultNamespace.Presenters
{
    public class BoardMusicCardPresenter : IDisposable
    {
        private readonly BoardMusicCardView view;
        private readonly BoardMusicCardModel model;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public BoardMusicCardPresenter(BoardMusicCardView view, BoardMusicCardModel model)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            ValidateModel();

            InitializeMVP();
        }

        private void ValidateModel()
        {
            if (model == null) {
                Debug.LogError("Model is null");
                return;
            }

            if (model.MusicCardModel.State.CurrentValue != MusicCardState.OnBoard) {
                Debug.LogError("Model is not on board");
                return;
            }
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            model.MusicCardModel.State.Subscribe(HandleStateChange).AddTo(subscriptions);
        }

        private void ConnectView()
        {
            view.OnCardClicked.Subscribe(HandleCardClick).AddTo(subscriptions);
        }

        private void HandleCardClick(Unit unit)
        {
            // TODO : Open card details with animation
            model.MusicCardDetailsPanelModel.ActivatePanel(model.MusicCardModel);
            Debug.Log("Card clicked");
        }

        private void HandleStateChange(MusicCardState state)
        {
            // TODO: Handle state change
            // Possible states: 
            // - Reserved
            // - InPlayerResources
            // Also handle if action was made by current player
            // Also animations of state change




            Debug.Log($"State changed to: {state}");
        }

        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}