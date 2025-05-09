using System;

public class CardsRowModel
{
    public Action ModelUpdated;
    
    public int Level { get; private set; }
    public int SlotCount => Slots.Length;
    public CardModel[] Slots;

    public CardsRowModel(int level)
    {
        Level = level;
        Slots = new CardModel[4];

        for (int i = 0; i < 4; i++)
        {
            Slots[i] = new CardModel();
        }
    }


    public void InitializeRow(CardData[] cardDatas)
    {
        for (int i = 0; i < cardDatas.Length; i++)
        {
            TrySetCardAt(cardDatas[i], i);
        }
    }

    public void AddCard(CardData cardData, int index)
    {
        
    }

    public void RemoveCard(int index)
    {
        
    }

    public void ShowAllCards()
    {
        foreach (var slot in Slots)
        {
            slot.SetCardVisible(true);
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