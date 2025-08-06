using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace Services
{
    public class ConfigService
    {
        private readonly GameConfig gameConfig;
        private readonly List<Sprite> playerAvatarsData;
        public ConfigService(GameConfig gameConfig, List<Sprite> playerAvatarsData)
        {
            this.gameConfig = gameConfig;
            this.playerAvatarsData = playerAvatarsData;
        }

        public void AddPlayer(string playerName, Sprite playerAvatar)
        {
            gameConfig.AddPlayer(playerName, playerAvatar);
        }

        public void ClearPlayers()
        {
            gameConfig.ClearPlayerConfigs();
        }

        public List<string> GetPlayerNames()
        {
            return gameConfig.GetPlayerConfigs().Select(playerConfig => playerConfig.PlayerName).ToList();
        }

        public List<Sprite> GetPlayerAvatars()
        {
            return gameConfig.GetPlayerConfigs().Select(playerConfig => playerConfig.PlayerAvatar).ToList();
        }

        public List<Sprite> GetAvailablePlayerAvatars()
        {
            return playerAvatarsData;
        }

        public bool CanAddPlayer()
        {
            return gameConfig.GetPlayerConfigs().Count < 4;
        }

        public GameConfig GetGameConfig()
        {
            return gameConfig;
        }

        public bool CanStartGame()
        {
            return gameConfig.GetPlayerConfigs().Count > 1 && gameConfig.GetPlayerConfigs().Count < 5;
        }

        public void SetupBoardConfig(BoardConfig boardConfig)
        {
            var boardTokenConfig = new BoardTokenConfig(boardConfig.TokenResources.InitialMelody, boardConfig.TokenResources.InitialHarmony, boardConfig.TokenResources.InitialRhythm, boardConfig.TokenResources.InitialInstrumentation, boardConfig.TokenResources.InitialDynamics, boardConfig.TokenResources.InitialInspiration);
        }

        public BoardConfig GetBoardConfig()
        {
            var boardTokenConfig = GetBoardTokenConfig();
            var boardMusicCardConfig = GetBoardMusicCardConfig();
            return new BoardConfig(boardTokenConfig, boardMusicCardConfig);
        }

        public BoardTokenConfig GetBoardTokenConfig()
        {
            var numberOfPlayers = gameConfig.GetPlayerConfigs().Count;
            if (numberOfPlayers == 2)
            {
                return new BoardTokenConfig(4, 4, 4, 4, 4, 5);
            }
            else if (numberOfPlayers == 3)
            {
                return new BoardTokenConfig(5, 5, 5, 5, 5, 5);
            }
            else if (numberOfPlayers == 4)
            {
                return new BoardTokenConfig(7, 7, 7, 7, 7, 5);
            }
            else
            {
                throw new Exception("Invalid number of players");
            }
        }

        public BoardMusicCardConfig GetBoardMusicCardConfig()
        {
            var level1SlotsCardIds = new string[] { };
            var level2SlotsCardIds = new string[] { };
            var level3SlotsCardIds = new string[] { };
            var cardInDecks = MusicCardRepository.Instance.GetAllCards().Select(card => card.id).ToList();
            return new BoardMusicCardConfig(level1SlotsCardIds, level2SlotsCardIds, level3SlotsCardIds, cardInDecks);
        }
    }
}