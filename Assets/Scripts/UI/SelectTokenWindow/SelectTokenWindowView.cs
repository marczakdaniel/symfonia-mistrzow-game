using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using DefaultNamespace.Elements;
using UI.SelectTokenWindow.SelectBoardTokenPanel;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();

        public SelectBoardTokenPanelView SelectBoardTokenPanelView => selectBoardTokenPanelView;

        [SerializeField] private SelectBoardTokenPanelView selectBoardTokenPanelView;
        [SerializeField] private ButtonElement closeButton;

        private void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
        }

        public void OnCloseWindow()
        {
            gameObject.SetActive(false);
        }

        public void OnOpenWindow()
        {
            gameObject.SetActive(true);
        }
    }
}