using System.Collections.Generic;
using Assets.Scripts.UI.Elements;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using R3;
using TMPro;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.PlayerResourcesWindow
{
    public class PlayerResourcesWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClicked { get; private set; } = new Subject<Unit>();
        public Subject<(string, int)> OnCardClicked { get; private set; } = new Subject<(string, int)>();

        [SerializeField] 
        private UniversalPlayerResourceElement[] playerResources = new UniversalPlayerResourceElement[6];

        [SerializeField]
        private DetailsMusicCardView[] detailsMusicCardView = new DetailsMusicCardView[3];

        [SerializeField]
        private ButtonElement closeButton;

        [SerializeField]
        private AnimationSequencerController openAnimation;

        [SerializeField]
        private AnimationSequencerController closeAnimation;

        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [SerializeField]
        private TextMeshProUGUI numberOfPointsText;

        public async UniTask PlayOpenAnimation()
        {
            await openAnimation.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimation.PlayAsync();
        }

        public void Initialize(string playerName, int numberOfPoints, Dictionary<ResourceType, int> currentPlayerTokens, Dictionary<ResourceType, int> currentPlayerCards, List<MusicCardData> reservedMusicCards)
        {
            playerNameText.text = playerName;
            numberOfPointsText.text = numberOfPoints.ToString();

            for (int i = 0; i < playerResources.Length; i++)
            {
                playerResources[i].Initialize((ResourceType)i, currentPlayerTokens[(ResourceType)i], currentPlayerCards[(ResourceType)i]);
            }

            for (int i = 0; i < detailsMusicCardView.Length; i++)
            {
                if (i < reservedMusicCards.Count)
                {
                    detailsMusicCardView[i].gameObject.SetActive(true);
                    detailsMusicCardView[i].Setup(reservedMusicCards[i]);
                }
                else
                {
                    detailsMusicCardView[i].gameObject.SetActive(false);
                }
            }
        }

        public async UniTask HideCard(int cardIndex)
        {
            await UniTask.Delay(50);
            detailsMusicCardView[cardIndex].gameObject.SetActive(false);
        }
        public void ShowCard(int cardIndex)
        {
            detailsMusicCardView[cardIndex].gameObject.SetActive(true);
        }

        public void Awake()
        {
            closeButton.OnClick.Subscribe(OnCloseButtonClicked.OnNext).AddTo(this);
            
            for (int cardIndex = 0; cardIndex < detailsMusicCardView.Length; cardIndex++)
            {
                var card = detailsMusicCardView[cardIndex];
                var capturedCardIndex = cardIndex; // Lokalna kopia dla closure
                if (card != null)
                {
                    Debug.Log($"Card clicked: {card.gameObject.name}, card index: {cardIndex}");
                    card.OnCardClicked.Subscribe(cardId => OnCardClicked.OnNext((cardId, capturedCardIndex))).AddTo(this);
                }
            }
        }
    }
}