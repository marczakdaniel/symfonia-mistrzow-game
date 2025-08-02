using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;

namespace UI.InfoWindow
{
    public class InfoWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        [SerializeField] 
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void SetDescription(string description)
        {
            descriptionText.text = description;
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }

        public void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }
}