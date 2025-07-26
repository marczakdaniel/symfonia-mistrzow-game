
using UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel.ReturnTokenSinglePlayerToken;
using UnityEngine;

namespace UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel
{
    public class ReturnTokenPlayerTokensPanelView : MonoBehaviour
    {
        [SerializeField] private ReturnTokenSinglePlayerTokenView[] returnTokenSinglePlayerTokenPrefab = new ReturnTokenSinglePlayerTokenView[6];

        public ReturnTokenSinglePlayerTokenView[] ReturnTokenSinglePlayerTokenPrefab => returnTokenSinglePlayerTokenPrefab;
    }
}