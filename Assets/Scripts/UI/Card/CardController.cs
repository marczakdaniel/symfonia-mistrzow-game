using System;
using UnityEngine;

public class CardController
{
    public CardModel Model { get; private set; }
    public CardView View { get; private set; }

    public Action<CardModel> OnCardClicked;

    public CardController(CardModel model, CardView view)
    {
        Model = model;
        View = view;
        
        View.Setup(Model);
        View.OnCardClicked += HandleClick;
    }
    
    public void UpdateModel(CardModel model)
    {
        Model = model;
        View.Setup(Model);
    }

    private void HandleClick()
    {
        OnCardClicked.Invoke(Model);
    }
}