using BrunoMikoski.AnimationSequencer;
using Coffee.UIEffects;
using UnityEngine;
using DG.Tweening;

namespace AnimationSequencer
{
    public class PlayUIEffectTweener : AnimationStepBase
    {
        [SerializeField]
        private UIEffectTweener uiEffectTweener;

        [SerializeField]
        private bool playForward;

        public override string DisplayName => "Play UI Effect Tweener";

        public override void ResetToInitialState()
        {
            if (playForward)
            {
                uiEffectTweener.SetTime(uiEffectTweener.duration);
            }
            else
            {
                uiEffectTweener.SetTime(0f);
            }
        }

        public override void AddTweenToSequence(Sequence animationSequence)
        {
            animationSequence.AppendInterval(Delay);
            animationSequence.AppendCallback(() => {
                if (playForward)
                {
                    uiEffectTweener.SetTime(0f);
                    uiEffectTweener.PlayForward(true);
                }
                else
                {
                    uiEffectTweener.SetTime(uiEffectTweener.duration);
                    uiEffectTweener.PlayReverse();
                }
            });
            animationSequence.AppendInterval(uiEffectTweener.duration);
        }
    }
}