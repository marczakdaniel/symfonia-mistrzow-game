using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using R3;
using System;

namespace DefaultNamespace.Views
{
    public class BoardMusicCardView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnCardClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;

        private void Awake()
        {
            ValidateComponents();
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

        public void OnPointerClick(PointerEventData eventData)
        {
            OnCardClicked?.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            OnCardClicked?.Dispose();
        }
    }
}