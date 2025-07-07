using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelView : MonoBehaviour {
        public Subject<Unit> OnCloseButtonClick = new();
        public Subject<Unit> OnBuyButtonClick = new();
        public Subject<Unit> OnReserveButtonClick = new();
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private TextMeshProUGUI cardDescription;
        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement buyButton;
        [SerializeField] private ButtonElement reserveButton;
        [SerializeField] private DetailsMusicCardView detailsMusicCardView;


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
        public void SetCardDetails(MusicCardData musicCardData) {
            if (musicCardData == null) {
                return;
            }
            cardName.text = musicCardData.cardName;
            cardDescription.text = musicCardData.cardDescription;
            detailsMusicCardView.Setup(musicCardData);
        }

        public UniTask PlayOpenAnimation() {
            return UniTask.CompletedTask;
        }
        public UniTask PlayCloseAnimation() {
            return UniTask.CompletedTask;
        }
        public UniTask PlayBuyAnimation() {
            return UniTask.CompletedTask;
        }
        public UniTask PlayReserveAnimation() {
            return UniTask.CompletedTask;
        }

        public void EnablePanel(){
            gameObject.SetActive(true);
        }
        public void DisablePanel(){
            gameObject.SetActive(false);
        }
    }
}