using DefaultNamespace;
using DefaultNamespace.Data;
using DefaultNamespace.Managers;
using DefaultNamespace.UI.CardActionOverlay;
using UnityEngine;

/*
 * BoardManager
 * Odpowiedzialność:
 * 1. Obsługa planszy
 * 2. Informowanie reszty Mangerów o wykonaniu akcji na planszy
 */

public class BoardManager
{
    private OverlayManager OverlayManager;
    
    private BoardModel _boardModel;
    private BoardView _boardView;
    private BoardController _boardController;

    public BoardManager(OverlayManager overlayManager) // TODO: tutaj jedynie przekazywane menagry z którymi będzie się komunikwoać
    {
        OverlayManager = overlayManager;
    }

    public void InitializeBoardMVC(BoardView view)
    {
        InitializeBoardModel();
        InitializeBoardView(view);
        InitializeBoardController();
    }

    private void InitializeBoardModel()
    {
        _boardModel = new BoardModel();
    }

    private void InitializeBoardView(BoardView view)
    {
        _boardView = view;
    }

    private void InitializeBoardController()
    {
        _boardController = new BoardController(_boardModel, _boardView);
        _boardController.OnCardClicked += HandleCardClicked;
    }
    
    // Handle Board Events

    private void HandleCardReserve(CardData cardData)
    {
        Debug.LogError($"Card Reserve Action {cardData.Id}");
    }

    private void HandleCardBuy(CardData cardData)
    {
        Debug.LogError($"Card Buy Action {cardData.Id}");
    }

    private void HandleCardClicked(CardData cardData)
    {
        Debug.LogError($"Card Click Action {cardData.Id}");

        var overlayData = new CardActionOverlayData(cardData, HandleCardBuy, HandleCardReserve);
        OverlayManager.OpenCardActionOverlay(overlayData);
    }
    
    // Expose necessary methods to change board

    public void InitializeBoard(BoardData boardData)
    {
        _boardModel.InitializeBoard(boardData);
    }

    public void AddCard(CardData cardData, int index)
    {
        _boardModel.GetCardsRowModelForLevel(cardData.Level).TrySetCardAt(cardData, index);
    }

    public void RemoveCard(int level, int index)
    {
        _boardModel.GetCardsRowModelForLevel(level).TryRemoveCardAt(index);
    }

    // 
}