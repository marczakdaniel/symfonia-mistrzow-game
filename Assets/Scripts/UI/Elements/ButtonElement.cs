using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Elements {
    public class ButtonElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Subject<Unit> OnClick = new Subject<Unit>();

        private bool lockClick = false;
        private Vector3 originalScale;
        private bool isPressed = false;

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (lockClick || isPressed)
            {
                return;
            }
            
            // Aktualizuj originalScale przed rozpoczęciem animacji
            originalScale = transform.localScale;
            
            isPressed = true;
            
            // Szybsza animacja naciśnięcia
            Sequence pressSequence = DOTween.Sequence();
            
            // Zmniejsz z lekkim obrotem
            pressSequence.Join(transform.DOScale(originalScale * 0.9f, 0.06f).SetEase(Ease.OutBack));
            pressSequence.Join(transform.DORotate(new Vector3(0, 0, -1f), 0.06f).SetEase(Ease.OutQuad));
            
            pressSequence.Play();
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (!isPressed || lockClick)
            {
                return;
            }
            
            isPressed = false;
            lockClick = true;
            
            // Szybsza animacja puszczenia
            Sequence releaseSequence = DOTween.Sequence();
            
            // Przywróć rozmiar z efektem sprężystości
            releaseSequence.Join(transform.DOScale(originalScale * 1.05f, 0.09f).SetEase(Ease.OutBack));
            releaseSequence.Join(transform.DOScale(originalScale, 0.05f).SetDelay(0.09f).SetEase(Ease.InOutQuad));
            
            // Przywróć obrót
            releaseSequence.Join(transform.DORotate(Vector3.zero, 0.09f).SetEase(Ease.OutQuad));
            
            releaseSequence.OnComplete(() => {
                lockClick = false;
                OnClick?.OnNext(Unit.Default);
            });
            
            releaseSequence.Play();
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (isPressed && !lockClick)
            {
                isPressed = false;
                
                // Przywróć rozmiar bez efektów
                Sequence exitSequence = DOTween.Sequence();
                exitSequence.Join(transform.DOScale(originalScale, 0.06f).SetEase(Ease.OutQuad));
                exitSequence.Join(transform.DORotate(Vector3.zero, 0.06f).SetEase(Ease.OutQuad));
                
                exitSequence.Play();
            }
        }
    }
}