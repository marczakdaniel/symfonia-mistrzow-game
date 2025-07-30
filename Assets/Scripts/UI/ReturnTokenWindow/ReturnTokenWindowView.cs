using DefaultNamespace.Elements;
using UnityEngine;
using R3;
using UI.ReturnTokenWindow.ReturnTokenSinglePlayerToken;
using UI.ReturnTokenWindow.ReturnTokenSelectedPanel;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
namespace UI.ReturnTokenWindow
{
    public class ReturnTokenWindowView : MonoBehaviour
    {
        public Subject<Unit> OnAcceptClicked = new Subject<Unit>();

        [SerializeField] private ButtonElement acceptButton;
        [SerializeField] private ReturnTokenSinglePlayerTokenView[] returnTokenSinglePlayerTokenPrefab = new ReturnTokenSinglePlayerTokenView[6];
        [SerializeField] private ReturnTokenSelectedPanelView returnTokenSelectedPanelPrefab;
        [SerializeField] private ReturnTokenAllTokenCountElement returnTokenAllTokenCountElementPrefab;
        [SerializeField] private AnimationSequencerController openAnimationSequencerController;
        [SerializeField] private AnimationSequencerController closeAnimationSequencerController;

        public ReturnTokenSinglePlayerTokenView[] ReturnTokenSinglePlayerTokenPrefab => returnTokenSinglePlayerTokenPrefab;
        public ReturnTokenSelectedPanelView ReturnTokenSelectedPanelPrefab => returnTokenSelectedPanelPrefab;
        public void Awake()
        {
            acceptButton.OnClick.Subscribe(OnAcceptClicked.OnNext).AddTo(this);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimationSequencerController.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimationSequencerController.PlayAsync();
        }

        public void SetAllTokenCount(int allTokenCount)
        {
            returnTokenAllTokenCountElementPrefab.SetAllTokenCount(allTokenCount);
        }
    }
}