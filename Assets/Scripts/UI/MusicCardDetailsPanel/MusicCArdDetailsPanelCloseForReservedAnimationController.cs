using UnityEngine;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;

namespace UI.MusicCardDetailsPanel {
    public class MusicCardDetailsPanelCloseForReservedAnimationController : MonoBehaviour {
        [SerializeField] private AnimationSequencerController closeForReservedAnimation;
        [SerializeField] private RectTransform endPosition;

        [SerializeField] private RectTransform[] playersPositions;

        public async UniTask PlayCloseForReservedAnimation(int playerIndex) {
            endPosition.position = GetPlayerPositions(playerIndex).position;
            await closeForReservedAnimation.PlayAsync();
        }

        private RectTransform GetPlayerPositions(int playerIndex) {
            return playersPositions[playerIndex];
        }
    }
}