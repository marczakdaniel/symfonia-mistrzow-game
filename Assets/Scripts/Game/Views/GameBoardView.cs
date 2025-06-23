using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Data;
using System.Collections.Generic;
using System;

namespace SplendorGame.Game.Views
{
    /// <summary>
    /// Main game board view displaying cards, tokens, and game state
    /// </summary>
    public class GameBoardView : MonoBehaviour, IView
    {
        [Header("Game Info")]
        [SerializeField] private TextMeshProUGUI currentPlayerText;
        [SerializeField] private TextMeshProUGUI turnNumberText;
        [SerializeField] private TextMeshProUGUI gamePhaseText;
        
        [Header("Available Cards")]
        [SerializeField] private Transform level1CardsContainer;
        [SerializeField] private Transform level2CardsContainer;
        [SerializeField] private Transform level3CardsContainer;
        [SerializeField] private GameObject cardViewPrefab;
        
        [Header("Token Pool")]
        [SerializeField] private Transform tokenPoolContainer;
        [SerializeField] private GameObject tokenItemPrefab;
        [SerializeField] private TextMeshProUGUI goldTokensText;
        
        [Header("Token Selection")]
        [SerializeField] private Button takeTokensButton;
        [SerializeField] private GameObject tokenSelectionPanel;
        [SerializeField] private Toggle[] tokenToggles;
        [SerializeField] private Button confirmTokensButton;
        [SerializeField] private Button cancelTokensButton;
        
        [Header("Players")]
        [SerializeField] private Transform playersContainer;
        [SerializeField] private GameObject playerViewPrefab;
        
        private readonly List<CardView> _level1CardViews = new();
        private readonly List<CardView> _level2CardViews = new();
        private readonly List<CardView> _level3CardViews = new();
        private readonly List<PlayerView> _playerViews = new();
        private readonly Dictionary<ResourceType, ResourceDisplayItem> _tokenItems = new();
        
        private readonly Subject<CardData> _onCardPurchase = new();
        private readonly Subject<CardData> _onCardReserve = new();
        private readonly Subject<ResourceType[]> _onTokensTaken = new();
        private readonly CompositeDisposable _disposables = new();
        
        // Events
        public Observable<CardData> OnCardPurchase => _onCardPurchase;
        public Observable<CardData> OnCardReserve => _onCardReserve;
        public Observable<ResourceType[]> OnTokensTaken => _onTokensTaken;
        
        public void Initialize()
        {
            InitializeTokenPool();
            InitializeTokenSelection();
            
            if (takeTokensButton != null)
            {
                takeTokensButton.OnClickAsObservable()
                    .Subscribe(_ => ShowTokenSelection())
                    .AddTo(_disposables);
            }
        }
        
        public void SetCurrentPlayer(string playerName)
        {
            if (currentPlayerText != null)
                currentPlayerText.text = $"Current Player: {playerName}";
        }
        
        public void SetTurnNumber(int turnNumber)
        {
            if (turnNumberText != null)
                turnNumberText.text = $"Turn: {turnNumber}";
        }
        
        public void SetGamePhase(string phase)
        {
            if (gamePhaseText != null)
                gamePhaseText.text = phase;
        }
        
        public void UpdateAvailableCards(IReadOnlyList<CardData> level1, IReadOnlyList<CardData> level2, IReadOnlyList<CardData> level3)
        {
            UpdateCardLevel(_level1CardViews, level1CardsContainer, level1);
            UpdateCardLevel(_level2CardViews, level2CardsContainer, level2);
            UpdateCardLevel(_level3CardViews, level3CardsContainer, level3);
        }
        
        public void UpdateTokenPool(ResourceCost availableTokens, int goldTokens)
        {
            _tokenItems[ResourceType.Diamond].SetAmount(availableTokens.Diamond);
            _tokenItems[ResourceType.Sapphire].SetAmount(availableTokens.Sapphire);
            _tokenItems[ResourceType.Emerald].SetAmount(availableTokens.Emerald);
            _tokenItems[ResourceType.Ruby].SetAmount(availableTokens.Ruby);
            _tokenItems[ResourceType.Onyx].SetAmount(availableTokens.Onyx);
            
            if (goldTokensText != null)
                goldTokensText.text = goldTokens.ToString();
        }
        
        public void UpdatePlayers(IReadOnlyList<PlayerView> playerViews)
        {
            // Clear existing player views
            foreach (var playerView in _playerViews)
            {
                if (playerView != null)
                    Destroy(playerView.gameObject);
            }
            _playerViews.Clear();
            
            // Add new player views
            foreach (var playerView in playerViews)
            {
                _playerViews.Add(playerView);
            }
        }
        
        public void SetInteractable(bool interactable)
        {
            // Enable/disable card interactions
            foreach (var cardView in _level1CardViews)
                cardView?.SetInteractable(interactable);
            
            foreach (var cardView in _level2CardViews)
                cardView?.SetInteractable(interactable);
            
            foreach (var cardView in _level3CardViews)
                cardView?.SetInteractable(interactable);
            
            // Enable/disable token taking
            if (takeTokensButton != null)
                takeTokensButton.interactable = interactable;
        }
        
        public void UpdateCardAffordability(CardData card, bool canAfford, bool canReserve)
        {
            var cardView = FindCardView(card);
            cardView?.SetAffordability(canAfford, canReserve);
        }
        
        private void UpdateCardLevel(List<CardView> cardViews, Transform container, IReadOnlyList<CardData> cards)
        {
            // Clear existing views
            foreach (var cardView in cardViews)
            {
                if (cardView != null)
                    Destroy(cardView.gameObject);
            }
            cardViews.Clear();
            
            // Create new card views
            foreach (var card in cards)
            {
                var cardObj = Instantiate(cardViewPrefab, container);
                var cardView = cardObj.GetComponent<CardView>();
                
                if (cardView != null)
                {
                    cardView.Initialize();
                    cardView.SetCardData(card);
                    
                    // Subscribe to card events
                    cardView.OnCardClicked
                        .Subscribe(clickedCard => _onCardPurchase.OnNext(clickedCard))
                        .AddTo(_disposables);
                    
                    cardView.OnReserveClicked
                        .Subscribe(reservedCard => _onCardReserve.OnNext(reservedCard))
                        .AddTo(_disposables);
                    
                    cardViews.Add(cardView);
                }
            }
        }
        
        private CardView FindCardView(CardData card)
        {
            foreach (var cardView in _level1CardViews)
            {
                if (cardView != null && ReferenceEquals(cardView.GetComponent<CardView>(), card))
                    return cardView;
            }
            
            foreach (var cardView in _level2CardViews)
            {
                if (cardView != null && ReferenceEquals(cardView.GetComponent<CardView>(), card))
                    return cardView;
            }
            
            foreach (var cardView in _level3CardViews)
            {
                if (cardView != null && ReferenceEquals(cardView.GetComponent<CardView>(), card))
                    return cardView;
            }
            
            return null;
        }
        
        private void InitializeTokenPool()
        {
            if (tokenPoolContainer == null || tokenItemPrefab == null) return;
            
            var resourceTypes = new[] {
                ResourceType.Diamond,
                ResourceType.Sapphire,
                ResourceType.Emerald,
                ResourceType.Ruby,
                ResourceType.Onyx
            };
            
            foreach (var resourceType in resourceTypes)
            {
                var tokenObj = Instantiate(tokenItemPrefab, tokenPoolContainer);
                var tokenItem = tokenObj.GetComponent<ResourceDisplayItem>();
                
                if (tokenItem != null)
                {
                    tokenItem.Initialize(resourceType);
                    _tokenItems[resourceType] = tokenItem;
                }
            }
        }
        
        private void InitializeTokenSelection()
        {
            if (confirmTokensButton != null)
            {
                confirmTokensButton.OnClickAsObservable()
                    .Subscribe(_ => ConfirmTokenSelection())
                    .AddTo(_disposables);
            }
            
            if (cancelTokensButton != null)
            {
                cancelTokensButton.OnClickAsObservable()
                    .Subscribe(_ => HideTokenSelection())
                    .AddTo(_disposables);
            }
            
            if (tokenSelectionPanel != null)
                tokenSelectionPanel.SetActive(false);
        }
        
        private void ShowTokenSelection()
        {
            if (tokenSelectionPanel != null)
                tokenSelectionPanel.SetActive(true);
        }
        
        private void HideTokenSelection()
        {
            if (tokenSelectionPanel != null)
                tokenSelectionPanel.SetActive(false);
            
            // Reset toggles
            if (tokenToggles != null)
            {
                foreach (var toggle in tokenToggles)
                {
                    if (toggle != null)
                        toggle.isOn = false;
                }
            }
        }
        
        private void ConfirmTokenSelection()
        {
            if (tokenToggles == null) return;
            
            var selectedTypes = new List<ResourceType>();
            
            for (int i = 0; i < tokenToggles.Length && i < 5; i++)
            {
                if (tokenToggles[i] != null && tokenToggles[i].isOn)
                {
                    selectedTypes.Add((ResourceType)i);
                }
            }
            
            if (selectedTypes.Count > 0)
            {
                _onTokensTaken.OnNext(selectedTypes.ToArray());
            }
            
            HideTokenSelection();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
            _onCardPurchase?.Dispose();
            _onCardReserve?.Dispose();
            _onTokensTaken?.Dispose();
            
            foreach (var cardView in _level1CardViews)
                cardView?.Dispose();
            
            foreach (var cardView in _level2CardViews)
                cardView?.Dispose();
            
            foreach (var cardView in _level3CardViews)
                cardView?.Dispose();
            
            foreach (var playerView in _playerViews)
                playerView?.Dispose();
        }
        
        private void OnDestroy()
        {
            Dispose();
        }
    }
} 