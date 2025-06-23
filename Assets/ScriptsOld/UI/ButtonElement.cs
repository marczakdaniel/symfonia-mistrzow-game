using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonElement : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform objectToScale;

    public Action OnClick;
        
    private Vector3 _originalScale;

    public void OnPointerDown(PointerEventData eventData)
    {
        _originalScale = objectToScale.localScale;
        objectToScale.localScale = _originalScale * 0.9f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        objectToScale.localScale = _originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
