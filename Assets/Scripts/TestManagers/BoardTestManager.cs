using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;

public class BoardTestManager : MonoBehaviour
{
    [SerializeField] private BoardView boardView;

    [SerializeField] private CardPool testCardDatas;
    
    private BoardModel boardModel;

    private BoardController boardController;

    private CardPoolRuntime testCardModelsPool;

    private Dictionary<int, List<CardModel>> cardsModels;
    

    void Start()
    {
        CreateTestCardModels();
        InitializeGame();
    }

    private void CreateTestCardModels()
    {
        testCardModelsPool = new CardPoolRuntime(testCardDatas);
        cardsModels = new Dictionary<int, List<CardModel>>(3);

        for (var index = 1; index <= 3; index++)
        {
            var cardModels = new List<CardModel>(4);
            for (int j = 1; j <= 4; j++)
            {
                cardModels.Add(testCardModelsPool.GetRandomCard(index));
            }

            cardsModels.Add(index, cardModels);
        }
    }

    private void InitializeGame()
    {
        boardModel = new BoardModel(cardsModels[1], cardsModels[2], cardsModels[3]);

        boardController = new BoardController(boardModel, boardView);
        boardController.OnCardClicked += HandleClicked;
    }

    private void HandleClicked(CardModel cardModel)
    {
        boardController.RemoveCard(cardModel);
        var newCardModel = testCardModelsPool.GetRandomCard(cardModel.Level);
        boardController.SetCard(newCardModel);
    }
}