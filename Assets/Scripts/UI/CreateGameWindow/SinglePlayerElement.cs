using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI.CreateGameWindow
{
    public class SinglePlayerElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private GameObject content;
        [SerializeField] private Image image;

        public void Initialize(string playerName, Sprite playerAvatar)
        {
            playerNameText.text = playerName;
            image.sprite = playerAvatar;
        }

        public void SetActive(bool isActive)
        {
            content.SetActive(isActive);
        }
    }
}   