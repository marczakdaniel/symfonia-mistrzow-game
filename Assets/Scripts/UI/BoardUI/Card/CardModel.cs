using System;
using UnityEngine;

public class CardModel
{
    public event Action OnCardSet;
    public event Action OnCardRemove;
    public event Action OnCardVisibleChanged;
    
    public CardData CurrentCard;
    public bool IsCardVisible;

    public bool ActionElementEnabled;

    public CardModel()
    {
        
    }

    public void SetCardVisible(bool value)
    {
        IsCardVisible = value;
        OnCardVisibleChanged?.Invoke();
    }

    public void SetCard(CardData cardData)
    {
        if (CurrentCard != null)
        {
            Debug.LogError("Card already set");
            return;
        }

        CurrentCard = cardData;
        OnCardSet?.Invoke();
    }

    public void RemoveCard()
    {
        if (CurrentCard == null)
        {
            Debug.LogError("Card not set");
            return;
        }

        CurrentCard = null;
        OnCardRemove?.Invoke();
    }
}