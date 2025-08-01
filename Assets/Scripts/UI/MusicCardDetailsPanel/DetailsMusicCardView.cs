using DefaultNamespace.Data;
using TMPro;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using UnityEngine;
using UnityEngine.UI;
using R3;
using DefaultNamespace.Elements;

namespace UI.MusicCardDetailsPanel {
    public class DetailsMusicCardView : MonoBehaviour 
    {
        public Subject<Unit> OnCardClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;
        [SerializeField] private ButtonElement button;

        private void Awake()
        {
            ValidateComponents();
            button.OnClick.Subscribe(_ => OnCardClicked.OnNext(Unit.Default)).AddTo(this);
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
            costView.Setup(card.cost);
            cardImage.sprite = card.cardImage;
            pointsText.text = card.points.ToString();
            resourceProvidedImage.sprite = card.resourceProvided.GetSingleResourceTypeImages().StackImage1;
        }
    }
}