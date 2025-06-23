using System;
using UnityEngine;

public class CardActionElement : MonoBehaviour
{
    public Action OnBuyButtonClick;
    public Action OnReserveButtonClick;
    
    [SerializeField] private BuyButtonElement buyButtonElement;
    [SerializeField] private BuyButtonElement reserveButtonElement;

    public void OnEnable()
    {
        buyButtonElement.OnButtonClick += HandleBuyButtonClick;
        reserveButtonElement.OnButtonClick += HandleReserveButtonClick;
    }

    public void OnDisable()
    {
        buyButtonElement.OnButtonClick -= HandleBuyButtonClick;
        reserveButtonElement.OnButtonClick -= HandleReserveButtonClick;
    }

    public void SetAction(bool value)
    {
        gameObject.SetActive(value);
    }

    private void HandleReserveButtonClick()
    {
        OnReserveButtonClick.Invoke();
    }

    private void HandleBuyButtonClick()
    {
        OnBuyButtonClick.Invoke();
    }
}