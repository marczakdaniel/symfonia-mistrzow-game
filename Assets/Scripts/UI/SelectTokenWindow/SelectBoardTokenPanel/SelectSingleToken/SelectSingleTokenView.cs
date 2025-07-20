using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using UnityEngine.EventSystems;
using DefaultNamespace.Data;
using Cysharp.Threading.Tasks;

namespace UI.SelectTokenWindow.SelectSingleToken
{
    public class SelectSingleTokenView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private Image tokenImage;  
        [SerializeField] private TextMeshProUGUI tokenCountText;

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

        public void Setup(ResourceType resourceType, int count)
        {
            tokenImage.sprite = resourceType.GetSingleResourceTypeImages().StackImage1;
            tokenCountText.text = "x" + count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTokenClicked?.OnNext(Unit.Default);
        }
    }
}