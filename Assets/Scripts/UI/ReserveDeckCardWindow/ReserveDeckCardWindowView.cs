using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using UI.MusicCardDetailsPanel;
using UnityEngine;
using TMPro;

namespace UI.ReserveDeckCardWindow 
{
    public class ReserveDeckCardWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClick = new Subject<Unit>();
        public Subject<Unit> OnReserveButtonClick = new Subject<Unit>();

        
        [SerializeField] 
        private ButtonElement closeButton;

        [SerializeField]
        private ButtonElement reserveButton;
        
        [SerializeField]
        private TextMeshProUGUI cardLevelText;

        [SerializeField]
        private DetailsMusicCardView detailsMusicCardView;

        [SerializeField]
        private ReserveDeckCardOpenWindowAnimation openWindowAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void Setup(MusicCardData musicCardData)
        {
            detailsMusicCardView.Setup(musicCardData);
        }

        public void SetCardLevel(int cardLevel)
        {
            detailsMusicCardView.SetCardLevel(cardLevel);
            detailsMusicCardView.SetCanBePurchased(false);
            cardLevelText.text = cardLevel switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                _ => ""
            };
        }

        public async UniTask PlayOpenAnimation(int cardLevel)
        {
            await openWindowAnimation.PlayOpenWindowAnimation(cardLevel);
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }

        public void Awake()
        {
            closeButton.OnClick.Subscribe(OnCloseButtonClick.OnNext).AddTo(this);
            reserveButton.OnClick.Subscribe(OnReserveButtonClick.OnNext).AddTo(this);
        }
    }
}