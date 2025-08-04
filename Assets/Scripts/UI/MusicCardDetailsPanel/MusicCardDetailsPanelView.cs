using BrunoMikoski.AnimationSequencer;
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
        [SerializeField] private AnimationSequencerController openAnimation;
        [SerializeField] private MusicCardDetailsPanelOpenFromBoardAnimationController openFromBoardAnimation;
        [SerializeField] private MusicCardDetailsPanelCloseForReservedAnimationController closeForReservedAnimation;
        [SerializeField] private AnimationSequencerController closeToBoardAnimation;
        [SerializeField] private AnimationSequencerController closeAnimation;
        [SerializeField] private AnimationSequencerController moveToCardPurchaseWindow;
        [SerializeField] private AnimationSequencerController moveFromCardPurchaseWindow;

        private void Awake() {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClick.OnNext(Unit.Default)).AddTo(this);
            buyButton.OnClick.Subscribe(_ => OnBuyButtonClick.OnNext(Unit.Default)).AddTo(this);
            reserveButton.OnClick.Subscribe(_ => OnReserveButtonClick.OnNext(Unit.Default)).AddTo(this);
        }

        public void SetCardDetails(MusicCardData musicCardData) {
            if (musicCardData == null) {
                return;
            }
            cardName.text = musicCardData.cardName;
            cardDescription.text = musicCardData.cardDescription;
            detailsMusicCardView.Setup(musicCardData);
        }

        public void SetCanBePurchased(bool canBePurchased) {
            detailsMusicCardView.SetCanBePurchased(canBePurchased);
        }

        public async UniTask PlayOpenAnimation() {
            await openAnimation.PlayAsync();
        }
        public async UniTask PlayOpenFromBoardAnimation(int level, int position) {
            await openFromBoardAnimation.PlayOpenFromBoardAnimation(level, position);
        }

        public async UniTask PlayCloseToBoardAnimation() {
            await closeToBoardAnimation.PlayAsync();
            PlayCloseAnimation(50).Forget();
        }

        public async UniTask PlayCloseAnimation(int delay = 0) {
            await UniTask.Delay(delay);
            await closeAnimation.PlayAsync();
        }

        public async UniTask PlayCloseForReservedAnimation(int playerIndex, int delay = 0) {
            await closeForReservedAnimation.PlayCloseForReservedAnimation(playerIndex);
        }

        public async UniTask PlayMoveToCardPurchaseWindowAnimation() {
            await moveToCardPurchaseWindow.PlayAsync();
        }

        public async UniTask PlayMoveFromCardPurchaseWindowAnimation() {
            await moveFromCardPurchaseWindow.PlayAsync();
        }
    }
}