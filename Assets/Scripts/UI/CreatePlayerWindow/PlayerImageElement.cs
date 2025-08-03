using UnityEngine;
using UnityEngine.UI;

namespace UI.CreatePlayerWindow
{
    public class PlayerImageElement : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image frameImage;
        [SerializeField] private Button button;

        public void Setup(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetSelected(bool selected)
        {
            frameImage.gameObject.SetActive(selected);
        }
    }
}