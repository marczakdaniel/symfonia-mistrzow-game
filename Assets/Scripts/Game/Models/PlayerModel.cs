using System;
using System.Collections.Generic;
using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Data;

namespace SplendorGame.Game.Models
{
    /// <summary>
    /// Model representing a player in the game
    /// </summary>
    public class PlayerModel : IModel
    {
        private readonly ReactiveProperty<string> _playerName = new("");
        private readonly ReactiveProperty<int> _score = new(0);
        private readonly ReactiveProperty<ResourceCost> _resources = new(new ResourceCost());
        private readonly ReactiveProperty<ResourceCost> _bonuses = new(new ResourceCost());
        private readonly ReactiveProperty<int> _goldTokens = new(0);
        private readonly ReactiveCollection<CardData> _purchasedCards = new();
        private readonly ReactiveCollection<CardData> _reservedCards = new();
        
        // Read-only observables
        public ReadOnlyReactiveProperty<string> PlayerName => _playerName;
        public ReadOnlyReactiveProperty<int> Score => _score;
        public ReadOnlyReactiveProperty<ResourceCost> Resources => _resources;
        public ReadOnlyReactiveProperty<ResourceCost> Bonuses => _bonuses;
        public ReadOnlyReactiveProperty<int> GoldTokens => _goldTokens;
        public ReadOnlyReactiveCollection<CardData> PurchasedCards => _purchasedCards;
        public ReadOnlyReactiveCollection<CardData> ReservedCards => _reservedCards;
        
        private readonly CompositeDisposable _disposables = new();
        
        public void Initialize()
        {
            // Calculate score based on purchased cards
            _purchasedCards.ObserveCountChanged()
                .Subscribe(_ => RecalculateScore())
                .AddTo(_disposables);
            
            // Calculate bonuses from purchased cards
            _purchasedCards.ObserveCountChanged()
                .Subscribe(_ => RecalculateBonuses())
                .AddTo(_disposables);
        }
        
        public void SetPlayerName(string name)
        {
            _playerName.Value = name;
        }
        
        public void AddResources(ResourceType type, int amount)
        {
            var current = _resources.Value.Clone();
            var currentAmount = current.GetCost(type);
            current.SetCost(type, currentAmount + amount);
            _resources.Value = current;
        }
        
        public void RemoveResources(ResourceType type, int amount)
        {
            var current = _resources.Value.Clone();
            var currentAmount = current.GetCost(type);
            current.SetCost(type, Math.Max(0, currentAmount - amount));
            _resources.Value = current;
        }
        
        public void AddGoldTokens(int amount)
        {
            _goldTokens.Value += amount;
        }
        
        public void RemoveGoldTokens(int amount)
        {
            _goldTokens.Value = Math.Max(0, _goldTokens.Value - amount);
        }
        
        public void PurchaseCard(CardData card)
        {
            if (card == null) return;
            
            _purchasedCards.Add(card);
            
            // Remove from reserved cards if it was reserved
            if (_reservedCards.Contains(card))
            {
                _reservedCards.Remove(card);
            }
        }
        
        public void ReserveCard(CardData card)
        {
            if (card == null || _reservedCards.Count >= 3) return;
            
            _reservedCards.Add(card);
            
            // Player gets a gold token when reserving (if available)
            AddGoldTokens(1);
        }
        
        public bool CanAffordCard(CardData card)
        {
            if (card == null) return false;
            
            var goldNeeded = card.CalculateGoldNeeded(_resources.Value, _bonuses.Value);
            return goldNeeded <= _goldTokens.Value;
        }
        
        private void RecalculateScore()
        {
            int totalScore = 0;
            foreach (var card in _purchasedCards)
            {
                totalScore += card.points;
            }
            _score.Value = totalScore;
        }
        
        private void RecalculateBonuses()
        {
            var bonuses = new ResourceCost();
            
            foreach (var card in _purchasedCards)
            {
                var currentBonus = bonuses.GetCost(card.providesResource);
                bonuses.SetCost(card.providesResource, currentBonus + 1);
            }
            
            _bonuses.Value = bonuses;
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
            _playerName?.Dispose();
            _score?.Dispose();
            _resources?.Dispose();
            _bonuses?.Dispose();
            _goldTokens?.Dispose();
            _purchasedCards?.Dispose();
            _reservedCards?.Dispose();
        }
    }
} 