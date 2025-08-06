using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.DeckCardInfoWindow
{
    public class DeckCardInfoWindowView : MonoBehaviour
    {
        public Subject<Unit> OnAcceptButtonClicked = new Subject<Unit>();

        [SerializeField]
        private DetailsMusicCardView detailsMusicCardView;

        [SerializeField]
        private TextMeshProUGUI titleText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField]
        private ButtonElement acceptButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private DeckCardInfoWindowCloseAnimation closeAnimation;

        public void Setup(MusicCardData musicCardData)
        {
            detailsMusicCardView.SetCardFront(true);
            detailsMusicCardView.Setup(musicCardData);
            titleText.text = musicCardData.CardName;
            descriptionText.text = musicCardData.CardDescription;
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation(int playerIndex)
        {
            await closeAnimation.PlayCloseAnimation(playerIndex);
        }

        public void Awake()
        {
            acceptButton.OnClick.Subscribe(OnAcceptButtonClicked.OnNext).AddTo(this);
        }
    }
}