using DefaultNamespace.Models;
using DefaultNamespace.Views;
using R3;
using UnityEngine;

namespace DefaultNamespace.Presenters
{
    public class MusicCardPresenter
    {
        private readonly MusicCardView view;
        private readonly MusicCardModel model;

        public MusicCardPresenter(MusicCardView view, MusicCardModel model)
        {
            this.view = view;
            this.model = model;

            InitializeMVP();
        }

        private void InitializeMVP()
        {
            ConnectModel();
            ConnectView();
        }

        private void ConnectModel()
        {
            model.State.Subscribe(HandleStateChange);
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
            Debug.Log("State changed to: " + state);
        }
    }
}