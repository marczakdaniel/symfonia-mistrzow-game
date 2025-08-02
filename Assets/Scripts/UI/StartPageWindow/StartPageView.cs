using BrunoMikoski.AnimationSequencer;
using DefaultNamespace.Elements;
using R3;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.StartPageWindow
{
    public class StartPageWindowView : MonoBehaviour
    {
        public Subject<Unit> OnPlayButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnTestButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnManualButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] 
        private ButtonElement playButton;

        [SerializeField]
        private ButtonElement testButton;

        [SerializeField]
        private ButtonElement manualButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void Awake()
        {
            playButton.OnClick.Subscribe(_ => OnPlayButtonClicked.OnNext(Unit.Default)).AddTo(this);
            testButton.OnClick.Subscribe(_ => OnTestButtonClicked.OnNext(Unit.Default)).AddTo(this);
            manualButton.OnClick.Subscribe(_ => OnManualButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }
    }
}