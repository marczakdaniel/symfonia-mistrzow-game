using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TokenView : MonoBehaviour, IPointerClickHandler
{
    public TokenType TokenType => tokenType;
    
    [SerializeField] private TokenType tokenType;
    [SerializeField] private Image tokenImage;
    [SerializeField] private TextMeshProUGUI numberOfTokensText;

    public Action OnTokenClicked;

    public void Setup(TokenModel model)
    {
        SetupTokenImage(model.TokenType);
        SetupNumberOfTokens(model.NumberOfTokens);
    }

    public void UpdateView(TokenModel model)
    {
        SetupNumberOfTokens(model.NumberOfTokens);
    }

    private void SetupNumberOfTokens(int modelNumberOfTokens)
    {
        numberOfTokensText.text = modelNumberOfTokens.ToString();
    }

    private void SetupTokenImage(TokenType modelTokenType)
    {
        tokenImage.color = modelTokenType switch
        {
            TokenType.Blue => Color.blue,
            TokenType.Red => Color.red,
            TokenType.Yellow => Color.yellow,
            TokenType.Green => Color.green,
            TokenType.Magenta => Color.magenta,
            TokenType.All => Color.cyan,
            _ => tokenImage.color
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTokenClicked?.Invoke();
    }
}