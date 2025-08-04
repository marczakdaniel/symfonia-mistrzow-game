using System.Collections.Generic;
using System.Xml.Serialization;
using Assets.Scripts.UI.Elements;
using BrunoMikoski.AnimationSequencer;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using DefaultNamespace.Elements;
using Mono.Cecil.Cil;
using MusicCardGame.UI.CardPurchaseWindow;
using R3;
using UI.CardPurchaseWindow.CardPurchaseSingleToken;
using UI.MusicCardDetailsPanel;
using UnityEngine;

namespace UI.CardPurchaseWindow
{
    public class CardPurchaseWindowView : MonoBehaviour
    {
        public Subject<Unit> OnCloseButtonClick = new();
        public Subject<Unit> OnConfirmButtonClick = new();

        [SerializeField] private ButtonElement closeButton;
        [SerializeField] private ButtonElement confirmButton;
        [SerializeField] private DetailsMusicCardView musicCardView;
        [SerializeField] private CardPurchaseSingleTokenView[] cardPurchaseSingleTokenViews = new CardPurchaseSingleTokenView[6];
        [SerializeField] private UniversalPlayerResourceElement[] playerResourcesElements = new UniversalPlayerResourceElement[6];

        [SerializeField] private AnimationSequencerController openAnimationFromMusicCardDetailsPanel;
        [SerializeField] private AnimationSequencerController closeAnimationToMusicCardDetailsPanel;
        [SerializeField] private CardPurchaseWindowPurchaseAnimation purchaseAnimation;
        public CardPurchaseSingleTokenView[] CardPurchaseSingleTokenViews => cardPurchaseSingleTokenViews;

        public void Awake()
        {
            closeButton.OnClick.Subscribe(OnCloseButtonClick.OnNext).AddTo(this);
            confirmButton.OnClick.Subscribe(OnConfirmButtonClick.OnNext).AddTo(this);
        }
        
        public void Setup(Dictionary<ResourceType, int> currentPlayerTokens, Dictionary<ResourceType, int> currentCardTokens)
        {
            for (int i = 0; i < 6; i++)
            {
                playerResourcesElements[i].Initialize((ResourceType)i, currentPlayerTokens[(ResourceType)i], currentCardTokens[(ResourceType)i]);
            }
        }

        public void SetCardDetails(MusicCardData musicCardData)
        {
            musicCardView.Setup(musicCardData);
        }

        public void SetCanBePurchased(bool canBePurchased)
        {
            musicCardView.SetCanBePurchased(canBePurchased);
        }

        public async UniTask PlayOpenAnimation()
        {
            await openAnimationFromMusicCardDetailsPanel.PlayAsync();
        }

        public async UniTask PlayCloseAnimation()
        {
            await closeAnimationToMusicCardDetailsPanel.PlayAsync();
        }

        public async UniTask PlayPurchaseAnimation(int playerIndex)
        {
            await purchaseAnimation.PlayPurchaseAnimation(playerIndex);
        }
    }
}