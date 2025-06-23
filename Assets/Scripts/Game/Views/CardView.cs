using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Data;
using System;

namespace SplendorGame.Game.Views
{
    /// <summary>
    /// View component for displaying a single card
    /// </summary>
    public class CardView : MonoBehaviour, IView
    {
        [Header("UI References")]
        [SerializeField] private Image cardBackground;
        [SerializeField] private Image cardArt;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image providesResourceIcon;
        
        [Header("Cost Display")]
        [SerializeField] private Transform costContainer;
        [SerializeField] private GameObject costItemPrefab;
        
        [Header("Interaction")]
        [SerializeField] private Button cardButton;
        [SerializeField] private Button reserveButton;
        
        private CardData _cardData;
        private readonly Subject<CardData> _onCardClicked = new();
        private readonly Subject<CardData> _onReserveClicked = new();
        private readonly CompositeDisposable _disposables = new();
        
        // Events
        public Observable<CardData> OnCardClicked => _onCardClicked;
        public Observable<CardData> OnReserveClicked => _onReserveClicked;
        
        public void Initialize()
        {
            if (cardButton != null)
            {
                cardButton.OnClickAsObservable()
                    .Subscribe(_ => _onCardClicked.OnNext(_cardData))
                    .AddTo(_disposables);
            }
            
            if (reserveButton != null)
            {
                reserveButton.OnClickAsObservable()
                    .Subscribe(_ => _onReserveClicked.OnNext(_cardData))
                    .AddTo(_disposables);
            }
        }
        
        public void SetCardData(CardData cardData)
        {
            _cardData = cardData;
            UpdateDisplay();
        }
        
        public void SetInteractable(bool interactable)
        {
            if (cardButton != null)
                cardButton.interactable = interactable;
            
            if (reserveButton != null)
                reserveButton.interactable = interactable;
        }
        
        public void SetAffordability(bool canAfford, bool canReserve)
        {
            if (cardButton != null)
            {
                cardButton.interactable = canAfford;
                var colors = cardButton.colors;
                colors.normalColor = canAfford ? Color.white : Color.gray;
                cardButton.colors = colors;
            }
            
            if (reserveButton != null)
            {
                reserveButton.interactable = canReserve;
            }
        }
        
        private void UpdateDisplay()
        {
            if (_cardData == null) return;
            
            // Update basic card info
            if (pointsText != null)
                pointsText.text = _cardData.points.ToString();
            
            if (levelText != null)
                levelText.text = $"Level {_cardData.level}";
            
            // Update card visual
            if (cardBackground != null)
                cardBackground.color = _cardData.cardColor;
            
            if (cardArt != null && _cardData.cardArt != null)
                cardArt.sprite = _cardData.cardArt;
            
            // Update provides resource icon
            if (providesResourceIcon != null)
            {
                providesResourceIcon.color = GetResourceColor(_cardData.providesResource);
            }
            
            // Update cost display
            UpdateCostDisplay();
        }
        
        private void UpdateCostDisplay()
        {
            if (costContainer == null || costItemPrefab == null) return;
            
            // Clear existing cost items
            foreach (Transform child in costContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Create cost items for non-zero costs
            foreach (var (type, amount) in _cardData.cost.GetNonZeroCosts())
            {
                var costItem = Instantiate(costItemPrefab, costContainer);
                var costItemView = costItem.GetComponent<ResourceCostItemView>();
                
                if (costItemView != null)
                {
                    costItemView.SetResourceCost(type, amount);
                }
            }
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
            _onCardClicked?.Dispose();
            _onReserveClicked?.Dispose();
        }
        
        private void OnDestroy()
        {
            Dispose();
        }
    }
    
    /// <summary>
    /// Helper component for displaying individual resource cost items
    /// </summary>
    public class ResourceCostItemView : MonoBehaviour
    {
        [SerializeField] private Image resourceIcon;
        [SerializeField] private TextMeshProUGUI amountText;
        
        public void SetResourceCost(ResourceType resourceType, int amount)
        {
            if (resourceIcon != null)
            {
                resourceIcon.color = GetResourceColor(resourceType);
            }
            
            if (amountText != null)
            {
                amountText.text = amount.ToString();
            }
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