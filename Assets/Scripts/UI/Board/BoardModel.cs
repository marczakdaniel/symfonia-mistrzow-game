using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;

public class BoardModel
{
    public CardsRowModel[] CardsRowModels;
    public bool ActionEnable;

    public BoardModel(BoardData boardData)
    {
        CardsRowModels = new CardsRowModel[3];
        
        CardsRowModels[0] = new CardsRowModel(1);
        CardsRowModels[1] = new CardsRowModel(2);
        CardsRowModels[2] = new CardsRowModel(3);

        InitializeWithData(boardData);
    }

    private void InitializeWithData(BoardData boardData)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                CardsRowModels[i].TrySetCardAt(boardData.Board[i, j], j);
            }
        }
    }

    public void SetActionEnable(bool value)
    {
        ActionEnable = true;
        foreach (var row in CardsRowModels)
        {
            row.SetActionEnable(value);
        }
    }

    public bool TrySetCardAt(CardData cardData, int index)
    {
        return GetCardsRowModelForLevel(cardData.Level).TrySetCardAt(cardData, index);
    }

    public bool TryRemoveCardAt(int level, int index)
    {
        return GetCardsRowModelForLevel(level).TryRemoveCardAt(index);
    }
    
    public CardsRowModel GetCardsRowModelForLevel(int level)
    {
        if (level < 1 || level > 3)
        {
            return null;
        }
        return CardsRowModels[level - 1];
    }
}