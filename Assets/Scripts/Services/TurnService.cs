using System;
using System.Collections.Generic;
using DefaultNamespace.Data;
using Models;
using UnityEngine;

namespace Services
{
    public class TurnService
    {
        private readonly GameModel gameModel;
        private TurnModel turnModel => gameModel.Turn;
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

        public int GetCurrentPlayerIndex()
        {
            return gameModel.Players.IndexOf(GetCurrentPlayerModel());
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
            gameModel.Board.RemoveTokens(selectedTokens);
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
            return currentPlayer.Tokens.GetTotalResourcese() > 10;
        }

        public bool CanAddTokenToReturnTokens(ResourceType token)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            return currentPlayer.Tokens.GetCount(token) > 0 && currentPlayer.Tokens.GetTotalResourcese() - turnModel.GetAllReturnTokensCount() > 10;
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
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var returnTokens = turnModel.GetReturnTokensCollection();

            gameModel.Board.AddTokens(returnTokens);
            currentPlayer.RemoveTokens(returnTokens);

            turnModel.ClearReturnTokens();
        }

        public void ClearReturnTokens()
        {
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

            if (!gameModel.Board.RemoveCardFromBoard(cardId))
            {
                UnityEngine.Debug.LogError($"[TurnService] Failed to remove card from board: {cardId}");
                return false;
            }

            if (!currentPlayer.ReservedCards.AddCard(cardId))
            {
                UnityEngine.Debug.LogError($"[TurnService] Failed to add card to reserved cards: {cardId}");
                return false;
            }

            if (gameModel.Board.TokenResources.GetCount(ResourceType.Inspiration) > 0)
            {
                var inspirationTokens = new ResourceCollectionModel(new ResourceType[] { ResourceType.Inspiration });
                gameModel.Board.RemoveTokens(inspirationTokens);
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
            var tokensFromCard = currentPlayer.PurchasedCards.GetAllResourceCollection();
            var allPlayerTokens = currentPlayer.Tokens.CombineCollections(tokensFromCard);
            var needToAdd = allPlayerTokens.HowManychNeedToAddToHaveAll(card.cost.GetResourceCollectionModel());
            var numberOfInspirationTokens = tokens.GetCount(ResourceType.Inspiration);
            return needToAdd == numberOfInspirationTokens;
        }

        public ResourceCollectionModel GetInitialSelectedTokens(string cardId)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var card = musicCardRepository.GetCard(cardId);
            var tokensFromCard = currentPlayer.PurchasedCards.GetAllResourceCollection();
            var allPlayerTokens = currentPlayer.Tokens.CombineCollections(tokensFromCard);
            
            var neededTokens = tokensFromCard.NeedToAddToHaveAll(card.cost.GetResourceCollectionModel());
            var numberOfInspirationTokensNeeded = allPlayerTokens.HowManychNeedToAddToHaveAll(card.cost.GetResourceCollectionModel());

            var initialInspirationTokens = Math.Min(currentPlayer.Tokens.GetCount(ResourceType.Inspiration), numberOfInspirationTokensNeeded);

            var initialTokens = currentPlayer.Tokens.GetMinTokens(neededTokens);

            initialTokens.AddResource(ResourceType.Inspiration, initialInspirationTokens);

            return initialTokens;
        }

        public ResourceCollectionModel GetTokensNeededToPurchase(string cardId)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var card = musicCardRepository.GetCard(cardId);
            var tokensFromCard = currentPlayer.PurchasedCards.GetAllResourceCollection();
            var allPlayerTokens = currentPlayer.Tokens.CombineCollections(tokensFromCard);
            var neededTokens = tokensFromCard.NeedToAddToHaveAll(card.cost.GetResourceCollectionModel());
            return neededTokens;
        }
        public ResourceCollectionModel GetCardPurchaseTokens()
        {
            return turnModel.CardPurchaseTokens;
        }

        public void PurchaseCardFromBoard(string cardId, ResourceCollectionModel tokens)
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            var card = musicCardRepository.GetCard(cardId);

            currentPlayer.AddCardToPurchased(cardId);
            gameModel.Board.RemoveCardFromBoard(cardId);

            currentPlayer.RemoveTokens(tokens);
            gameModel.Board.AddTokens(tokens);
        }

        public void PurchaseCardFromReserve(string cardId, ResourceCollectionModel tokens)
        {
            turnModel.SetState(TurnState.ReadyToEndTurn);
            
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            currentPlayer.AddCardToPurchased(cardId);
            currentPlayer.RemoveCardFromReserved(cardId);
            
            currentPlayer.RemoveTokens(tokens);
            gameModel.Board.AddTokens(tokens);
        }



        public void InitializeCardPurchaseTokens(ResourceCollectionModel initialTokens)
        {
            turnModel.CardPurchaseTokens.Add(initialTokens);
        }

        public bool CanAddTokenToCardPurchase(string cardId, ResourceType token)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var tokensNeededToPurchase = GetTokensNeededToPurchase(cardId);
            var currentTokens = turnModel.CardPurchaseTokens.GetCount(token) + 1;
            var hasEnoughTokens = currentTokens <= currentPlayer.Tokens.GetCount(token);
            var isItInRange = currentTokens <= tokensNeededToPurchase.GetCount(token) || token == ResourceType.Inspiration;
            return hasEnoughTokens && isItInRange;
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

        // Concert Cards Actions
        public bool CanClaimConcertCard(out string cardId)
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var concertCards = gameModel.ConcertCards;
            var playerMusicCards = currentPlayer.PurchasedCards.GetAllResourceCollection();

            foreach (var card in concertCards)
            {
                if (card.State == ConcertCardState.Available && card.CanClaim(playerMusicCards))
                {
                    cardId = card.ConcertCardData.Id;
                    return true;
                }
            }

            cardId = null;
            return false;
        }

        public void ClaimConcertCard(string cardId)
        {
            if (!CanClaimConcertCard(out string id) || id != cardId)
            {
                Debug.LogError($"[TurnService] Cannot claim concert card: {cardId}");
                return;
            }

            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            var concertCard = gameModel.ConcertCards.Find(card => card.ConcertCardData.Id == cardId);

            currentPlayer.AddConcertCard(concertCard.ConcertCardData);
            concertCard.SetState(ConcertCardState.Claimed);
        }

        public List<ConcertCardModel> GetConcertCards()
        {
            return gameModel.ConcertCards;
        }

    }
}