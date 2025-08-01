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
        public Subject<string> OnCardClicked { get; private set; } = new Subject<string>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;
        [SerializeField] private ButtonElement button;
        [SerializeField] private GameObject level1Frame;
        [SerializeField] private GameObject level2Frame;
        [SerializeField] private GameObject level3Frame;

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

            switch (card.level)
            {
                case 1:
                    level1Frame.SetActive(true);
                    level2Frame.SetActive(false);
                    level3Frame.SetActive(false);
                    break;
                case 2:
                    level1Frame.SetActive(false);
                    level2Frame.SetActive(true);
                    level3Frame.SetActive(false);
                    break;
                case 3:
                    level1Frame.SetActive(false);
                    level2Frame.SetActive(false);
                    level3Frame.SetActive(true);
                    break;
                default:
                    level1Frame.SetActive(true);
                    level2Frame.SetActive(false);
                    level3Frame.SetActive(false);
                    break;
            }
        }
    }
}