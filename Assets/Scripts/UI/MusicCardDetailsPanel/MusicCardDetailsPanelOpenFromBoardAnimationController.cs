using UnityEngine;

using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelOpenFromBoardAnimationController : MonoBehaviour {
        [SerializeField] private AnimationSequencerController openFromBoardAnimation;
        [SerializeField] private RectTransform startPosition;
        [SerializeField] private RectTransform[] level1Positions;
        [SerializeField] private RectTransform[] level2Positions;
        [SerializeField] private RectTransform[] level3Positions;

        public async UniTask PlayOpenFromBoardAnimation(int level, int position) {
            startPosition.position = GetStartPosition(level, position).position;
            await openFromBoardAnimation.PlayAsync();
        }

        private RectTransform GetStartPosition(int level, int position) {
            switch (level) {
                case 1:
                    return level1Positions[position];
                case 2:
                    return level2Positions[position];
                case 3:
                    return level3Positions[position];
                default:
                    return startPosition;
            }
        }
    }
}