using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BoardController
{
    private const int CARDS_ROW_NUMBER = 3;
    
    public BoardModel Model { get; private set; }
    public BoardView View { get; private set; }

    private CardsRowController[] _cardsRowControllers = new CardsRowController[CARDS_ROW_NUMBER];

    public Action<CardData> OnCardClicked;

    public BoardController(BoardModel model, BoardView view)
    {
        Model = model;
        View = view;

        ConnectModelEvents();
        InitializeController();
    }

    private void ConnectModelEvents()
    {
        Model.OnBoardInitialized += HandleModelBoardInitialized;
    }

    private void HandleModelBoardInitialized()
    {
        InitializeBoard().Forget();
    }

    private async UniTask InitializeBoard()
    {
        await View.PlayAllShowdownAnimation();
        Model.ShowAllCards();
        View.ResetAllCardsShowdownAnimation();
        await View.PlayAllFlipAnimation();
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

    private void HandleClicked(CardData cardData)
    {
        OnCardClicked.Invoke(cardData);
    }
}