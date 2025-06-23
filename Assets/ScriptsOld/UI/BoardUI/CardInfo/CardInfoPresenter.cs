using System;
using UnityEditor.Rendering.LookDev;

public class CardInfoPresenter
{
    public Action<CardData> OnBuyButtonClicked;
    public Action<CardData> OnReserveButtonClicked;

    private CardInfoModel _model;
    private CardInfoView _view;
    
    public CardInfoPresenter(CardInfoModel model, CardInfoView view)
    {
        _model = model;
        _view = view;

        ConnectModelEvents();
        ConnectViewEvents();
    }

    public void ShowCardInfo(CardData cardData)
    {
        _model.SetCardData(cardData);
        _model.SetIsWindowVisible(true);
    }
    
    private void ConnectModelEvents()
    {
        _model.OnCardDataChanged += OnCardDataChanged;
        _model.OnIsWindowVisibleChanged += OnIsWindowVisibleChanged;
    }

    private void ConnectViewEvents()
    {
        _view.OnBuyButtonClicked += HandleBuyButtonClicked;
        _view.OnReserveButtonClicked += HandleReserveButtonClicked;
        _view.OnCloseButtonClicked += HandleCloseButtonClicked;
    }

    // Model => Presenter => View

    private void OnCardDataChanged()
    {
        _view.Setup(_model);
    }

    private void OnIsWindowVisibleChanged()
    {
        _view.SetActive(_model.IsWindowVisible);
    }
    
    // View -> Presenter
    private void HandleBuyButtonClicked()
    {
        OnBuyButtonClicked?.Invoke(_model.CardData);
    }

    private void HandleReserveButtonClicked()
    {
        OnReserveButtonClicked?.Invoke(_model.CardData);
    }

    private void HandleCloseButtonClicked()
    {
        _model.SetIsWindowVisible(false);
    }
}
