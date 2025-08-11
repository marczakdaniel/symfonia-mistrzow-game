using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace UI.ResultWindow
{
    public class ResultWindowView : MonoBehaviour
    {
        public Subject<int> OnPlayerClicked = new Subject<int>();

        [SerializeField]
        private ResultPlayerElement[] playerElements = new ResultPlayerElement[4];

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        public void Setup(
            List<string> playersName,
            List<int> playersPoints,
            List<Sprite> playersAvatars
        )
        {
            for (int i = 0; i < playersName.Count; i++)
            {
                playerElements[i].Setup(playersName[i], playersPoints[i], playersAvatars[i]);
                playerElements[i].gameObject.SetActive(true);
            }

            for (int i = playersName.Count; i < playerElements.Length; i++)
            {
                playerElements[i].gameObject.SetActive(false);
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
            for (int i = 0; i < playerElements.Length; i++)
            {
                playerElements[i].OnClicked.Subscribe(_ => OnPlayerClicked.OnNext(i)).AddTo(this);
            }
        }
    }
}