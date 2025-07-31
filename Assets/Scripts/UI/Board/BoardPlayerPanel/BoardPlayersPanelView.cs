using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Board.BoardPlayerPanel
{
    public class BoardPlayerPanelView : MonoBehaviour
    {
        public Subject<Unit> OnClick { get; private set; } = new Subject<Unit>();

        [SerializeField]
        private ButtonElement buttonElement;

        [SerializeField] 
        private Image playerImage;

        [SerializeField] 
        private GameObject activePlayerIndicator;

        [SerializeField]
        private TextMeshProUGUI playerPointsText;

        public void SetPlayerImage(Sprite sprite)
        {
            playerImage.sprite = sprite;
        }

        public void SetActivePlayerIndicator(bool isActive)
        {
            activePlayerIndicator.SetActive(isActive);
        }

        public void SetPlayerPoints(int points)
        {
            playerPointsText.text = points.ToString();
        }

        public void Awake()
        {
            buttonElement.OnClick.Subscribe(OnClick.OnNext).AddTo(this);
        }
    }
}