using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.DeckCardInfoWindow
{
    public class DeckCardInfoWindowCloseAnimation : MonoBehaviour
    {
        [SerializeField]
        private AnimationSequencerController closeAnimation;

        [SerializeField]
        private RectTransform cardEndPosition;

        [SerializeField]
        private RectTransform[] playerPositions = new RectTransform[4];

        public async UniTask PlayCloseAnimation(int playerIndex)
        {
            SetCardEndPosition(playerIndex);
            await closeAnimation.PlayAsync();
        }

        private void SetCardEndPosition(int playerIndex)
        {
            cardEndPosition.position = playerPositions[playerIndex].position;
        }
    }
}