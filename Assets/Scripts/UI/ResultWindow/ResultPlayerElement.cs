using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResultWindow
{
    public class ResultPlayerElement : MonoBehaviour
    {
        public Subject<Unit> OnClicked = new Subject<Unit>();

        [SerializeField]
        private TextMeshProUGUI playerName;

        [SerializeField]
        private TextMeshProUGUI playerPoints;

        [SerializeField]
        private Image playerAvatar;

        [SerializeField]
        private ButtonElement resultButton;

        public void Setup(string playerName, int points, Sprite playerAvatar)
        {
            this.playerName.text = playerName;
            this.playerPoints.text = points.ToString();
            this.playerAvatar.sprite = playerAvatar;
        }

        public void Awake()
        {
            resultButton.OnClick.Subscribe(OnClicked.OnNext).AddTo(this);
        }
    }
}