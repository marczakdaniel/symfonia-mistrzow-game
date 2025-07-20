using DefaultNamespace.Elements;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using DefaultNamespace.Data;

namespace UI.SelectTokenWindow.ChoosenBoardTokenPanel
{
    public class ChoosenBoardTokenPanelView : MonoBehaviour
    {
        public Subject<Unit> OnPanelClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private ChoosenSingleBoardTokenView[] choosenBoardTokenViews = new ChoosenSingleBoardTokenView[3];
        [SerializeField] private ButtonElement buttonElement;

        public async UniTask OnDisabled()
        {
            foreach (var choosenBoardTokenView in choosenBoardTokenViews)
            {
                choosenBoardTokenView.Hide();
            }
        }

        public async UniTask OnOpenAnimation(ResourceType?[] tokens)
        {
            SetupChoosenBoardTokenViews(tokens);
            await UniTask.CompletedTask;
        }

        public async UniTask OnCloseAnimation()
        {
            foreach (var choosenBoardTokenView in choosenBoardTokenViews)
            {
                choosenBoardTokenView.Hide();
            }
            await UniTask.CompletedTask;
        }

        public async UniTask OnAddingTokenAnimation(ResourceType?[] tokens)
        {
            SetupChoosenBoardTokenViews(tokens);
            await UniTask.CompletedTask;
        }

        public async UniTask OnRemovingTokenAnimation(ResourceType?[] tokens)
        {
            SetupChoosenBoardTokenViews(tokens);
            await UniTask.CompletedTask;
        }

        public async UniTask OnActivated()
        {
        }

        public void Awake()
        {
            buttonElement.OnClick.Subscribe(_ => OnPanelClicked?.OnNext(Unit.Default)).AddTo(this);
        }

        private void SetupChoosenBoardTokenViews(ResourceType?[] tokens)
        {
            if (tokens.Length != choosenBoardTokenViews.Length)
            {
                Debug.LogError($"[ChoosenBoardTokenPanelView] Tokens length does not match choosenBoardTokenViews length");
                return;
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == null)
                {
                    choosenBoardTokenViews[i].Hide();
                    continue;
                }

                choosenBoardTokenViews[i].SetupResourceType(tokens[i].Value);
            }
        }
    }
}