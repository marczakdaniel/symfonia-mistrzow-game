using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Services
{
    public class PlayerService
    {
        private readonly GameModel gameModel;
        private List<PlayerModel> players => gameModel.Players;
        private MusicCardRepository musicCardRepository => MusicCardRepository.Instance;
        public PlayerService(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        public PlayerModel GetPlayer(string playerId)
        {
            return players.FirstOrDefault(p => p.PlayerId == playerId);
        }

        public PlayerModel[] GetPlayers()
        {
            return players.ToArray();
        }

        public Sprite[] GetPlayerAvatars()
        {
            return players.Select(p => p.PlayerAvatar).ToArray();
        }

        public void UpdatePoints(string playerId)
        {
            var player = GetPlayer(playerId);
            player.CalculatePoints();
        }

        public ResourceCollectionModel GetPlayerResourcesFromCardAndTokens(string playerId)
        {
            var player = GetPlayer(playerId);
            var tokensFromCard = player.GetPurchasedAllResourceCollection();
            var allPlayerTokens = player.Tokens.CombineCollections(tokensFromCard);
            return allPlayerTokens;
        }
    }
}