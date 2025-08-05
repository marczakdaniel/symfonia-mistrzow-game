using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data;
using DefaultNamespace.Data;
using Managers;
using UnityEngine;

namespace Models
{   
    public class GameModel
    {
        public string GameId { get; private set; }
        public string GameName { get; private set; }
        public int PlayerCount { get; private set; }

        public List<PlayerModel> Players { get; }
        public BoardModel Board { get; private set; }

        public TurnModel Turn { get; private set; }
        public List<ConcertCardModel> ConcertCards { get; private set; } = new List<ConcertCardModel>();

        public GameModel()
        {
            //GameId = Guid.NewGuid().ToString();
            //GameName = gameName;
            Board = new BoardModel();
            Players = new List<PlayerModel>();
            Turn = new TurnModel();
        }

        private bool isInitialized = false;

        public void Initialize(GameConfig gameConfig)
        {
            InitializePlayers(gameConfig.playerConfigs);
            InitializeBoard(gameConfig.boardConfig);
            InitializeConcertCards(gameConfig.concertCards);

            isInitialized = true;
        }

        private void InitializePlayers(List<PlayerConfig> playerConfigs)
        {
            foreach (var playerConfig in playerConfigs)
            {
                var player = new PlayerModel(playerConfig);
                AddPlayer(player);
            }
        }

        private void InitializeBoard(BoardConfig boardConfig)
        {
            Board.Initialize(boardConfig);
        }

        private void InitializeConcertCards(ConcertCardData[] concertCards)
        {
            foreach (var concertCard in concertCards)
            {
                var concertCardModel = new ConcertCardModel(concertCard);
                ConcertCards.Add(concertCardModel);
            }
        }

        public void AddPlayer(PlayerModel player)
        {
            if (player == null || Players.Contains(player))
            {
                return;
            }

            Players.Add(player);

        }

        public PlayerModel GetPlayer(string playerId)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId);
        }

        public string GetNextPlayerId(string currentPlayerId)
        {
            if (currentPlayerId == null)
            {
                return Players[0].PlayerId;
            }

            var currentIndex = Players.FindIndex(p => p.PlayerId == currentPlayerId);
            return Players[(currentIndex + 1) % Players.Count].PlayerId;
        }


        // Start Game Command
        public bool CanStartGame()
        {
            return isInitialized;
        }

        public int GetBoardTokenCount(ResourceType resourceType)
        {
            return Board.GetTokenCount(resourceType);
        }
    }
}