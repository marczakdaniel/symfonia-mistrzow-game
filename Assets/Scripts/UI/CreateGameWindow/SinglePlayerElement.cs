using UnityEngine;
using TMPro;

namespace UI.CreateGameWindow
{
    public class SinglePlayerElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private GameObject content;

        public void Initialize(string playerName)
        {
            playerNameText.text = playerName;
        }

        public void SetActive(bool isActive)
        {
            content.SetActive(isActive);
        }
    }
}   