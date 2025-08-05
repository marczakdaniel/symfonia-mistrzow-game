using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.ReserveDeckCardWindow
{
    public class ReserveDeckCardOpenWindowAnimation : MonoBehaviour
    {
        [SerializeField]
        private RectTransform startCardPosition;

        [SerializeField]
        private RectTransform[] deckCardPositions = new RectTransform[3];

        [SerializeField]
        private AnimationSequencerController openWindowAnimation;

        public async UniTask PlayOpenWindowAnimation(int cardLevel)
        {
            SetStartCardPosition(cardLevel);
            await openWindowAnimation.PlayAsync();
        }

        private void SetStartCardPosition(int cardLevel)
        {
            startCardPosition.position = deckCardPositions[cardLevel - 1].position;
        }
    }
}