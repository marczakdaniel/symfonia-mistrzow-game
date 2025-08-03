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

        public async UniTask PlayOpenAnimation() {
            await openAnimation.PlayAsync();
        }
        public async UniTask PlayOpenFromBoardAnimation(int level, int position) {
            await openFromBoardAnimation.PlayOpenFromBoardAnimation(level, position);
        }

        public async UniTask PlayCloseToBoardAnimation() {
            await closeToBoardAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation() {
            await closeAnimation.PlayAsync();
        }

        public async UniTask PlayCloseForReservedAnimation(int playerIndex) {
            await closeForReservedAnimation.PlayCloseForReservedAnimation(playerIndex);
        }
    }
}