using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Managers;
using NUnit.Framework;
using UnityEngine;

public class BoardTestManager : MonoBehaviour
{
    [SerializeField] private BoardView boardView;

    [SerializeField] private CardPool testCardDatas;
    
    private BoardData boardData;

    private BoardManager boardManager;
    
    private CardPoolRuntime testCardModelsPool;

    [SerializeField] private OverlayManager OverlayManager;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            boardManager.InitializeBoard(boardData);
        }
    }

    void Start()
    {
        CreateCardData();
        CreateBoardManager();
    }

    private void CreateBoardManager()
    {
        boardManager = new BoardManager(OverlayManager);
        boardManager.InitializeBoardMVC(boardView);
    }

    private void CreateCardData()
    {
        testCardModelsPool = new CardPoolRuntime(testCardDatas);
        boardData = new BoardData();

        for (var i = 0;  i < 3; i++)
        {
            boardData.Board[i] = new CardData[4];
            for (int j = 0; j < 4; j++)
            {
                boardData.Board[i][j] = testCardModelsPool.GetRandomCard(i + 1);
            }
        }
    }
}