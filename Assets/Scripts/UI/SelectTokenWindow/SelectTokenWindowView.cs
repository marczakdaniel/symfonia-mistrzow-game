using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;
using DefaultNamespace.Elements;
using UI.SelectTokenWindow.SelectBoardTokenPanel;
using UI.SelectTokenWindow.ChoosenBoardTokenPanel;

namespace UI.SelectTokenWindow
{
    public class SelectTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnAcceptButtonClicked { get; private set; } = new Subject<Unit>();
        
        public SelectBoardTokenPanelView SelectBoardTokenPanelView => selectBoardTokenPanelView;
        public ChoosenBoardTokenPanelView ChoosenBoardTokenPanelView => choosenBoardTokenPanelView;

        [SerializeField] private SelectBoardTokenPanelView selectBoardTokenPanelView;
        [SerializeField] private ChoosenBoardTokenPanelView choosenBoardTokenPanelView;
        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement acceptButton;

        private void Awake()
        {
            closeButton.OnClick.Subscribe(_ => OnCloseButtonClicked.OnNext(Unit.Default)).AddTo(this);
            acceptButton.OnClick.Subscribe(_ => OnAcceptButtonClicked.OnNext(Unit.Default)).AddTo(this);
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