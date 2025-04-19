using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private BoardModel _boardModel;
    private BoardView _boardView;
    private BoardController _boardController;

    public BoardManager(BoardModel boardModel, BoardView boardView, BoardController boardController)
    {
        _boardModel = boardModel;
        _boardView = boardView;
        _boardController = boardController;
    }
    
    
}