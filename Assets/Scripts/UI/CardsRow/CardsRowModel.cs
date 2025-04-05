using System.Collections.Generic;

public class CardsRowModel
{
    public int Level { get; private set; }
    public int SlotCount => _slots.Length;
    private CardModel[] _slots;

    public CardsRowModel(int level, List<CardModel> cardModels)
    {
        Level = level;
        _slots = new CardModel[4];
        
        for (var index = 0; index < _slots.Length; index++)
        {
            _slots[index] = cardModels[index];
        }
    }
    

    public bool TryAddCardAt(CardModel cardModel, int index)
    {
        if (index < 0 || index >= SlotCount)
        {
            return false;
        }

        if (_slots[index] != null)
        {
            
            return false;
        }

        _slots[index] = cardModel;
        return true;
    }

    public bool TryRemoveCardAt(int index)
    {
        if (index < 0 || index >= SlotCount)
        {
            return false;
        }

        if (_slots[index] == null)
        {
            return false;
        }

        _slots[index] = null;
        return true;
    }

    /*public bool TryRemoveCard(CardModel cardModel)
    {
        for (var index = 0; index < _slots.Length; index++)
        {
            if (_slots[index].Id != cardModel.Id)
            {
                continue;
            }
            
            _slots[index] = null;
            return true;
        }

        return false;
    }

    public bool TryAddCard(CardModel cardModel)
    {
        for (var index = 0; index < _slots.Length; index++)
        {
            if (_slots[index] != null)
            {
                continue;
            }

            _slots[index] = cardModel;
            return true;
        }

        return false;
    }*/

    public CardModel GetCardModelAt(int index)
    {
        if (index < 0 || index > _slots.Length)
        {
            return null;
        }

        return _slots[index];
    }

    public int GetIndexForCardModel(CardModel cardModel)
    {
        for (var index = 0; index < _slots.Length; index++)
        {
            if (_slots[index] != null && _slots[index].Id == cardModel.Id)
            {
                return index;
            }
        }

        return -1;
    }

    public int GetFreeSpaceIndex()
    {
        for (var index = 0; index < _slots.Length; index++)
        {
            if (_slots[index] == null)
            {
                return index;
            }
        }

        return -1;
    }
}