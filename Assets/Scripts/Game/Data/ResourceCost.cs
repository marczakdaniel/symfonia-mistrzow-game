using System;
using System.Collections.Generic;
using UnityEngine;

namespace SplendorGame.Game.Data
{
    /// <summary>
    /// Represents the cost of resources for cards or actions
    /// </summary>
    [Serializable]
    public class ResourceCost
    {
        [SerializeField] private int diamond;
        [SerializeField] private int sapphire;
        [SerializeField] private int emerald;
        [SerializeField] private int ruby;
        [SerializeField] private int onyx;
        
        public int Diamond { get => diamond; set => diamond = value; }
        public int Sapphire { get => sapphire; set => sapphire = value; }
        public int Emerald { get => emerald; set => emerald = value; }
        public int Ruby { get => ruby; set => ruby = value; }
        public int Onyx { get => onyx; set => onyx = value; }
        
        public ResourceCost()
        {
        }
        
        public ResourceCost(int diamond, int sapphire, int emerald, int ruby, int onyx)
        {
            this.diamond = diamond;
            this.sapphire = sapphire;
            this.emerald = emerald;
            this.ruby = ruby;
            this.onyx = onyx;
        }
        
        public int GetCost(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Diamond => diamond,
                ResourceType.Sapphire => sapphire,
                ResourceType.Emerald => emerald,
                ResourceType.Ruby => ruby,
                ResourceType.Onyx => onyx,
                _ => 0
            };
        }
        
        public void SetCost(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Diamond:
                    diamond = amount;
                    break;
                case ResourceType.Sapphire:
                    sapphire = amount;
                    break;
                case ResourceType.Emerald:
                    emerald = amount;
                    break;
                case ResourceType.Ruby:
                    ruby = amount;
                    break;
                case ResourceType.Onyx:
                    onyx = amount;
                    break;
            }
        }
        
        public int TotalCost => diamond + sapphire + emerald + ruby + onyx;
        
        public IEnumerable<(ResourceType type, int amount)> GetNonZeroCosts()
        {
            if (diamond > 0) yield return (ResourceType.Diamond, diamond);
            if (sapphire > 0) yield return (ResourceType.Sapphire, sapphire);
            if (emerald > 0) yield return (ResourceType.Emerald, emerald);
            if (ruby > 0) yield return (ResourceType.Ruby, ruby);
            if (onyx > 0) yield return (ResourceType.Onyx, onyx);
        }
        
        public ResourceCost Clone()
        {
            return new ResourceCost(diamond, sapphire, emerald, ruby, onyx);
        }
    }
} 