using DefaultNamespace.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSingleCostElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TokensImagesSO tokenImages;

    public void Setup(TokenType tokenType, int cost)
    {
        if (cost == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        SetupImage(tokenType);
        SetupCostText(cost);
    }

    private void SetupCostText(int cost)
    {
        costText.text = cost.ToString();
    }

    private void SetupImage(TokenType tokenType)
    {
        image.sprite = tokenImages.GetTokenCardImage(tokenType);
    }
}