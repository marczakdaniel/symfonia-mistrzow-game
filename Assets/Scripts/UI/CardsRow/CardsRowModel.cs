using System;

public class CardsRowModel
{
    public Action ModelUpdated;
    
    public int Level { get; private set; }
    public int SlotCount => Slots.Length;
    public CardModel[] Slots;
    public bool ActionEnable;

    public CardsRowModel(int level)
    {
        Level = level;
        Slots = new CardModel[4];

        for (int i = 0; i < 4; i++)
        {
            Slots[i] = new CardModel();
        }
    }
    
    public void SetActionEnable(bool value)
    {
        ActionEnable = true;
        foreach (var card in Slots)
        {
            card.SetActionEnable(value);
        }
    }

    public bool TrySetCardAt(CardData cardData, int index)
    {
        if (index < 0 || index >= SlotCount)
        {
            return false;
        }
        
        return Slots[index].TrySetCardModel(cardData);
    }

    public bool TryRemoveCardAt(int index)
    {
        if (index < 0 || index >= SlotCount)
        {
            return false;
        }
        return Slots[index].TryRemoveCardModel();
    }

    public CardModel GetCardModelAt(int index)
    {
        if (index < 0 || index > Slots.Length)
        {
            return null;
        }

        return Slots[index];
    }
}