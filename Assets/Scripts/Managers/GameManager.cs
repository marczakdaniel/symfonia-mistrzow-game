using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ActionManager ActionManager;
    public TurnManager TurnManager;
    public BoardManager BoardManager;

    private void Start()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        ActionManager = new ActionManager();
        TurnManager = new TurnManager();
    }
}