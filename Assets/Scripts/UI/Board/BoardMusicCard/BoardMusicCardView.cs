using System;
using R3;
using UnityEngine;
using DefaultNamespace.Data;
using Cysharp.Threading.Tasks;
using UI.MusicCardDetailsPanel;
using BrunoMikoski.AnimationSequencer;
using UnityEngine.UI;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class BoardMusicCardView : MonoBehaviour
    {
        public Subject<Unit> OnCardClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] 
        private DetailsMusicCardView detailsMusicCardView;

        [SerializeField] 
        private AnimationSequencerController revealAnimation;

        [SerializeField]
        private AnimationSequencerController revealAnimationPart2;

        [SerializeField]
        private AnimationSequencerController simpleShowAnimation;


        [SerializeField]
        private AnimationSequencerController hideAnimation;


        public void Setup(MusicCardData musicCardData)
        {
            detailsMusicCardView.Setup(musicCardData);
        }

        public async UniTask PlayRevealAnimation()
        {
            await revealAnimation.PlayAsync();
            await revealAnimationPart2.PlayAsync();
        }

        public async UniTask PlaySimpleShowAnimation()
        {
            await simpleShowAnimation.PlayAsync();
        }

        public async UniTask PlayHideAnimation(int delay = 0)
        {
            await UniTask.Delay(delay);
            await hideAnimation.PlayAsync();
        }

        public void Awake()
        {
            detailsMusicCardView.OnCardClicked.Subscribe(_ => OnCardClicked.OnNext(Unit.Default)).AddTo(this);
        }


    }
}