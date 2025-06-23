using System;
using Unity.VisualScripting;
using UnityEngine;
public class CardsRowPresenter
{
    public CardsRowModel Model { get; private set; }
    public CardsRowView View { get; private set; }

    private CardPresenter[] cardPresenters = new CardPresenter[4];

    public Action<CardData> OnCardClicked;

    public CardsRowPresenter(CardsRowModel model, CardsRowView view)
    {
        Model = model;
        View = view;

        InitializeChildControllers();
        ConnectChildControllersEvents();
        ConnectModelEvents();
        SetupLevelText();
    }

    public void InitializeCards(CardData[] cardDatas)
    {
        for (var index = 0; index < Model.SlotCount; index++)
        {
            cardPresenters[index].SetCard(cardDatas[index]);
        }
    }

    public void SetCard(CardData cardData, int index)
    {
        cardPresenters[index].SetCard(cardData);
    }

    public void RemoveCard(int index)
    {
        cardPresenters[index].RemoveCard();
    }

    public void SetCardVisible(int index, bool visible)
    {
        cardPresenters[index].SetCardVisible(visible);
    }

    public void ShowAllCards()
    {
        foreach (var cardPresenter in cardPresenters)
        {
            cardPresenter.SetCardVisible(true);
        }
    }

    private void SetupLevelText()
    {
        View.Setup(GetLevelText());
    }

    private string GetLevelText()
    {
        return Model.Level switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            _ => "X",
        };
    }

    private void InitializeChildControllers()
    {
        for (var index = 0; index < Model.SlotCount; index++)
        {
            var cardView = View.GetCardViewAt(index);
            cardPresenters[index] = new CardPresenter(new CardModel(index), cardView);
        }
    }

    private void ConnectChildControllersEvents()
    {
        foreach (var cardPresenter in cardPresenters)
        {
            cardPresenter.OnCardClicked += HandleClicked;
        }
    }

    private void ConnectModelEvents()
    {
        
    }

    private void HandleClicked(CardData cardData)
    {
        OnCardClicked.Invoke(cardData);
    }
}