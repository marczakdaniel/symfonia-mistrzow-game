using System;
using UnityEngine;

public class CardPresenter
{
    public Action<CardData> OnCardClicked;

    private CardModel Model;
    private CardView View;
    
    public CardPresenter(CardModel model, CardView view)
    {
        Model = model;
        View = view;

        ConnectModelEvents();
        ConnectViewEvents();
        
        View.Initialize();
    }

    private void ConnectModelEvents()
    {
        Model.OnCardSet += HandleOnCardSet;
        Model.OnCardRemove += HandleOnCardRemove;
        Model.OnCardVisibleChanged += HandleOnCardVisibleChanged;
    }

    private void ConnectViewEvents()
    {
        View.OnCardClicked += HandleClick;
    }

    // Model -> Presenter -> View

    private void HandleOnCardVisibleChanged()
    {
        View.SetVisible(Model.IsCardVisible);
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

    // Presenter -> Father Presenter

    private void HandleClick()
    {
        OnCardClicked.Invoke(Model.CurrentCard);
    }

    //  Father Presenter -> Presenter -> Model
    public void SetCard(CardData cardData)
    {
        Model.SetCard(cardData);
    }

    public void RemoveCard()
    {
        Model.RemoveCard();
    }

    public void SetCardVisible(bool visible)
    {
        Model.SetCardVisible(visible);
    }
    
}