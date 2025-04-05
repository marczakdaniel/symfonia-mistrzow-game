using System;
using UnityEngine;
public class CardsRowController
{
    public CardsRowModel Model { get; private set; }
    public CardsRowView View { get; private set; }

    private CardController[] cardControllers = new CardController[4];

    public Action<CardModel> OnCardClicked;

    public CardsRowController(CardsRowModel model, CardsRowView view)
    {
        Model = model;
        View = view;

        InitializeController();
    }

    private void InitializeController()
    {
        for (var index = 0; index < cardControllers.Length; index++)
        {
            var cardModel = Model.GetCardModelAt(index); 
            if (cardModel == null) return;
            var cardView = View.GetCardViewAt(index);
            if (cardView == null) return;
            
            cardControllers[index] = new CardController(cardModel, cardView);
            cardControllers[index].OnCardClicked += HandleClicked;
        }
    }

    public void SetCardAt(CardModel cardModel, int index)
    {
        if (!Model.TryAddCardAt(cardModel, index))
        {
            return;
        }
        cardControllers[index].UpdateModel(cardModel);
    }

    public void RemoveCardAt(int index)
    {
        Model.TryRemoveCardAt(index);
        cardControllers[index].UpdateModel(null);
    }

    public void RemoveCard(CardModel cardModel)
    {
        var index = Model.GetIndexForCardModel(cardModel);
        RemoveCardAt(index);
    }

    public void AddCard(CardModel cardModel)
    {
        var index = Model.GetFreeSpaceIndex();
        SetCardAt(cardModel, index);
    }

    private void HandleClicked(CardModel cardModel)
    {
        OnCardClicked.Invoke(cardModel);
    }
}