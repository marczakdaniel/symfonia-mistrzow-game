using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoardPresenter
{
    private const int CARDS_ROW_NUMBER = 3;
    
    public BoardModel Model { get; private set; }
    public BoardView View { get; private set; }

    private CardsRowPresenter[] _cardsRowPresenter = new CardsRowPresenter[CARDS_ROW_NUMBER];

    public Action<CardData> OnCardClicked;

    public BoardPresenter(BoardModel model, BoardView view)
    {
        Model = model;
        View = view;

        ConnectModelEvents();
        ConnectViewEvents();
        InitializeChildControllers();
        ConnectChildControllersEvents();
    }

    public async UniTask InitializeBoard(CardData[][] cardDatas)
    {
        for (var index = 0; index < 3; index++)
        {
            _cardsRowPresenter[index].InitializeCards(cardDatas[index]);
        }
        
        await View.PlayAllShowdownAnimation();
        
        foreach (var cardsRowController in _cardsRowPresenter)
        {
            cardsRowController.ShowAllCards();
        }

        View.ResetAllCardsShowdownAnimation();
        await View.PlayAllFlipAnimation();
    }

    public async UniTask PutCardOnBoard(CardData cardData, int position)
    {
        var row = cardData.Level - 1;
        _cardsRowPresenter[row].SetCard(cardData, position);
        await View.PlaySingleCardShowdownAnimation(row, position);
        _cardsRowPresenter[row].SetCardVisible(position, true);
        View.ResetSingleCardShowdownAnimation(row, position);
        await View.PlaySingleCardFlipAnimation(row, position);
    }

    public async UniTask RemoveCardFromBoard(int row, int position)
    {
        
    }

    private void ConnectModelEvents()
    {
    }

    private void ConnectViewEvents()
    {
    }

    private void InitializeChildControllers()
    {
        for (var index = 0; index < 3; index++)
        {
            var cardsRowView = View.GetCardsRowView(index);

            _cardsRowPresenter[index] = new CardsRowPresenter(new CardsRowModel(index), cardsRowView);
        }
    }

    private void ConnectChildControllersEvents()
    {
        foreach (var cardsRowController in _cardsRowPresenter)
        {
            cardsRowController.OnCardClicked += HandleClicked;
        }
    }

    private void HandleClicked(CardData cardData)
    {
        OnCardClicked.Invoke(cardData);
    }
}