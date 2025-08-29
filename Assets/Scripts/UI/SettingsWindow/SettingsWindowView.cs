using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using UnityEngine;

namespace UI.SettingsWindow
{
    public class SettingsWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked = new Subject<Unit>();
        public Subject<Unit> OnRestartButtonClicked = new Subject<Unit>();
        public Subject<Unit> OnMusicButtonClicked = new Subject<Unit>();
        [SerializeField]
        private ButtonElement closeButton;
        
        [SerializeField]
        private ButtonElement restartButton;

        [SerializeField]
        private ButtonElement musicButton;

        [SerializeField]
        private AnimationSequencerController openedAnimation;
        
        [SerializeField]
        private AnimationSequencerController closedAnimation;

        public void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
            restartButton.OnClick.Subscribe(_ => OnRestartButtonClicked.OnNext(Unit.Default)).AddTo(this);
            musicButton.OnClick.Subscribe(_ => OnMusicButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask PlayOpenedAnimation()
        {
            await openedAnimation.PlayAsync();
        }

        public async UniTask PlayClosedAnimation()
        {
            await closedAnimation.PlayAsync();
        }
    }
}