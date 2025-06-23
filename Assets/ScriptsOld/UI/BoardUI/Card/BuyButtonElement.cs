using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButtonElement : MonoBehaviour, IPointerClickHandler
{
    public Action OnButtonClick;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClick.Invoke();
    }
}