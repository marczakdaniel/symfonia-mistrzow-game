using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SymfoniaMistrzow.MVP.Common;

namespace SymfoniaMistrzow.MVP.Card
{
    /// <summary>
    /// MonoBehaviour that holds references to the UI elements of a card.
    /// It should have minimal logic, only for displaying data.
    /// </summary>
    public class CardView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image gemImage;
        [SerializeField] private Button buyButton;
        // Add references to cost display elements, artwork, etc.

        public Button BuyButton => buyButton;

        public void SetPoints(int points)
        {
            pointsText.text = points.ToString();
        }

        public void SetGemImage(Sprite sprite)
        {
            gemImage.sprite = sprite;
        }

        // Methods to update cost display, etc.
    }
} 