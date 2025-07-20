using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Elements {
    public class ButtonElement : MonoBehaviour, IPointerClickHandler 
    {
        public Subject<Unit> OnClick = new Subject<Unit>();

        private bool lockClick = false;

        public void OnPointerClick(PointerEventData eventData) {
            if (lockClick)
            {
                return;
            }
            lockClick = true;
            transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1f).OnComplete(() => lockClick = false);
            OnClick?.OnNext(Unit.Default);
        }
    }
}