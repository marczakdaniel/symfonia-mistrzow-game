using DefaultNamespace.Elements;
using UnityEngine;
using R3;
using UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken;
using UI.ReturnTokenWindow.ReturnTokenSelectedPanel;

namespace UI.ReturnTokenWindow
{
    public class ReturnTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnAcceptClicked = new Subject<Unit>();

        [SerializeField] private ButtonElement acceptButton;
        [SerializeField] private ReturnTokenSinglePlayerTokenView[] returnTokenSinglePlayerTokenPrefab = new ReturnTokenSinglePlayerTokenView[6];
        [SerializeField] private ReturnTokenSelectedPanelView returnTokenSelectedPanelPrefab;

        public ReturnTokenSinglePlayerTokenView[] ReturnTokenSinglePlayerTokenPrefab => returnTokenSinglePlayerTokenPrefab;
        public ReturnTokenSelectedPanelView ReturnTokenSelectedPanelPrefab => returnTokenSelectedPanelPrefab;
        public void Awake()
        {
            acceptButton.OnClick.Subscribe(OnAcceptClicked.OnNext).AddTo(this);
        }
    }
}