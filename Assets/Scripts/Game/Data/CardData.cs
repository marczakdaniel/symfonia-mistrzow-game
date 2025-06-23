using UnityEngine;
using SplendorGame.Game.Data;

namespace SplendorGame.Game.Data
{
    /// <summary>
    /// ScriptableObject containing card data for Splendor
    /// </summary>
    [CreateAssetMenu(fileName = "New Card", menuName = "Splendor/Card Data")]
    public class CardData : ScriptableObject
    {
        [Header("Card Identity")]
        public int id;
        public int level = 1;
        public int points = 0;
        
        [Header("Card Provides")]
        public ResourceType providesResource = ResourceType.Diamond;
        
        [Header("Card Cost")]
        public ResourceCost cost = new ResourceCost();
        
        [Header("Visual")]
        public Sprite cardArt;
        public Color cardColor = Color.white;
        
        /// <summary>
        /// Check if this card can be purchased with given resources
        /// </summary>
        public bool CanAfford(ResourceCost playerResources, ResourceCost playerBonuses)
        {
            var totalResources = new ResourceCost();
            
            // Add player's actual resources
            totalResources.Diamond = playerResources.Diamond + playerBonuses.Diamond;
            totalResources.Sapphire = playerResources.Sapphire + playerBonuses.Sapphire;
            totalResources.Emerald = playerResources.Emerald + playerBonuses.Emerald;
            totalResources.Ruby = playerResources.Ruby + playerBonuses.Ruby;
            totalResources.Onyx = playerResources.Onyx + playerBonuses.Onyx;
            
            // Check if we have enough of each resource
            return totalResources.Diamond >= cost.Diamond &&
                   totalResources.Sapphire >= cost.Sapphire &&
                   totalResources.Emerald >= cost.Emerald &&
                   totalResources.Ruby >= cost.Ruby &&
                   totalResources.Onyx >= cost.Onyx;
        }
        
        /// <summary>
        /// Calculate how much gold is needed to buy this card
        /// </summary>
        public int CalculateGoldNeeded(ResourceCost playerResources, ResourceCost playerBonuses)
        {
            int goldNeeded = 0;
            
            var totalResources = new ResourceCost();
            totalResources.Diamond = playerResources.Diamond + playerBonuses.Diamond;
            totalResources.Sapphire = playerResources.Sapphire + playerBonuses.Sapphire;
            totalResources.Emerald = playerResources.Emerald + playerBonuses.Emerald;
            totalResources.Ruby = playerResources.Ruby + playerBonuses.Ruby;
            totalResources.Onyx = playerResources.Onyx + playerBonuses.Onyx;
            
            goldNeeded += Mathf.Max(0, cost.Diamond - totalResources.Diamond);
            goldNeeded += Mathf.Max(0, cost.Sapphire - totalResources.Sapphire);
            goldNeeded += Mathf.Max(0, cost.Emerald - totalResources.Emerald);
            goldNeeded += Mathf.Max(0, cost.Ruby - totalResources.Ruby);
            goldNeeded += Mathf.Max(0, cost.Onyx - totalResources.Onyx);
            
            return goldNeeded;
        }
    }
} 