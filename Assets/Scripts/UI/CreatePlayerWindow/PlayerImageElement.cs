using DefaultNamespace.Elements;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CreatePlayerWindow
{
    public class PlayerImageElement : MonoBehaviour
    {
        public Subject<(Sprite, int)> OnClick { get; private set; } = new Subject<(Sprite, int)>();

        [SerializeField] private Image image;
        [SerializeField] private Image frameImage;
        [SerializeField] private ButtonElement button;

        private int index;

        public int Index => index;

        public void Setup(Sprite sprite, int index)
        {
            image.sprite = sprite;
            this.index = index;
        }

        public void SetSelected(bool selected)
        {
            frameImage.gameObject.SetActive(selected);
        }

        public void Awake()
        {
            button.OnClick.Subscribe(_ => OnClick.OnNext((image.sprite, index))).AddTo(this);
        }
    }
}