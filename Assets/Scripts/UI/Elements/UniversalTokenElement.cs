using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{

    public class UniversalTokenElement : MonoBehaviour
    {
        public Subject<Unit> OnTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private ButtonElement button;
        [SerializeField] private ValueChangeAnimationController valueChangeAnimationController;
        [SerializeField] private AnimationSequencerController showAnimation;
        [SerializeField] private AnimationSequencerController hideAnimation;

        private int currentValue = 0;
        private ResourceType resourceType;

        public void Initialize(ResourceType resourceType, int count)
        {
            this.resourceType = resourceType;
            this.currentValue = count;

            UpdateValue(count, false).Forget();
        }

        public async UniTask UpdateValue(int value, bool playAnimation = true)
        {
            var valueChange = value - currentValue;
            currentValue = value;
            countText.text = $"x{currentValue}";
            image.sprite = resourceType.GetSingleResourceTypeImages().StackImage1;

            if (playAnimation && valueChange != 0)
            {
                await valueChangeAnimationController.PlayAnimation(valueChange);
            }
        }

        public async UniTask PlayShowAnimation()
        {
            await showAnimation.PlayAsync();
        }

        public async UniTask PlayHideAnimation()
        {
            await hideAnimation.PlayAsync();
        }

        public void Awake()
        {
            button.OnClick.Subscribe(_ => OnTokenClicked.OnNext(Unit.Default)).AddTo(this);
        }
    }

}