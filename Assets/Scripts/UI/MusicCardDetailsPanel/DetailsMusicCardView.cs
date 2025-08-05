using DefaultNamespace.Data;
using TMPro;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using UnityEngine;
using UnityEngine.UI;
using R3;
using DefaultNamespace.Elements;
using Coffee.UIEffects;

namespace UI.MusicCardDetailsPanel {
    public class DetailsMusicCardView : MonoBehaviour 
    {
        public Subject<string> OnCardClicked { get; private set; } = new Subject<string>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;
        [SerializeField] private ButtonElement button;
        [SerializeField] private UIEffect uiEffect;
        [SerializeField] private Color[] uiEffectColors = new Color[3];
        [SerializeField] private GameObject cardReverse;
        [SerializeField] private GameObject cardFront;

        private string cardId;

        private void Awake()
        {
            ValidateComponents();
            button.OnClick.Subscribe(_ => OnCardClicked.OnNext(cardId)).AddTo(this);
        }

        private void ValidateComponents()
        {
            if (costView == null)
                Debug.LogError("CostView is not assigned on MusicCardView", this);
            if (cardImage == null)
                Debug.LogError("CardImage is not assigned on MusicCardView", this);
            if (pointsText == null)
                Debug.LogError("PointsText is not assigned on MusicCardView", this);
            if (resourceProvidedImage == null)
                Debug.LogError("ResourceProvidedImage is not assigned on MusicCardView", this);
        }

        public void Setup(MusicCardData card)
        {
            cardId = card.Id;
            costView.Setup(card.cost);
            cardImage.sprite = card.cardImage;
            pointsText.text = card.points.ToString();
            resourceProvidedImage.sprite = card.resourceProvided.GetSingleResourceTypeImages().StackImage1;

            SetCardLevel(card.level);
            SetCanBePurchased(false);
        }

        public void SetCardFront(bool isFront)
        {
            cardReverse.SetActive(!isFront);
            cardFront.SetActive(isFront);
        }

        public void SetCardLevel(int cardLevel)
        {
            uiEffect.color = uiEffectColors[cardLevel - 1];
        }   

        public void SetCanBePurchased(bool canBePurchased)
        {
            uiEffect.edgeMode = canBePurchased ? EdgeMode.Shiny : EdgeMode.None;
        }
    }
}