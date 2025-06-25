using DefaultNamespace.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using R3;

namespace DefaultNamespace.Views
{
    public class MusicCardView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnCardClicked = new Subject<Unit>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;

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

        void OnDestroy()
        {
            OnCardClicked?.Dispose();
        }
    }
}