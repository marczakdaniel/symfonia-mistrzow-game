using System;
using UnityEngine;

public class CardInfoView : MonoBehaviour
{
    public Action OnBuyButtonClicked;
    public Action OnReserveButtonClicked;
    public Action OnCloseButtonClicked;


    [SerializeField] private CardView cardView;
    [SerializeField] private ButtonElement buyButton;
    [SerializeField] private ButtonElement reserveButton;
    [SerializeField] private ButtonElement closeButton;


    public void Setup(CardInfoModel model)
    {
        cardView.Setup(model.CardData);
        cardView.SetVisible(true);
        cardView.ShowFrontSide();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    void OnEnable()
    {
        buyButton.OnClick += OnBuyButtonClicked;
        reserveButton.OnClick += OnReserveButtonClicked;
        closeButton.OnClick += OnCloseButtonClicked;
    }
    
    void OnDisable()
    {
        buyButton.OnClick -= OnBuyButtonClicked;
        reserveButton.OnClick -= OnReserveButtonClicked;
        closeButton.OnClick -= OnCloseButtonClicked;
    }
}   


