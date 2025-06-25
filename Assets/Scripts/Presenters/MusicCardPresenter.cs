using DefaultNamespace.Models;
using DefaultNamespace.Views;
using R3;
using System;
using UnityEngine;

namespace DefaultNamespace.Presenters
{
    public class MusicCardPresenter : IDisposable
    {
        private readonly MusicCardView view;
        private readonly MusicCardModel model;
        private readonly CompositeDisposable subscriptions = new CompositeDisposable();

        public MusicCardPresenter(MusicCardView view, MusicCardModel model)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.model = model ?? throw new ArgumentNullException(nameof(model));

            InitializeMVP();
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            model.State.Subscribe(HandleStateChange).AddTo(subscriptions);
        }

        private void ConnectView()
        {
            view.OnCardClicked.Subscribe(HandleCardClick);
        }

        private void HandleCardClick(Unit unit)
        {
            Debug.Log("Card clicked");
        }

        private void HandleStateChange(MusicCardState state)
        {
            Debug.Log($"State changed to: {state}");
        }

        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}