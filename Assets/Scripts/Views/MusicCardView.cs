using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DefaultNamespace.Views
{
    public class MusicCardView : MonoBehaviour
    {
        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;

        public void Setup(MusicCardData card)
        {
            costView.Setup(card.cost);
            cardImage.sprite = card.cardImage;
            pointsText.text = card.points.ToString();
        }
    }
}