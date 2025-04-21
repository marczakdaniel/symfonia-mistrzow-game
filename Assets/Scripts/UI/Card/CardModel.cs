using System;

public class CardModel
{
    public event Action OnCardSet;
    public event Action OnCardRemove;
    
    public CardData CurrentCard;
    public bool ActionEnable;

    public bool ActionElementEnabled;

    public CardModel()
    {
        
    }

    public void SetActionEnable(bool value)
    {
        ActionEnable = value;
        
    }

    public bool TrySetCardModel(CardData cardData)
    {
        if (CurrentCard != null)
        {
            return false;
        }
        
        CurrentCard = cardData;
        OnCardSet?.Invoke();
        return true;
    }

    public bool TryRemoveCardModel()
    {
        if (CurrentCard == null)
        {
            return false;
        }
        CurrentCard = null;
        OnCardRemove?.Invoke();
        return true;
    }
}