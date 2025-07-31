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

        public async UniTask PlayOpenAnimation() {
        }

        /// <summary>
        /// Odtwarza animację otwierania okna z określonym poziomem i pozycją
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public async UniTask PlayOpenAnimation(int level, int position) {
        }
        public async UniTask PlayCloseAnimation() {
        }

        /// <summary>
        /// Odtwarza animację zamykania okna z określonym poziomem i pozycją
        /// </summary>
        /// <param name="level">Poziom (0-2)</param>
        /// <param name="position">Pozycja na poziomie (0-3)</param>
        public async UniTask PlayCloseAnimation(int level, int position) {
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