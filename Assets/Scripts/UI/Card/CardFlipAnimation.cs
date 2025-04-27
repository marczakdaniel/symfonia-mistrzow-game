using System;
using DG.Tweening;
using UnityEngine;

namespace DefaultNamespace.UI.Card
{
    public class CardFlipAnimation : MonoBehaviour
    {
        public float flipDuration = 0.5f;
        
        [SerializeField] private GameObject frontSide;
        [SerializeField] private GameObject backSide;
        [SerializeField] private RectTransform rectTransform;
        
        private bool isFront = true;
        private bool isFlipping = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Flip();
        }

        public void Flip()
        {
            if (isFlipping) return;
            isFlipping = true;

            Vector2 originalPosition = rectTransform.anchoredPosition;

            // Równoczesne skalowanie i podskok
            Sequence flipSequence = DOTween.Sequence();

            // Zmniejsz skalę X do 0 + podskocz w górę
            flipSequence.Append(rectTransform.DOScaleX(0f, flipDuration / 2).SetEase(Ease.InCubic));
            flipSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y + 20, flipDuration / 4).SetEase(Ease.OutQuad));

            flipSequence.AppendCallback(() =>
            {
                // Zmiana strony
                isFront = !isFront;
                frontSide.SetActive(isFront);
                backSide.SetActive(!isFront);
            });

            // Zwiększ skalę X z powrotem + wróć na dół
            flipSequence.Append(rectTransform.DOScaleX(1f, flipDuration / 2).SetEase(Ease.OutCubic));
            flipSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, flipDuration / 4).SetEase(Ease.InQuad));

            flipSequence.OnComplete(() => isFlipping = false);
        }
    }
}