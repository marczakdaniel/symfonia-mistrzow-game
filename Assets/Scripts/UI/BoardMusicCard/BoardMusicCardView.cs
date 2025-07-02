using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DefaultNamespace.Data;
using Cysharp.Threading.Tasks;

namespace UI.BoardMusicCard
{
    public class BoardMusicCardView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnCardClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;

        public async UniTask PlayPutOnBoardAnimation()
        {
            // TODO : Play put on board animation
            await UniTask.Delay(1000);
        }

        public async UniTask PlayRevealAnimation()
        {
            // TODO : Play reveal animation
            await UniTask.Delay(1000);
        }

        public async UniTask PlayMovingToPlayerResourcesAnimation()
        {
            // TODO : Play moving to player resources animation
            await UniTask.Delay(1000);
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