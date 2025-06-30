using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Elements {
    public class ButtonElement : MonoBehaviour, IPointerClickHandler 
    {
        public Subject<Unit> OnClick = new Subject<Unit>();

        public void OnPointerClick(PointerEventData eventData) {
            transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1f);
            OnClick?.OnNext(Unit.Default);
        }
    }
}