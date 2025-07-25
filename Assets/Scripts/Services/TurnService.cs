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

        }
        
        public void ConfirmSelectedTokens()
        {
            turnModel.SetState(TurnState.ConfirmingAction);
            var currentPlayer = gameModel.GetPlayer(turnModel.CurrentPlayerId);
            //currentPlayer.AddTokens(turnModel.SelectedTokens);
        }
    }
}