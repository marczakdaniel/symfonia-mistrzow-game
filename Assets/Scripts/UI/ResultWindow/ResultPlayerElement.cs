using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResultWindow
{
    public class ResultPlayerElement : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerName;

        [SerializeField]
        private TextMeshProUGUI playerPoints;

        [SerializeField]
        private Image playerAvatar;

        public void Setup(string playerName, int points, Sprite playerAvatar)
        {
            this.playerName.text = playerName;
            this.playerPoints.text = points.ToString();
            this.playerAvatar.sprite = playerAvatar;
        }
    }
}