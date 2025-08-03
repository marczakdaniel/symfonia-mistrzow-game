using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class BoardMusicCardAnimationController : MonoBehaviour
    {
        [Header("Put Card On Board Animation")]
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private RectTransform initialPosition;
        [SerializeField] private RectTransform finalPosition;
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private float scaleAnimationDuration = 0.3f;
        [SerializeField] private float rotationAmount = 360f;

        private GameObject instantiatedCard;
        private RectTransform cardRectTransform;

        public async UniTask PlayPutOnBoardAnimation()
        {
            if (cardPrefab == null || initialPosition == null || finalPosition == null)
            {
                Debug.LogError("Card prefab or positions are not set!");
                return;
            }

            // Utwórz kartę na pozycji początkowej
            instantiatedCard = Instantiate(cardPrefab, initialPosition.parent);
            cardRectTransform = instantiatedCard.GetComponent<RectTransform>();
            
            // Ustaw pozycję początkową
            cardRectTransform.position = initialPosition.position;
            cardRectTransform.localScale = Vector3.zero;
            
            // Sekwencja animacji
            var sequence = DOTween.Sequence();
            
            // 1. Animacja pojawiania się karty (scale up)
            sequence.Append(cardRectTransform.DOScale(1.2f, scaleAnimationDuration)
                .SetEase(Ease.OutBack));
            
            // 2. Lekka korekta skali
            sequence.Append(cardRectTransform.DOScale(1f, scaleAnimationDuration * 0.5f)
                .SetEase(Ease.InOutQuad));
            
            // 3. Animacja przemieszczenia z rotacją
            sequence.Join(cardRectTransform.DOMove(finalPosition.position, animationDuration)
                .SetEase(Ease.InOutCubic));
            
            sequence.Join(cardRectTransform.DORotate(new Vector3(0, 0, rotationAmount), animationDuration)
                .SetEase(Ease.InOutCubic));
            
            // 4. Finalna animacja skali dla efektu "lądowania"
            sequence.Append(cardRectTransform.DOScale(0.95f, 0.1f)
                .SetEase(Ease.OutQuad));
            
            sequence.Append(cardRectTransform.DOScale(1f, 0.1f)
                .SetEase(Ease.OutQuad));

            // Poczekaj na zakończenie animacji
            await sequence.AsyncWaitForCompletion();
        }

        public async UniTask ResetPutOnBoardAnimation()
        {
            // Zatrzymaj wszystkie animacje DOTween na tym obiekcie
            DOTween.Kill(cardRectTransform);
            
            // Usuń kartę bez efektów
            if (instantiatedCard != null)
            {
                Destroy(instantiatedCard);
                instantiatedCard = null;
                cardRectTransform = null;
            }
            
            await UniTask.Yield();
        }

        public async UniTask PlayRevealAnimation()
        {
            await UniTask.Delay(1);
        }
    }
}