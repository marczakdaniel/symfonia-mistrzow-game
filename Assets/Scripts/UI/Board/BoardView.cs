using System;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField] private CardsRowView[] rowViews;
    //[SerializeField] private CardActionElement actionElement;
    
    public Action OnCardBuy;
    public Action OnCardReserved;
    
    /*public void OnEnable()
    {
        actionElement.OnBuyButtonClick += HandleBuyButtonClick;
        actionElement.OnReserveButtonClick += HandleReserveButtonClick;
    }
    
    public void OnDisable()
    {
        actionElement.OnBuyButtonClick -= HandleBuyButtonClick;
        actionElement.OnReserveButtonClick -= HandleReserveButtonClick;
    }
    
    private void HandleReserveButtonClick()
    {
        OnCardReserved.Invoke();
    }

    private void HandleBuyButtonClick()
    {
        OnCardBuy.Invoke();
    }
    
    public void SetActiveCardActionElement(bool value)
    {
        actionElement.SetAction(value);
    }*/
    
    public CardsRowView GetCardsRowView(int index)
    {
        if (index < 0 || index >= rowViews.Length)
        {
            return null;
        }

        return rowViews[index];
    }
}