using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MusicCardGame.UI.CardPurchaseWindow 
{
    public class CardPurchaseWindowPurchaseAnimation : MonoBehaviour 
    {
        [SerializeField] private AnimationSequencerController purchaseAnimation;
        [SerializeField] private RectTransform endPosition;

        [SerializeField] private RectTransform[] playersPositions;

        public async UniTask PlayPurchaseAnimation(int playerIndex) {
            endPosition.position = GetPlayerPositions(playerIndex).position;
            await purchaseAnimation.PlayAsync();
        }

        private RectTransform GetPlayerPositions(int playerIndex) {
            return playersPositions[playerIndex];
        }
    }
}