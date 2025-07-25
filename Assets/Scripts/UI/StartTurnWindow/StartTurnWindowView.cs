using DefaultNamespace.Elements;
using R3;
using UnityEngine;
using TMPro;

namespace UI.StartTurnWindow
{
    public class StartTurnWindowView : MonoBehaviour
    {
        public Subject<Unit> OnStartTurnButtonClick { get; private set; } = new Subject<Unit>();

        [SerializeField] private ButtonElement startTurnButton;
        [SerializeField] private TextMeshProUGUI currentPlayerNameText;

        public void Awake()
        {
            startTurnButton.OnClick.Subscribe(OnStartTurnButtonClick.OnNext);
        }

        public void SetCurrentPlayerName(string name)
        {
            currentPlayerNameText.SetText(name);
        }
    }
}