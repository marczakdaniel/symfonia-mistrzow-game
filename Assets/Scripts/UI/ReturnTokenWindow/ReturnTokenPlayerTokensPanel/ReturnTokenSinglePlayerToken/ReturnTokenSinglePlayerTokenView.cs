using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ReturnTokenWindow.ReturnTokenPlayerTokensPanel.ReturnTokenSinglePlayerToken
{
    public class ReturnTokenSinglePlayerTokenView : MonoBehaviour
    {
        public Subject<Unit> OnTokenClicked = new Subject<Unit>();

        [SerializeField] private ButtonElement tokenButton;
        [SerializeField] private Image tokenImage;
        [SerializeField] private TextMeshProUGUI tokenCountText;

        public void SetToken(ResourceType token, int count)
        {
            tokenImage.sprite = token.GetSingleResourceTypeImages().StackImage1;
            tokenCountText.text = count.ToString();
        }

        public void Awake()
        {
            tokenButton.OnClick.Subscribe(OnTokenClicked.OnNext).AddTo(this);
        }
    }
}