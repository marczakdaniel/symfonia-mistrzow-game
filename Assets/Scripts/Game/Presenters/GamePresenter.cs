using R3;
using SplendorGame.Core.MVP;
using SplendorGame.Game.Models;
using SplendorGame.Game.Views;
using SplendorGame.Game.Data;
using System.Collections.Generic;
using System.Linq;

namespace SplendorGame.Game.Presenters
{
    /// <summary>
    /// Main presenter managing the game flow and connecting model to view
    /// </summary>
    public class GamePresenter : BasePresenter<GameStateModel, GameBoardView>
    {
        private readonly List<PlayerPresenter> _playerPresenters = new();
        private readonly CardData[] _level1Cards;
        private readonly CardData[] _level2Cards;
        private readonly CardData[] _level3Cards;
        
        public GamePresenter(GameStateModel model, GameBoardView view, 
            CardData[] level1Cards, CardData[] level2Cards, CardData[] level3Cards)
            : base(model, view)
        {
            _level1Cards = level1Cards;
            _level2Cards = level2Cards;
            _level3Cards = level3Cards;
        }
        
        protected override void BindViewToModel()
        {
            // Bind game state changes to view
            Model.CurrentPhase
                .Subscribe(phase => View.SetGamePhase(phase.ToString()))
                .AddTo(_disposables);
            
            Model.TurnNumber
                .Subscribe(turnNumber => View.SetTurnNumber(turnNumber))
                .AddTo(_disposables);
            
            Model.CurrentPlayerIndex
                .Subscribe(_ => UpdateCurrentPlayerDisplay())
                .AddTo(_disposables);
            
            // Bind available cards to view
            Model.AvailableLevel1Cards.ObserveCountChanged()
                .Subscribe(_ => UpdateAvailableCards())
                .AddTo(_disposables);
            
            Model.AvailableLevel2Cards.ObserveCountChanged()
                .Subscribe(_ => UpdateAvailableCards())
                .AddTo(_disposables);
            
            Model.AvailableLevel3Cards.ObserveCountChanged()
                .Subscribe(_ => UpdateAvailableCards())
                .AddTo(_disposables);
            
            // Bind token pool to view
            Model.AvailableTokens
                .Subscribe(tokens => UpdateTokenPool(tokens))
                .AddTo(_disposables);
            
            Model.AvailableGoldTokens
                .Subscribe(_ => UpdateTokenPool(Model.AvailableTokens.Value))
                .AddTo(_disposables);
            
            // Bind view events to model actions
            View.OnCardPurchase
                .Subscribe(card => TryPurchaseCard(card))
                .AddTo(_disposables);
            
            View.OnCardReserve
                .Subscribe(card => TryReserveCard(card))
                .AddTo(_disposables);
            
            View.OnTokensTaken
                .Subscribe(tokens => TryTakeTokens(tokens))
                .AddTo(_disposables);
            
            // Update interaction state based on game phase
            Model.CurrentPhase
                .Subscribe(phase => UpdateInteractionState(phase))
                .AddTo(_disposables);
        }
        
        public void AddPlayer(string playerName)
        {
            Model.AddPlayer(playerName);
            CreatePlayerPresenter(Model.Players.Last());
        }
        
        public void StartGame()
        {
            Model.StartGame(_level1Cards, _level2Cards, _level3Cards);
        }
        
        private void CreatePlayerPresenter(PlayerModel playerModel)
        {
            // Create player view
            var playerViewObj = UnityEngine.Object.Instantiate(View.playerViewPrefab, View.playersContainer);
            var playerView = playerViewObj.GetComponent<PlayerView>();
            
            if (playerView != null)
            {
                // Create presenter for this player
                var playerPresenter = new PlayerPresenter(playerModel, playerView);
                playerPresenter.Initialize();
                _playerPresenters.Add(playerPresenter);
            }
        }
        
        private void UpdateCurrentPlayerDisplay()
        {
            var currentPlayer = Model.CurrentPlayer;
            if (currentPlayer != null)
            {
                View.SetCurrentPlayer(currentPlayer.PlayerName.Value);
                
                // Update player indicators
                for (int i = 0; i < _playerPresenters.Count; i++)
                {
                    var isCurrentPlayer = i == Model.CurrentPlayerIndex.Value;
                    _playerPresenters[i].SetCurrentPlayer(isCurrentPlayer);
                }
            }
        }
        
        private void UpdateAvailableCards()
        {
            View.UpdateAvailableCards(
                Model.AvailableLevel1Cards.ToList(),
                Model.AvailableLevel2Cards.ToList(),
                Model.AvailableLevel3Cards.ToList()
            );
            
            // Update card affordability for current player
            UpdateCardAffordability();
        }
        
        private void UpdateTokenPool(ResourceCost tokens)
        {
            View.UpdateTokenPool(tokens, Model.AvailableGoldTokens.Value);
        }
        
        private void UpdateInteractionState(GamePhase phase)
        {
            bool canInteract = phase == GamePhase.PlayerTurn;
            View.SetInteractable(canInteract);
            
            if (canInteract)
            {
                UpdateCardAffordability();
            }
        }
        
        private void UpdateCardAffordability()
        {
            var currentPlayer = Model.CurrentPlayer;
            if (currentPlayer == null) return;
            
            var allCards = new List<CardData>();
            allCards.AddRange(Model.AvailableLevel1Cards);
            allCards.AddRange(Model.AvailableLevel2Cards);
            allCards.AddRange(Model.AvailableLevel3Cards);
            
            foreach (var card in allCards)
            {
                bool canAfford = currentPlayer.CanAffordCard(card);
                bool canReserve = currentPlayer.ReservedCards.Count < 3;
                View.UpdateCardAffordability(card, canAfford, canReserve);
            }
        }
        
        private void TryPurchaseCard(CardData card)
        {
            if (Model.CurrentPhase.Value != GamePhase.PlayerTurn) return;
            
            bool success = Model.BuyCard(card);
            if (!success)
            {
                // Could show error message to user
                UnityEngine.Debug.Log($"Cannot purchase card {card.id}");
            }
        }
        
        private void TryReserveCard(CardData card)
        {
            if (Model.CurrentPhase.Value != GamePhase.PlayerTurn) return;
            
            bool success = Model.ReserveCard(card);
            if (!success)
            {
                // Could show error message to user
                UnityEngine.Debug.Log($"Cannot reserve card {card.id}");
            }
        }
        
        private void TryTakeTokens(ResourceType[] tokens)
        {
            if (Model.CurrentPhase.Value != GamePhase.PlayerTurn) return;
            
            bool success = false;
            
            if (tokens.Length == 1)
            {
                // Taking 2 of the same type
                success = Model.TakeTokens(tokens[0]);
            }
            else if (tokens.Length == 3)
            {
                // Taking 3 different types
                success = Model.TakeTokens(tokens[0], tokens[1], tokens[2]);
            }
            
            if (!success)
            {
                // Could show error message to user
                UnityEngine.Debug.Log("Cannot take those tokens");
            }
        }
        
        public override void Dispose()
        {
            foreach (var playerPresenter in _playerPresenters)
            {
                playerPresenter?.Dispose();
            }
            _playerPresenters.Clear();
            
            base.Dispose();
        }
    }
} 