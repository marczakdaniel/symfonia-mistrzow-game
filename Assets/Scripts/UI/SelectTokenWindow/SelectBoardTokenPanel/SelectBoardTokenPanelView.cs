using UnityEngine;
using UI.SelectTokenWindow.SelectSingleToken;

namespace UI.SelectTokenWindow.SelectBoardTokenPanel
{
    public class SelectBoardTokenPanelView : MonoBehaviour
    {
        public SelectSingleTokenView[] SelectSingleTokenViews => selectSingleTokenViews;
        [SerializeField] private SelectSingleTokenView[] selectSingleTokenViews;
    }
}