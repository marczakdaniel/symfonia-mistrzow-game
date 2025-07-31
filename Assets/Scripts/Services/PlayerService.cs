using System.Collections.Generic;
using System.Linq;
using Models;

namespace Services
{
    public class PlayerService
    {
        private readonly GameModel gameModel;
        private List<PlayerModel> players => gameModel.Players;

        public PlayerService(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public PlayerModel GetPlayer(string playerId)
        {
            return players.FirstOrDefault(p => p.PlayerId == playerId);
        }
    }
}