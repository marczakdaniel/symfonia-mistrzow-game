using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;
using R3;
using UnityEngine;

namespace UI.Board.BoardEndTurnButton
{
    public class BoardEndTurnButtonView : MonoBehaviour
    {
        public Subject<Unit> OnButtonClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private ButtonElement button;
        [SerializeField] private AnimationSequencerController activeAnimation;
        [SerializeField] private AnimationSequencerController disabledAnimation;

        public void Awake()
        {
            button.OnClick.Subscribe(_ => OnButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public async UniTask PlayActiveAnimation()
        {
            await activeAnimation.PlayAsync();
        }

        public async UniTask PlayDisabledAnimation()
        {
            await disabledAnimation.PlayAsync();
        }
    }
}