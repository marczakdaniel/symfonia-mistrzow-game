using Assets.Scripts.UI.Elements;
using TMPro;
using UnityEngine;

namespace UI.CardPurchaseWindow.CardPurchaseSingleToken
{
    public class CardPurchaseTokenElement : UniversalTokenElement
    {
        [SerializeField] private TextMeshProUGUI playerTokensCountText;

        public void SetPlayerTokensCount(int count)
        {
            playerTokensCountText.text = count.ToString();
        }
    }
}