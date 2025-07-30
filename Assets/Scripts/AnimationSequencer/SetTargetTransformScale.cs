
using System;
using BrunoMikoski.AnimationSequencer;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections.Generic;
namespace AnimationSequencer
{
    [Serializable]
    public class SetTargetsTransformScale : AnimationStepBase
    {
        [SerializeField]
        private Transform[] targets;

        [SerializeField] 
        private Vector3 scale;

        public override string DisplayName => "Set Targets Transform Scale";
        private List<Vector3> initialScales = new List<Vector3>();

        public override void ResetToInitialState()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (initialScales.Count > i)
                {
                    targets[i].localScale = initialScales[i];
                }
            }
        }

        public override void AddTweenToSequence(Sequence animationSequence)
        {
            animationSequence.AppendInterval(Delay);
            animationSequence.AppendCallback(() => {
                for (int i = 0; i < targets.Length; i++)
                {
                    initialScales.Add(targets[i].localScale);
                    targets[i].localScale = scale;
                }
            });
            animationSequence.AppendInterval(0f);
        }
    }
}