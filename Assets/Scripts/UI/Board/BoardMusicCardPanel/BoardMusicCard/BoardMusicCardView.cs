using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DefaultNamespace.Data;
using Cysharp.Threading.Tasks;

namespace UI.Board.BoardMusicCardPanel.BoardMusicCard
{
    public class BoardMusicCardView : MonoBehaviour, IPointerClickHandler
    {
        public Subject<Unit> OnCardClicked { get; private set; } = new Subject<Unit>();

        [SerializeField] private MusicCardCostView costView;
        [SerializeField] private Image cardImage;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Image resourceProvidedImage;

        [SerializeField] private BoardMusicCardAnimationController animationController;

        public UniTask PlayPutOnBoardAnimation()
        {
            return animationController.PlayPutOnBoardAnimation();
        }

        public async UniTask PlayRevealAnimation()
        {
            // TODO : Play reveal animation
            await animationController.ResetPutOnBoardAnimation();
            await UniTask.Delay(1);
        }

        public async UniTask PlayMovingToPlayerResourcesAnimation()
        {
            // TODO : Play moving to player resources animation
            await UniTask.Delay(1000);
        }

        public void Setup(IMusicCardDataReader card)
        {
            costView.Setup(card.Cost);
            cardImage.sprite = card.CardImage;
            pointsText.text = card.Points.ToString();
            resourceProvidedImage.sprite = card.ResourceProvided.GetSingleResourceTypeImages().StackImage1;
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