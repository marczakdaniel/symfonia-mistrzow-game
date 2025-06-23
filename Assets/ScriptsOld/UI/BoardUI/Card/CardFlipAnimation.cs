using System;
using Cysharp.Threading.Tasks;
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
        
        private bool isFront = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Play().Forget();
        }

        public async UniTask Play()
        {
            var isFinished = false;


            Vector2 originalPosition = rectTransform.anchoredPosition;

            Sequence flipSequence = DOTween.Sequence();

            flipSequence.Append(rectTransform.DOScaleX(0f, flipDuration / 2).SetEase(Ease.InCubic));
            flipSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y + 20, flipDuration / 4).SetEase(Ease.OutQuad));

            flipSequence.AppendCallback(() =>
            {
                isFront = !isFront;
                frontSide.SetActive(true);
                backSide.SetActive(false);
            });

            // Zwiększ skalę X z powrotem + wróć na dół
            flipSequence.Append(rectTransform.DOScaleX(1f, flipDuration / 2).SetEase(Ease.OutCubic));
            flipSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, flipDuration / 4).SetEase(Ease.InQuad));

            flipSequence.OnComplete(() => isFinished = true);
            await UniTask.WaitUntil(() => isFinished);
        }
    }
}