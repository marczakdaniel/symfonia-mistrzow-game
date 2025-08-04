using Cysharp.Threading.Tasks;
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
        public Subject<Unit> OnAddTokenClicked { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnRemoveTokenClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private CardPurchaseTokenElement cardPurchaseTokenElement;
        [SerializeField] private ButtonElement addTokenButton;
        [SerializeField] private ButtonElement removeTokenButton;

        public void Initialize(ResourceType token, int currentSelectedTokensCount, int playerTokensCount)
        {
            cardPurchaseTokenElement.Initialize(token, currentSelectedTokensCount);
            cardPurchaseTokenElement.SetPlayerTokensCount(playerTokensCount);
        }

        public void UpdateCurrentSelectedTokensCount(int count)
        {
            cardPurchaseTokenElement.UpdateValue(count).Forget();
        }

        public void Awake()
        {
            addTokenButton.OnClick.Subscribe(OnAddTokenClicked.OnNext).AddTo(this);
            removeTokenButton.OnClick.Subscribe(OnRemoveTokenClicked.OnNext).AddTo(this);
        }
    }
}