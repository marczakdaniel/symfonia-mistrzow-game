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

        public void DisableCard()
        {
            gameObject.SetActive(false);
        }
        
        public void EnableCard()
        {
            gameObject.SetActive(true);
        }

        public UniTask PlayPutOnBoardAnimation()
        {
            return UniTask.CompletedTask;
        }

        public UniTask PlayRevealAnimation()
        {
            // TODO : Play reveal animation
            return UniTask.CompletedTask;
        }

        public UniTask PlayMovingToPlayerResourcesAnimation()
        {
            // TODO : Play moving to player resources animation
            return UniTask.CompletedTask;
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