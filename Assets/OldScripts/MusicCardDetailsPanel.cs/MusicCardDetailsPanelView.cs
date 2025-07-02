using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using DefaultNamespace.Models;
using R3;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelView : MonoBehaviour 
    {
        public Subject<Unit> OnCloseButtonClick = new();
        public Subject<Unit> OnBuyButtonClick = new();
        public Subject<Unit> OnReserveButtonClick = new();
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;
        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement buyButton;
        [SerializeField] private ButtonElement reserveButton;
        //[SerializeField] private DetailsMusicCardView detailsMusicCardView;

        [SerializeField] private MusicCardDetailsPanelAnimationController animationController;

        private void Awake() {
            closeButton.OnClick.Subscribe(CloseButtonClicked).AddTo(this);
            buyButton.OnClick.Subscribe(BuyButtonClicked).AddTo(this);
            reserveButton.OnClick.Subscribe(ReserveButtonClicked).AddTo(this);
        }
        private void CloseButtonClicked(Unit unit) {
            OnCloseButtonClick.OnNext(unit);
        }
        private void BuyButtonClicked(Unit unit) {
            OnBuyButtonClick.OnNext(unit);
        }
        private void ReserveButtonClicked(Unit unit) {
            OnReserveButtonClick.OnNext(unit);
        }
        public void SetCardDetails(MusicCardModel musicCardModel) {
            cardName.text = musicCardModel.CardName ;
            cardDescription.text = musicCardModel.CardDescription;
            //detailsMusicCardView.Setup(musicCardModel.Data);
        }

        public UniTask PlayOpenAnimation() {
            return animationController.PlayOpenAnimation();
        }
        public UniTask PlayCloseAnimation() {
            return animationController.PlayCloseAnimation();
        }
    }
}