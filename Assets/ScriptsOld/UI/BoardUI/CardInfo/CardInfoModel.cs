using System;

public class CardInfoModel
{
    public Action OnCardDataChanged;
    public Action OnIsWindowVisibleChanged;
    public CardData CardData { get; private set; }
    public bool IsWindowVisible { get; private set; }
    
    public CardInfoModel()
    {

    }

    public void SetCardData(CardData cardData)
    {
        CardData = cardData;
        OnCardDataChanged?.Invoke();
    }

    public void SetIsWindowVisible(bool isVisible)
    {
        IsWindowVisible = isVisible;
        OnIsWindowVisibleChanged?.Invoke();
    }
}

