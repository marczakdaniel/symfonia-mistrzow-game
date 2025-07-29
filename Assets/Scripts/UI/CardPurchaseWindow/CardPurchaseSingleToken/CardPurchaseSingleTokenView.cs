using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CardPurchaseWindow.CardPurchaseSingleToken
{
    public class CardPurchaseSingleTokenView : MonoBehaviour
    {
        public Subject<Unit> OnTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private ButtonElement tokenButton;
        [SerializeField] private Image tokenImage;
        [SerializeField] private TextMeshProUGUI tokenCountText;

        public void SetToken(ResourceType token, int currentSelectedTokensCount, int playerTokensCount)
        {
            tokenImage.sprite = token.GetSingleResourceTypeImages().StackImage1;
            tokenCountText.text = $"{currentSelectedTokensCount}/{playerTokensCount}";
        }

        public void Awake()
        {
            tokenButton.OnClick.Subscribe(OnTokenClicked.OnNext).AddTo(this);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}