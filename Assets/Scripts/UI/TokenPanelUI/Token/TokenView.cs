using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DefaultNamespace.ScriptableObjects;

public class TokenView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image tokenImage;
    [SerializeField] private TextMeshProUGUI numberOfTokensText;
    [SerializeField] private TokensImagesSO tokenImages;

    public Action OnTokenClicked;

    public void Setup(TokenModel model)
    {
        SetupTokenImage(model.TokenType, model.NumberOfTokens);
        SetupNumberOfTokens(model.NumberOfTokens);
    }

    public void UpdateView(TokenModel model)
    {
        SetupTokenImage(model.TokenType, model.NumberOfTokens);
        SetupNumberOfTokens(model.NumberOfTokens);
    }

    private void SetupNumberOfTokens(int modelNumberOfTokens)
    {
        numberOfTokensText.text = "x" + modelNumberOfTokens.ToString();
    }

    private void SetupTokenImage(TokenType modelTokenType, int modelNumberOfTokens)
    {
        tokenImage.sprite = tokenImages.GetStackImage(modelTokenType, modelNumberOfTokens);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTokenClicked?.Invoke();
    }
}