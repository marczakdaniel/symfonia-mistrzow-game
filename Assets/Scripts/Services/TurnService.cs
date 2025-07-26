using DefaultNamespace.Data;
using Models;
using UnityEngine;

namespace Services
{
    public class TurnService
    {
        private readonly GameModel gameModel;
        private TurnModel turnModel => gameModel.turnModel;
        
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
            return hasEnoughTokens && turnModel.CanAddTokenToSelectedTokens(token);
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
    }
}