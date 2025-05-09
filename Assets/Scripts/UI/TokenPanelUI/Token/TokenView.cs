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
            TokenType.Brown => Color.yellow,
            TokenType.Green => Color.green,
            TokenType.Purple => Color.magenta,
            TokenType.All => Color.cyan,
            _ => tokenImage.color
        };
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTokenClicked?.Invoke();
    }
}