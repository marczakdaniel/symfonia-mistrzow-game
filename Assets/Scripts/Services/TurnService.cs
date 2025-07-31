using DefaultNamespace.Data;
using Models;
using UnityEngine;

namespace Services
{
    public class TurnService
    {
        private readonly GameModel gameModel;
        private TurnModel turnModel => gameModel.turnModel;
        private MusicCardRepository musicCardRepository => MusicCardRepository.Instance;
        
        public TurnService(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public PlayerModel GetCurrentPlayerModel()
        {
            return gameModel.GetPlayer(turnModel.CurrentPlayerId);
        }

        public string GetCurrentPlayerId()
        {
            return turnModel.CurrentPlayerId;
        }

        // Game flow

        public void StartPlayerTurn()
        {
            turnModel.SetState(TurnState.WaitingForAction);
        }

        public void EndPlayerTurn()
        {
        }

        public void NextPlayerTurn()
        {
            var nextPlayerId = gameModel.GetNextPlayerId(turnModel.CurrentPlayerId);
            turnModel.SetCurrentPlayer(nextPlayerId);
        }

        // PLAYER ACTIONS
        // 1. Select Tokens
        // 2. Reserve Card
        // 3. Purchase Card from Board
        // 4. Purchase Card from Reserved

        // Selected Tokens Actions
        public void StartSelectingTokens()
        {
            if (turnModel.State != TurnState.WaitingForAction)
            {
                return;
            }
            turnModel.SetState(TurnState.SelectingTokens);
        }

        public bool CanAddTokenToSelectedTokens(ResourceType token)
        {
            var hasEnoughTokens = gameModel.GetBoardTokenCount(token) > turnModel.GetSelectedTokensCount(token);
            var hasEnoughTokenToSelectTwo = turnModel.GetSelectedTokensCount(token) >= 1 ? gameModel.GetBoardTokenCount(token) >= 4 : true;
            return hasEnoughTokenToSelectTwo && hasEnoughTokens && turnModel.CanAddTokenToSelectedTokens(token);
        }

        public void AddTokenToSelectedTokens(ResourceType token)
        {
            turnModel.AddTokenToSelectedTokens(token);
        }

        public void RemoveTokenFromSelectedTokens(ResourceType token)
        {
            turnModel.RemoveTokenFromSelectedTokens(token);
        }

        public void ClearSelectedTokens()
        {
            turnModel.ClearSelectedTokens();
        }

        public ResourceType?[] GetSelectedTokens()
        {
            return turnModel.GetSelectedTokens();
        }

        public int GetSelectedTokensCount(ResourceType token)
        {
            return turnModel.GetSelectedTokensCount(token);
        }

        public bool CanConfirmSelectedTokens()
        {
            return turnModel.State == TurnState.SelectingTokens;
        }

        public void ConfirmSelectedTokens()
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            var selectedTokens = turnModel.GetSelectedTokensCollection();
            gameModel.board.RemoveTokens(selectedTokens);
            currentPlayer.AddTokens(selectedTokens);
        }

        public void EndSelectingTokensWithNoConfirmation()
        {
            turnModel.SetState(TurnState.WaitingForAction);
            ClearSelectedTokens();
        }

        // Return Tokens Actions

        public bool IsTokenReturnNeeded()
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            return currentPlayer.Tokens.GetTotalResourcese() > 5;
        }

        public bool CanAddTokenToReturnTokens(ResourceType token)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            return currentPlayer.Tokens.GetCount(token) > 0 && currentPlayer.Tokens.GetTotalResourcese() - turnModel.GetAllReturnTokensCount() > 5;
        }

        public void StartReturningTokens()
        {
            
        }

        public void AddTokenToReturnTokens(ResourceType token)
        {
            turnModel.AddTokenToReturnTokens(token);
        }

        public void RemoveTokenFromReturnTokens(ResourceType token)
        {
            turnModel.RemoveTokenFromReturnTokens(token);
        }

        public void ConfirmReturnTokens()
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var returnTokens = turnModel.GetReturnTokensCollection();

            gameModel.board.AddTokens(returnTokens);
            currentPlayer.RemoveTokens(returnTokens);

            turnModel.ClearReturnTokens();
        }

        public ResourceType?[] GetReturnTokens()
        {
            return turnModel.GetReturnTokens();
        }

        public int GetReturnTokensCount(ResourceType token)
        {
            return turnModel.GetReturnTokensCount(token);
        }

        public int GetAllReturnTokensCount()
        {
            return turnModel.GetAllReturnTokensCount();
        }

        // Reserve Card Actions
        public void StartSelectingMusicCard()
        {
            if (turnModel.State != TurnState.WaitingForAction)
            {
                return;
            }
            turnModel.SetState(TurnState.SelectingMusicCard);
        }

        public bool CanConfirmReserveMusicCard()
        {
            return turnModel.State == TurnState.SelectingMusicCard;
        }

        public bool CanReserveCard()
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            return currentPlayer.ReservedCards.Count < 3;
        }

        public bool ReserveCard(string cardId)
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            if (!gameModel.board.RemoveCardFromBoard(cardId))
            {
                UnityEngine.Debug.LogError($"[TurnService] Failed to remove card from board: {cardId}");
                return false;
            }

            if (!currentPlayer.ReservedCards.AddCard(cardId))
            {
                UnityEngine.Debug.LogError($"[TurnService] Failed to add card to reserved cards: {cardId}");
                return false;
            }

            if (gameModel.board.TokenResources.GetCount(ResourceType.Inspiration) > 0)
            {
                var inspirationTokens = new ResourceCollectionModel(new ResourceType[] { ResourceType.Inspiration });
                gameModel.board.RemoveTokens(inspirationTokens);
                currentPlayer.AddTokens(inspirationTokens);
            }

            return true;
        }

        public void StopSelectingMusicCard()
        {
            turnModel.SetState(TurnState.WaitingForAction);
        }

        // Buy card actions

        public bool CanPurchaseCard(string cardId)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var card = musicCardRepository.GetCard(cardId);
            var needToAdd = card.cost.GetResourceCollectionModel().HowManychNeedToAddToHaveAll(currentPlayer.Tokens);
            var numberOfInspirationTokens = currentPlayer.Tokens.GetCount(ResourceType.Inspiration);
            return needToAdd <= numberOfInspirationTokens;
        }

        public bool CanPurchaseCardWithResources(string cardId, ResourceCollectionModel tokens)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var card = musicCardRepository.GetCard(cardId);
            var needToAdd = tokens.HowManychNeedToAddToHaveAll(card.cost.GetResourceCollectionModel());
            var numberOfInspirationTokens = tokens.GetCount(ResourceType.Inspiration);
            return needToAdd == numberOfInspirationTokens;
        }

        public ResourceCollectionModel GetCardPurchaseTokens()
        {
            return turnModel.CardPurchaseTokens;
        }

        public void PurchaseCard(string cardId, ResourceCollectionModel tokens)
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            var card = musicCardRepository.GetCard(cardId);
            currentPlayer.AddCardToPurchased(cardId);
            gameModel.board.RemoveCardFromBoard(cardId);
            currentPlayer.RemoveTokens(tokens);
            gameModel.board.AddTokens(tokens);
        }

        public bool CanAddTokenToCardPurchase(ResourceType token)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var currentTokens = turnModel.CardPurchaseTokens.GetCount(token) + 1;
            return currentTokens <= currentPlayer.Tokens.GetCount(token);
        }

        public void AddTokenToCardPurchase(ResourceType token)
        {
            turnModel.CardPurchaseTokens.AddResource(token, 1);
        }

        public bool CanRemoveTokenFromCardPurchase(ResourceType token)
        {
            return turnModel.CardPurchaseTokens.GetCount(token) > 0;
        }

        public void RemoveTokenFromCardPurchase(ResourceType token)
        {
            turnModel.CardPurchaseTokens.RemoveResource(token, 1);
        }

        public int GetCardPurchaseTokensCount(ResourceType token)
        {
            return turnModel.CardPurchaseTokens.GetCount(token);
        }

        public void ClearCardPurchaseTokens()
        {
            turnModel.CardPurchaseTokens.Clear();
        }
    }
}