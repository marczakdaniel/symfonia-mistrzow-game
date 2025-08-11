using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.ResultPlayerResources
{
    public class ResultPlayerResourcesView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked = new Subject<Unit>();

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private RectTransform cardContainer;

        [SerializeField]
        private DetailsMusicCardView detailsMusicCardView;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void Setup(List<MusicCardData> musicCardDatas)
        {
            foreach (var musicCardData in musicCardDatas)
            {
                var cardView = Instantiate(detailsMusicCardView, cardContainer);
                cardView.Setup(musicCardData);
            }
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
            closeButton.OnClick.Subscribe(OnCloseButtonClicked.OnNext).AddTo(this);
        }
    }
}