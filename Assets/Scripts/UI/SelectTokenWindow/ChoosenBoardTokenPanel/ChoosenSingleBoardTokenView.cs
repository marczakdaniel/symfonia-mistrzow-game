using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using UnityEngine;
using UnityEngine.UI;


namespace UI.SelectTokenWindow.ChoosenBoardTokenPanel
{
    public class ChoosenSingleBoardTokenView : MonoBehaviour
    {
        [SerializeField] private Image tokenImage;

        public void SetupResourceType(ResourceType resourceType)
        {
            tokenImage.sprite = resourceType.GetSingleResourceTypeImages().StackImage1;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}