using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using UnityEngine.EventSystems;
using DefaultNamespace.Data;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Elements;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public class SelectSingleTokenView : MonoBehaviour
    {
        public Subject<Unit> OnTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private Image tokenImage;  
        [SerializeField] private TextMeshProUGUI tokenCountText;

        [SerializeField] private ButtonElement buttonElement;

        public async UniTask OnDisabled()
        {
            // TODO: Disable animation
            gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }

        public async UniTask OnActivated()
        {
            // TODO: Active animation
            gameObject.SetActive(true);
            await UniTask.CompletedTask;
        }

        public async UniTask OnAddingTokenAnimation(ResourceType resourceType, int count)
        {
            // TODO: Adding token animation
            Setup(resourceType, count);
            await UniTask.CompletedTask;
        }

        public async UniTask OnRemovingTokenAnimation(ResourceType resourceType, int count)
        {
            // TODO: Removing token animation
            Setup(resourceType, count);
            await UniTask.CompletedTask;
        }

        public async UniTask OnOpenAnimation(ResourceType resourceType, int count)
        {
            Setup(resourceType, count);
            // TODO: Open animation
            await UniTask.CompletedTask;
        }
        
        public async UniTask OnCloseAnimation()
        {
            // TODO: Close animation
            await UniTask.CompletedTask;
        }

        public void Setup(ResourceType resourceType, int count)
        {
            tokenImage.sprite = resourceType.GetSingleResourceTypeImages().StackImage1;
            tokenCountText.text = "x" + count.ToString();
        }

        public void Awake()
        {
            buttonElement.OnClick.Subscribe(_ => OnTokenClicked?.OnNext(Unit.Default)).AddTo(this);
        }
    }
}