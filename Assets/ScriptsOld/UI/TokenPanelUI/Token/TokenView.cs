using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DefaultNamespace.ScriptableObjects;
using Cysharp.Threading.Tasks;

public class TokenView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image tokenImage;
    [SerializeField] private TextMeshProUGUI numberOfTokensText;
    [SerializeField] private TokensImagesSO tokenImages;
    [SerializeField] private TokenValueChangeAnimation valueChangeAnimation;

    public Action OnTokenClicked;

    public void Setup(TokenModel model)
    {
        SetupTokenImage(model.TokenType, model.NumberOfTokens);
        SetupNumberOfTokens(model.NumberOfTokens);
    }

    public void InitializeToken(int numberOfTokens)
    {
        SetupNumberOfTokens(numberOfTokens);
    }

    public async UniTask AddToken(int numberOfTokens, int difference)
    {
        SetupNumberOfTokens(numberOfTokens);
        await valueChangeAnimation.PlayValueChangeAnimation(difference, true);
    }

    public async UniTask RemoveToken(int numberOfTokens, int difference)
    {
        SetupNumberOfTokens(numberOfTokens);
        await valueChangeAnimation.PlayValueChangeAnimation(difference, false);
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
        return;
        tokenImage.sprite = tokenImages.GetStackImage(modelTokenType, modelNumberOfTokens);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTokenClicked?.Invoke();
    }
}