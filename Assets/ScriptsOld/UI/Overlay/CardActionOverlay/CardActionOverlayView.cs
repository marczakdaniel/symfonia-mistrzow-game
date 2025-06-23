using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI.CardActionOverlay
{
    public class CardActionOverlayView : MonoBehaviour
    {
        [SerializeField] private CardView cardView;
        [SerializeField] private CardActionElement cardActionElement;
        [SerializeField] private Button blend;
        
        public Action OnCardBuy;
        public Action OnCardReserved;
        public Action OnBlendClicked;

        public void Setup(CardData cardData)
        {
            cardView.Setup(cardData);
            cardActionElement.SetAction(true);
        }
        
        public void OnEnable()
        {
            cardActionElement.OnBuyButtonClick += HandleBuyButtonClick;
            cardActionElement.OnReserveButtonClick += HandleReserveButtonClick;
            blend.onClick.AddListener(OnBlendClicked.Invoke);
        }

        public void OnDisable()
        {
            cardActionElement.OnBuyButtonClick -= HandleBuyButtonClick;
            cardActionElement.OnReserveButtonClick -= HandleReserveButtonClick;
            blend.onClick.RemoveListener(OnBlendClicked.Invoke);
        }
        
        private void HandleReserveButtonClick()
        {
            OnCardReserved.Invoke();
        }

        private void HandleBuyButtonClick()
        {
            OnCardBuy.Invoke();
        }
    }
}