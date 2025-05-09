using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;

public class BoardModel
{
    public CardsRowModel[] CardsRowModels;
    public Action OnBoardInitialized;

    public BoardModel()
    {
        CardsRowModels = new CardsRowModel[3];
        
        CardsRowModels[0] = new CardsRowModel(1);
        CardsRowModels[1] = new CardsRowModel(2);
        CardsRowModels[2] = new CardsRowModel(3);
    }

    public void InitializeBoard(BoardData boardData)
    {
        for (int i = 0; i < 3; i++)
        {
            CardsRowModels[i].InitializeRow(boardData.Board[i]);
        }
        OnBoardInitialized?.Invoke();
    }

    public void AddCard(CardData cardData, int index)
    {

    }

    public void RemoveCard(int index)
    {

    }

    public void ShowAllCards()
    {
        foreach (var cardsRowModel in CardsRowModels)
        {
            cardsRowModel.ShowAllCards();
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