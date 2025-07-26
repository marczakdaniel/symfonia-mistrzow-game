using DefaultNamespace.Data;
using Models;

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

        // Game flow

        public void StartPlayerTurn()
        {
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

        public void ConfirmSelectedTokens()
        {
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);

            var selectedTokens = turnModel.GetSelectedTokensCollection();
            gameModel.board.RemoveTokens(selectedTokens);
            currentPlayer.AddTokens(selectedTokens);
        }
    }
}