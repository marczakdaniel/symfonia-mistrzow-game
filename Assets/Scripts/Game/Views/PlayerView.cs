using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Data;
using System.Collections.Generic;

namespace SplendorGame.Game.Views
{
    /// <summary>
    /// View component for displaying player information
    /// </summary>
    public class PlayerView : MonoBehaviour, IView
    {
        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image playerIndicator;
        
        [Header("Resources")]
        [SerializeField] private Transform resourcesContainer;
        [SerializeField] private GameObject resourceItemPrefab;
        [SerializeField] private TextMeshProUGUI goldTokensText;
        
        [Header("Bonuses")]
        [SerializeField] private Transform bonusesContainer;
        [SerializeField] private GameObject bonusItemPrefab;
        
        [Header("Cards")]
        [SerializeField] private Transform purchasedCardsContainer;
        [SerializeField] private Transform reservedCardsContainer;
        [SerializeField] private GameObject cardViewPrefab;
        
        private readonly Dictionary<ResourceType, ResourceDisplayItem> _resourceItems = new();
        private readonly Dictionary<ResourceType, ResourceDisplayItem> _bonusItems = new();
        private readonly List<CardView> _purchasedCardViews = new();
        private readonly List<CardView> _reservedCardViews = new();
        
        private readonly CompositeDisposable _disposables = new();
        
        public void Initialize()
        {
            InitializeResourceDisplay();
            InitializeBonusDisplay();
        }
        
        public void SetPlayerName(string playerName)
        {
            if (playerNameText != null)
                playerNameText.text = playerName;
        }
        
        public void SetScore(int score)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {score}";
        }
        
        public void SetCurrentPlayer(bool isCurrentPlayer)
        {
            if (playerIndicator != null)
            {
                playerIndicator.color = isCurrentPlayer ? Color.green : Color.gray;
            }
        }
        
        public void UpdateResources(ResourceCost resources)
        {
            _resourceItems[ResourceType.Diamond].SetAmount(resources.Diamond);
            _resourceItems[ResourceType.Sapphire].SetAmount(resources.Sapphire);
            _resourceItems[ResourceType.Emerald].SetAmount(resources.Emerald);
            _resourceItems[ResourceType.Ruby].SetAmount(resources.Ruby);
            _resourceItems[ResourceType.Onyx].SetAmount(resources.Onyx);
        }
        
        public void UpdateGoldTokens(int goldTokens)
        {
            if (goldTokensText != null)
                goldTokensText.text = goldTokens.ToString();
        }
        
        public void UpdateBonuses(ResourceCost bonuses)
        {
            _bonusItems[ResourceType.Diamond].SetAmount(bonuses.Diamond);
            _bonusItems[ResourceType.Sapphire].SetAmount(bonuses.Sapphire);
            _bonusItems[ResourceType.Emerald].SetAmount(bonuses.Emerald);
            _bonusItems[ResourceType.Ruby].SetAmount(bonuses.Ruby);
            _bonusItems[ResourceType.Onyx].SetAmount(bonuses.Onyx);
        }
        
        public void UpdatePurchasedCards(IReadOnlyList<CardData> cards)
        {
            // Clear existing views
            foreach (var cardView in _purchasedCardViews)
            {
                if (cardView != null)
                    Destroy(cardView.gameObject);
            }
            _purchasedCardViews.Clear();
            
            // Create new card views
            foreach (var card in cards)
            {
                var cardObj = Instantiate(cardViewPrefab, purchasedCardsContainer);
                var cardView = cardObj.GetComponent<CardView>();
                
                if (cardView != null)
                {
                    cardView.Initialize();
                    cardView.SetCardData(card);
                    cardView.SetInteractable(false); // Purchased cards are not interactable
                    _purchasedCardViews.Add(cardView);
                }
            }
        }
        
        public void UpdateReservedCards(IReadOnlyList<CardData> cards)
        {
            // Clear existing views
            foreach (var cardView in _reservedCardViews)
            {
                if (cardView != null)
                    Destroy(cardView.gameObject);
            }
            _reservedCardViews.Clear();
            
            // Create new card views
            foreach (var card in cards)
            {
                var cardObj = Instantiate(cardViewPrefab, reservedCardsContainer);
                var cardView = cardObj.GetComponent<CardView>();
                
                if (cardView != null)
                {
                    cardView.Initialize();
                    cardView.SetCardData(card);
                    _reservedCardViews.Add(cardView);
                }
            }
        }
        
        private void InitializeResourceDisplay()
        {
            if (resourcesContainer == null || resourceItemPrefab == null) return;
            
            var resourceTypes = new[] {
                ResourceType.Diamond,
                ResourceType.Sapphire,
                ResourceType.Emerald,
                ResourceType.Ruby,
                ResourceType.Onyx
            };
            
            foreach (var resourceType in resourceTypes)
            {
                var resourceObj = Instantiate(resourceItemPrefab, resourcesContainer);
                var resourceItem = resourceObj.GetComponent<ResourceDisplayItem>();
                
                if (resourceItem != null)
                {
                    resourceItem.Initialize(resourceType);
                    _resourceItems[resourceType] = resourceItem;
                }
            }
        }
        
        private void InitializeBonusDisplay()
        {
            if (bonusesContainer == null || bonusItemPrefab == null) return;
            
            var resourceTypes = new[] {
                ResourceType.Diamond,
                ResourceType.Sapphire,
                ResourceType.Emerald,
                ResourceType.Ruby,
                ResourceType.Onyx
            };
            
            foreach (var resourceType in resourceTypes)
            {
                var bonusObj = Instantiate(bonusItemPrefab, bonusesContainer);
                var bonusItem = bonusObj.GetComponent<ResourceDisplayItem>();
                
                if (bonusItem != null)
                {
                    bonusItem.Initialize(resourceType);
                    _bonusItems[resourceType] = bonusItem;
                }
            }
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
            
            foreach (var cardView in _purchasedCardViews)
            {
                cardView?.Dispose();
            }
            
            foreach (var cardView in _reservedCardViews)
            {
                cardView?.Dispose();
            }
        }
        
        private void OnDestroy()
        {
            Dispose();
        }
    }
    
    /// <summary>
    /// Helper component for displaying resource amounts
    /// </summary>
    public class ResourceDisplayItem : MonoBehaviour
    {
        [SerializeField] private Image resourceIcon;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private GameObject container;
        
        private ResourceType _resourceType;
        
        public void Initialize(ResourceType resourceType)
        {
            _resourceType = resourceType;
            
            if (resourceIcon != null)
            {
                resourceIcon.color = GetResourceColor(resourceType);
            }
        }
        
        public void SetAmount(int amount)
        {
            if (amountText != null)
                amountText.text = amount.ToString();
            
            // Hide if amount is 0
            if (container != null)
                container.SetActive(amount > 0);
        }
        
        private Color GetResourceColor(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Diamond => Color.white,
                ResourceType.Sapphire => Color.blue,
                ResourceType.Emerald => Color.green,
                ResourceType.Ruby => Color.red,
                ResourceType.Onyx => Color.black,
                ResourceType.Gold => Color.yellow,
                _ => Color.gray
            };
        }
    }
} 