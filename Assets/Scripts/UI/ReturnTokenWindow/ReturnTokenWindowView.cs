using DefaultNamespace.Elements;
using UnityEngine;
using R3;
using UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel.ReturnTokenSinglePlayerToken;

namespace UI.ReturnTokenWindow
{
    public class ReturnTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnAcceptClicked = new Subject<Unit>();

        [SerializeField] private ButtonElement acceptButton;
        [SerializeField] private ReturnTokenSinglePlayerTokenView[] returnTokenSinglePlayerTokenPrefab = new ReturnTokenSinglePlayerTokenView[6];

        public ReturnTokenSinglePlayerTokenView[] ReturnTokenSinglePlayerTokenPrefab => returnTokenSinglePlayerTokenPrefab;

        public void Awake()
        {
            acceptButton.OnClick.Subscribe(OnAcceptClicked.OnNext).AddTo(this);
        }
    }
}