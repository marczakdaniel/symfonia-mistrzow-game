using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Data;

namespace SplendorGame.Game.Models
{
    public enum GamePhase
    {
        WaitingForPlayers,
        GameStarted,
        PlayerTurn,
        GameEnded
    }
    
    public enum TurnAction
    {
        TakeTokens,
        BuyCard,
        ReserveCard
    }
    
    /// <summary>
    /// Main game state model managing the entire game
    /// </summary>
    public class GameStateModel : IModel
    {
        private readonly ReactiveProperty<GamePhase> _currentPhase = new(GamePhase.WaitingForPlayers);
        private readonly ReactiveProperty<int> _currentPlayerIndex = new(0);
        private readonly ReactiveProperty<int> _turnNumber = new(1);
        private readonly ReactiveProperty<PlayerModel> _winner = new();
        
        private readonly ReactiveCollection<PlayerModel> _players = new();
        private readonly ReactiveProperty<ResourceCost> _availableTokens = new();
        private readonly ReactiveProperty<int> _availableGoldTokens = new(5);
        
        // Card pools for each level
        private readonly ReactiveCollection<CardData> _level1Cards = new();
        private readonly ReactiveCollection<CardData> _level2Cards = new();
        private readonly ReactiveCollection<CardData> _level3Cards = new();
        
        // Available cards on table (4 of each level)
        private readonly ReactiveCollection<CardData> _availableLevel1Cards = new();
        private readonly ReactiveCollection<CardData> _availableLevel2Cards = new();
        private readonly ReactiveCollection<CardData> _availableLevel3Cards = new();
        
        // Read-only observables
        public ReadOnlyReactiveProperty<GamePhase> CurrentPhase => _currentPhase;
        public ReadOnlyReactiveProperty<int> CurrentPlayerIndex => _currentPlayerIndex;
        public ReadOnlyReactiveProperty<int> TurnNumber => _turnNumber;
        public ReadOnlyReactiveProperty<PlayerModel> Winner => _winner;
        public ReadOnlyReactiveCollection<PlayerModel> Players => _players;
        public ReadOnlyReactiveProperty<ResourceCost> AvailableTokens => _availableTokens;
        public ReadOnlyReactiveProperty<int> AvailableGoldTokens => _availableGoldTokens;
        public ReadOnlyReactiveCollection<CardData> AvailableLevel1Cards => _availableLevel1Cards;
        public ReadOnlyReactiveCollection<CardData> AvailableLevel2Cards => _availableLevel2Cards;
        public ReadOnlyReactiveCollection<CardData> AvailableLevel3Cards => _availableLevel3Cards;
        
        public PlayerModel CurrentPlayer => _players.Count > 0 ? _players[_currentPlayerIndex.Value] : null;
        
        private readonly CompositeDisposable _disposables = new();
        
        // Game settings
        private const int WINNING_SCORE = 15;
        private const int MAX_HAND_SIZE = 10;
        
        public void Initialize()
        {
            InitializeTokens();
            
            // Subscribe to player score changes to check for winner
            _players.ObserveAdd()
                .Subscribe(addEvent => 
                {
                    addEvent.Value.Score
                        .Subscribe(score => CheckForWinner())
                        .AddTo(_disposables);
                })
                .AddTo(_disposables);
        }
        
        public void AddPlayer(string playerName)
        {
            if (_currentPhase.Value != GamePhase.WaitingForPlayers) return;
            
            var player = new PlayerModel();
            player.Initialize();
            player.SetPlayerName(playerName);
            _players.Add(player);
        }
        
        public void StartGame(CardData[] level1Cards, CardData[] level2Cards, CardData[] level3Cards)
        {
            if (_players.Count < 2) return;
            
            InitializeCardPools(level1Cards, level2Cards, level3Cards);
            RefillAvailableCards();
            
            _currentPhase.Value = GamePhase.GameStarted;
            _currentPlayerIndex.Value = 0;
            _turnNumber.Value = 1;
            
            StartPlayerTurn();
        }
        
        public bool TakeTokens(ResourceType type1, ResourceType type2 = ResourceType.Gold, ResourceType type3 = ResourceType.Gold)
        {
            if (_currentPhase.Value != GamePhase.PlayerTurn) return false;
            
            var currentPlayer = CurrentPlayer;
            if (currentPlayer == null) return false;
            
            var availableTokens = _availableTokens.Value;
            bool isDoubleTake = type2 == ResourceType.Gold && type3 == ResourceType.Gold && type1 != ResourceType.Gold;
            
            if (isDoubleTake)
            {
                // Taking 2 of the same type
                if (availableTokens.GetCost(type1) < 4) return false; // Need at least 4 to take 2
                
                RemoveTokensFromPool(type1, 2);
                currentPlayer.AddResources(type1, 2);
            }
            else
            {
                // Taking 3 different types
                if (type1 == type2 || type1 == type3 || type2 == type3) return false;
                if (type1 == ResourceType.Gold || type2 == ResourceType.Gold || type3 == ResourceType.Gold) return false;
                
                if (availableTokens.GetCost(type1) < 1 || 
                    availableTokens.GetCost(type2) < 1 || 
                    availableTokens.GetCost(type3) < 1) return false;
                
                RemoveTokensFromPool(type1, 1);
                RemoveTokensFromPool(type2, 1);
                RemoveTokensFromPool(type3, 1);
                
                currentPlayer.AddResources(type1, 1);
                currentPlayer.AddResources(type2, 1);
                currentPlayer.AddResources(type3, 1);
            }
            
            EndPlayerTurn();
            return true;
        }
        
        public bool BuyCard(CardData card)
        {
            if (_currentPhase.Value != GamePhase.PlayerTurn) return false;
            
            var currentPlayer = CurrentPlayer;
            if (currentPlayer == null || !currentPlayer.CanAffordCard(card)) return false;
            
            // Calculate cost and pay for card
            var goldNeeded = card.CalculateGoldNeeded(currentPlayer.Resources.Value, currentPlayer.Bonuses.Value);
            var actualCost = card.cost.Clone();
            
            // Reduce cost by bonuses
            var bonuses = currentPlayer.Bonuses.Value;
            actualCost.Diamond = Math.Max(0, actualCost.Diamond - bonuses.Diamond);
            actualCost.Sapphire = Math.Max(0, actualCost.Sapphire - bonuses.Sapphire);
            actualCost.Emerald = Math.Max(0, actualCost.Emerald - bonuses.Emerald);
            actualCost.Ruby = Math.Max(0, actualCost.Ruby - bonuses.Ruby);
            actualCost.Onyx = Math.Max(0, actualCost.Onyx - bonuses.Onyx);
            
            // Pay with resources
            foreach (var (type, amount) in actualCost.GetNonZeroCosts())
            {
                var playerHas = currentPlayer.Resources.Value.GetCost(type);
                var toPay = Math.Min(amount, playerHas);
                currentPlayer.RemoveResources(type, toPay);
                AddTokensToPool(type, toPay);
            }
            
            // Pay remaining with gold
            currentPlayer.RemoveGoldTokens(goldNeeded);
            _availableGoldTokens.Value += goldNeeded;
            
            // Purchase the card
            currentPlayer.PurchaseCard(card);
            RemoveCardFromTable(card);
            
            EndPlayerTurn();
            return true;
        }
        
        public bool ReserveCard(CardData card)
        {
            if (_currentPhase.Value != GamePhase.PlayerTurn) return false;
            
            var currentPlayer = CurrentPlayer;
            if (currentPlayer == null || currentPlayer.ReservedCards.Count >= 3) return false;
            
            currentPlayer.ReserveCard(card);
            
            // Give gold token if available
            if (_availableGoldTokens.Value > 0)
            {
                _availableGoldTokens.Value--;
            }
            
            RemoveCardFromTable(card);
            
            EndPlayerTurn();
            return true;
        }
        
        private void InitializeTokens()
        {
            // Standard Splendor token setup
            var tokens = new ResourceCost(7, 7, 7, 7, 7); // 7 of each color for 4 players
            _availableTokens.Value = tokens;
            _availableGoldTokens.Value = 5;
        }
        
        private void InitializeCardPools(CardData[] level1, CardData[] level2, CardData[] level3)
        {
            _level1Cards.Clear();
            _level2Cards.Clear();
            _level3Cards.Clear();
            
            foreach (var card in level1) _level1Cards.Add(card);
            foreach (var card in level2) _level2Cards.Add(card);
            foreach (var card in level3) _level3Cards.Add(card);
            
            // Shuffle decks (simplified)
            ShuffleCards(_level1Cards);
            ShuffleCards(_level2Cards);
            ShuffleCards(_level3Cards);
        }
        
        private void ShuffleCards(ReactiveCollection<CardData> cards)
        {
            var list = cards.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                var randomIndex = UnityEngine.Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            
            cards.Clear();
            foreach (var card in list)
            {
                cards.Add(card);
            }
        }
        
        private void RefillAvailableCards()
        {
            RefillLevel(_level1Cards, _availableLevel1Cards);
            RefillLevel(_level2Cards, _availableLevel2Cards);
            RefillLevel(_level3Cards, _availableLevel3Cards);
        }
        
        private void RefillLevel(ReactiveCollection<CardData> deck, ReactiveCollection<CardData> available)
        {
            while (available.Count < 4 && deck.Count > 0)
            {
                available.Add(deck[0]);
                deck.RemoveAt(0);
            }
        }
        
        private void RemoveCardFromTable(CardData card)
        {
            if (_availableLevel1Cards.Contains(card))
            {
                _availableLevel1Cards.Remove(card);
                RefillLevel(_level1Cards, _availableLevel1Cards);
            }
            else if (_availableLevel2Cards.Contains(card))
            {
                _availableLevel2Cards.Remove(card);
                RefillLevel(_level2Cards, _availableLevel2Cards);
            }
            else if (_availableLevel3Cards.Contains(card))
            {
                _availableLevel3Cards.Remove(card);
                RefillLevel(_level3Cards, _availableLevel3Cards);
            }
        }
        
        private void AddTokensToPool(ResourceType type, int amount)
        {
            var current = _availableTokens.Value.Clone();
            var currentAmount = current.GetCost(type);
            current.SetCost(type, currentAmount + amount);
            _availableTokens.Value = current;
        }
        
        private void RemoveTokensFromPool(ResourceType type, int amount)
        {
            var current = _availableTokens.Value.Clone();
            var currentAmount = current.GetCost(type);
            current.SetCost(type, Math.Max(0, currentAmount - amount));
            _availableTokens.Value = current;
        }
        
        private void StartPlayerTurn()
        {
            _currentPhase.Value = GamePhase.PlayerTurn;
        }
        
        private void EndPlayerTurn()
        {
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            
            if (_currentPlayerIndex.Value == 0)
            {
                _turnNumber.Value++;
            }
            
            CheckForWinner();
            
            if (_winner.Value == null)
            {
                StartPlayerTurn();
            }
            else
            {
                _currentPhase.Value = GamePhase.GameEnded;
            }
        }
        
        private void CheckForWinner()
        {
            var potentialWinner = _players.FirstOrDefault(p => p.Score.Value >= WINNING_SCORE);
            if (potentialWinner != null && _currentPlayerIndex.Value == 0) // End of round
            {
                // Find player with highest score
                var winner = _players.OrderByDescending(p => p.Score.Value).First();
                _winner.Value = winner;
            }
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
            _currentPhase?.Dispose();
            _currentPlayerIndex?.Dispose();
            _turnNumber?.Dispose();
            _winner?.Dispose();
            _players?.Dispose();
            _availableTokens?.Dispose();
            _availableGoldTokens?.Dispose();
            _level1Cards?.Dispose();
            _level2Cards?.Dispose();
            _level3Cards?.Dispose();
            _availableLevel1Cards?.Dispose();
            _availableLevel2Cards?.Dispose();
            _availableLevel3Cards?.Dispose();
        }
    }
} 