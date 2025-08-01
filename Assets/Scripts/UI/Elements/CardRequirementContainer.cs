using DefaultNamespace.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class CardRequirementContainer : MonoBehaviour
    {
        [SerializeField] 
        private Image image;
        [SerializeField] 
        private TextMeshProUGUI countText;

        public void Initialize(ResourceType resourceType, int count)
        {
            image.color = resourceType.GetColor();
            countText.text = count.ToString();
        }
    }
}