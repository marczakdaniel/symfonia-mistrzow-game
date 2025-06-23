using UnityEngine;
using TMPro;
using SymfoniaMistrzow.MVP.Common;

namespace SymfoniaMistrzow.MVP.Player
{
    public class PlayerView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI pointsText;
        // Add references to UI elements for tokens

        public void SetName(string name)
        {
            nameText.text = name;
        }

        public void SetPoints(int points)
        {
            pointsText.text = $"Points: {points}";
        }
    }
} 