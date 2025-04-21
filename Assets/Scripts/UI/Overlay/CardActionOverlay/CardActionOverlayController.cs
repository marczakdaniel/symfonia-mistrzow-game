using System;
using UnityEngine;

namespace DefaultNamespace.UI.CardActionOverlay
{
    public class CardActionOverlayController
    {
        private CardActionOverlayModel Model;
        private CardActionOverlayView View;
        
        public Action<CardData> OnCardBuy;
        public Action<CardData> OnCardReserve;

        public CardActionOverlayController(CardActionOverlayModel model, CardActionOverlayView view)
        {
            Model = model;
            View = view;

            InitializeController();
            View.Setup(model.CardData);
            View.gameObject.SetActive(true);
        }

        private void InitializeController()
        {
            View.OnCardBuy += HandleCardBuy;
            View.OnCardReserved += HandleCardReserved;
            View.OnBlendClicked += HandleBlendClicked;
        }

        private void HandleBlendClicked()
        {
            View.gameObject.SetActive(false);
        }

        private void HandleCardBuy()
        {
            OnCardBuy?.Invoke(Model.CardData);
            Debug.LogError($"Card Action Buy");
        }
        
        private void HandleCardReserved()
        {
            OnCardReserve?.Invoke(Model.CardData);
            Debug.LogError($"Card Action Reserved");
        }
    }
}