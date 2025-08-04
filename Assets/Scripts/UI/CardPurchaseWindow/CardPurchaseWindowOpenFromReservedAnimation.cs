using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.CardPurchaseWindow
{
    public class CardPurchaseWindowOpenFromReservedAnimation : MonoBehaviour
    {
        [SerializeField] private AnimationSequencerController openAnimation;
        [SerializeField] private RectTransform[] cardPositions;
        [SerializeField] private RectTransform endPosition;

        public async UniTask PlayOpenAnimation(int cardIndex)
        {
            Debug.Log($"Playing open animation for card index: {cardIndex}");
            endPosition.position = GetCardPosition(cardIndex).position;
            await openAnimation.PlayAsync();
        }

        private RectTransform GetCardPosition(int cardIndex)
        {
            return cardPositions[cardIndex];
        }
    }
}