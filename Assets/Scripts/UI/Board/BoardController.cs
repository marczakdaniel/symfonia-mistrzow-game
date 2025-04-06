using System;
using UnityEngine;

public class BoardController
{
    private const int CARDS_ROW_NUMBER = 3;
    
    public BoardModel Model { get; private set; }
    public BoardView View { get; private set; }

    private CardsRowController[] _cardsRowControllers = new CardsRowController[CARDS_ROW_NUMBER];

    public Action<CardModel> OnCardClicked;

    public BoardController(BoardModel model, BoardView view)
    {
        Model = model;
        View = view;

        InitializeController();
    }

    private void InitializeController()
    {
        for (var index = 0; index < 3; index++)
        {
            var cardsRowModel = Model.CardsRowModels[index];
            var cardsRowView = View.GetCardsRowView(index);

            _cardsRowControllers[index] = new CardsRowController(cardsRowModel, cardsRowView);
            _cardsRowControllers[index].OnCardClicked += HandleClicked;
        }
    }

    /*public void SetCardAt(CardModel cardModel, int level, int position)
    {
        var cardsRowModel = Model.GetCardsRowModelForLevel(level);
        cardsRowModel.TryAddCardAt(cardModel, position);
    }

    public void RemoveCardAt(int level, int position)
    {
        var cardsRowModel = Model.GetCardsRowModelForLevel(level);
        cardsRowModel.TryRemoveCardAt(position);
        _cardsRowControllers[level].RemoveCardAt(position);
    }*/

    public void RemoveCard(CardModel cardModel)
    {
        var cardsRowModel = Model.GetCardsRowModelForLevel(cardModel.Level);
        _cardsRowControllers[cardsRowModel.Level - 1].RemoveCard(cardModel);
    }

    public void SetCard(CardModel cardModel)
    {
        if (cardModel == null)
        {
            return;
        }
        var cardsRowModel = Model.GetCardsRowModelForLevel(cardModel.Level);
        _cardsRowControllers[cardsRowModel.Level - 1].AddCard(cardModel);
    }
    
    private void HandleClicked(CardModel cardModel)
    {
        OnCardClicked.Invoke(cardModel);
    }
}