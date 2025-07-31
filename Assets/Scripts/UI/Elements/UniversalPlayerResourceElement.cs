using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UniversalPlayerResourceElement : MonoBehaviour
    {
        [SerializeField] 
        private Image tokenImage;

        [SerializeField]
        private TextMeshProUGUI tokenCountText;

        [SerializeField]
        private Image cardImage;
        [SerializeField]
        private TextMeshProUGUI cardCountText;

        public void Initialize(ResourceType resourceType, int tokenCount, int cardCount)
        {
            tokenImage.color = resourceType.GetColor();
            tokenCountText.text = tokenCount.ToString();
            cardImage.color = resourceType.GetColor();
            cardCountText.text = cardCount.ToString();

            UpdateValue(tokenCount, cardCount);
        }

        public void UpdateValue(int tokenCount, int cardCount)
        {
            tokenCountText.text = tokenCount.ToString();
            cardCountText.text = cardCount.ToString();
        }
    }
}