using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.ConcertCard
{
    public class ConcertCardView : MonoBehaviour
    {
        [SerializeField] private CardCostElement requirementElement;
        [SerializeField] private TextMeshProUGUI pointsText;

        public void Setup(ConcertCardModel model)
        {
            if (model == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            requirementElement.Setup(model.Requirements);
            pointsText.text = model.Points.ToString();
        }
    }
}