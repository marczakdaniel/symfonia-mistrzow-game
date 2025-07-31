using System.Xml.Serialization;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using Mono.Cecil.Cil;
using R3;
using UI.CardPurchaseWindow.CardPurchaseSingleToken;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.CardPurchaseWindow
{
    public class CardPurchaseWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClick = new();
        public Subject<Unit> OnConfirmButtonClick = new();

        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement confirmButton;
        [SerializeField] private DetailsMusicCardView musicCardView;
        [SerializeField] private CardPurchaseSingleTokenView[] cardPurchaseSingleTokenViews = new CardPurchaseSingleTokenView[6];

        public CardPurchaseSingleTokenView[] CardPurchaseSingleTokenViews => cardPurchaseSingleTokenViews;

        public void Awake()
        {
            closeButton.OnClick.Subscribe(OnCloseButtonClick.OnNext).AddTo(this);
            confirmButton.OnClick.Subscribe(OnConfirmButtonClick.OnNext).AddTo(this);
        }
        

        public void SetCardDetails(MusicCardData musicCardData)
        {
            musicCardView.Setup(musicCardData);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}