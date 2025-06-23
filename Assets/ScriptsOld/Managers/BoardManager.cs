using Cysharp.Threading.Tasks;
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
    private BoardPresenter _boardPresenter;
    private CardInfoPresenter _cardInfoPresenter;

    public BoardManager(OverlayManager overlayManager) // TODO: tutaj jedynie przekazywane menagry z którymi będzie się komunikwoać
    {
        OverlayManager = overlayManager;
    }


    public void InitializeBoardMVC(BoardView view)
    {
        _boardPresenter = new BoardPresenter(new BoardModel(), view);
        HandleBoardEvents();
    }

    public void InitializeBoard(BoardData boardData)
    {
        _boardPresenter.InitializeBoard(boardData.Board).Forget();
    }

    public void PutCardOnBoard(CardData cardData, int index)
    {
        _boardPresenter.PutCardOnBoard(cardData, index).Forget();
    }

    public void RemoveCardFromBoard(CardData cardData)
    {
    }
    
    public void InitializeCardInfoPresenter(CardInfoView cardInfoView)
    {
        _cardInfoPresenter = new CardInfoPresenter(new CardInfoModel(), cardInfoView);
        HandleCardInfoEvents();
    }

    public void ShowCardInfo(CardData cardData)
    {
        _cardInfoPresenter.ShowCardInfo(cardData);
    }

    private void HandleBoardEvents()
    {
        _boardPresenter.OnCardClicked += HandleCardClicked;
    }

    private void HandleCardInfoEvents()
    {
        _cardInfoPresenter.OnBuyButtonClicked += HandleCardBuy;
        _cardInfoPresenter.OnReserveButtonClicked += HandleCardReserve;
    }

    // Handle Board Events

    private void HandleCardReserve(CardData cardData)
    {
        Debug.LogError($"Card Reserve Action {cardData.Id}");
    }

    private void HandleCardBuy(CardData cardData)
    {
        Debug.LogError($"Card Buy Action {cardData.Id}");
        RemoveCardFromBoard(cardData);
    }

    private void HandleCardClicked(CardData cardData)
    {
        Debug.LogError($"Card Click Action {cardData.Id}");
        ShowCardInfo(cardData);
    }
    
    // Expose necessary methods to change board
    // 
}