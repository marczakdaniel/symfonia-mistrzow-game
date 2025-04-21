using System;
using UnityEngine;

public class CardController
{
    public Action<CardData> OnCardClicked;

    private CardModel Model;
    private CardView View;
    
    public CardController(CardModel model, CardView view)
    {
        Model = model;
        View = view;
        
        Model.OnCardSet += HandleOnCardSet;
        Model.OnCardRemove += HandleOnCardRemove;
        
        View.OnCardClicked += HandleClick;
        
        View.Setup(Model.CurrentCard);
    }

    private void HandleOnCardSet()
    {
        // TODO
        View.Setup(Model.CurrentCard);
    }
    
    private void HandleOnCardRemove()
    {
        // TODO 
        View.Setup(Model.CurrentCard);
    }

    private void HandleClick()
    {
        OnCardClicked.Invoke(Model.CurrentCard);
        Model.ActionElementEnabled = !Model.ActionElementEnabled;
    }
}