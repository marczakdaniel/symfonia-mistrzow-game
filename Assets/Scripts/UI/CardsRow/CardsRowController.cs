using System;
using UnityEngine;
public class CardsRowController
{
    public CardsRowModel Model { get; private set; }
    public CardsRowView View { get; private set; }

    private CardController[] cardControllers = new CardController[4];

    public Action<CardData> OnCardClicked;

    public CardsRowController(CardsRowModel model, CardsRowView view)
    {
        Model = model;
        View = view;

        InitializeController();
    }

    private void InitializeController()
    {
        for (var index = 0; index < Model.SlotCount; index++)
        {
            var cardModel = Model.GetCardModelAt(index); 
            var cardView = View.GetCardViewAt(index);
            
            cardControllers[index] = new CardController(cardModel, cardView);
            cardControllers[index].OnCardClicked += HandleClicked;
        }
    }

    private void HandleClicked(CardData cardData)
    {
        OnCardClicked.Invoke(cardData);
    }
}