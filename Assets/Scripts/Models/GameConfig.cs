using System;
using System.Collections.Generic;
using Assets.Scripts.Data;
using DefaultNamespace.Data;

namespace Models
{
    public class GameConfig
    {
        public MusicCardData[] musicCardDatas;
        public List<PlayerConfig> playerConfigs = new List<PlayerConfig>();
        public BoardConfig boardConfig;
        public ConcertCardData[] concertCards;

        private bool isMusicCardDatasInitialized = false;
        private bool isPlayerConfigsInitialized => playerConfigs.Count > 0;
        private bool isBoardConfigInitialized = false;
        private bool isConcertCardsInitialized = false;

        public bool IsInitialized => isMusicCardDatasInitialized && isPlayerConfigsInitialized && isBoardConfigInitialized && isConcertCardsInitialized;

        public GameConfig(MusicCardData[] musicCardDatas)
        {
            SetupMusicCardDatas(musicCardDatas);
        }

        public void SetupMusicCardDatas(MusicCardData[] musicCardDatas)
        {
            this.musicCardDatas = musicCardDatas;
            isMusicCardDatasInitialized = true;
        }

        public void SetupBoardConfig(BoardConfig boardConfig)
        {
            this.boardConfig = boardConfig;
            isBoardConfigInitialized = true;
        }

        public void SetupConcertCardsConfig(ConcertCardData[] concertCards)
        {
            this.concertCards = concertCards;
            isConcertCardsInitialized = true;
        }

        public void AddPlayer(string playerName)
        {
            var playerId = Guid.NewGuid().ToString();
            var playerConfig = new PlayerConfig(playerId, playerName);
            playerConfigs.Add(playerConfig);
        }

        public void ClearPlayerConfigs()
        {
            playerConfigs.Clear();
        }

        public List<PlayerConfig> GetPlayerConfigs()
        {
            return playerConfigs;
        }
    }

    public class BoardConfig
    {
        public BoardTokenConfig TokenResources;
        public BoardMusicCardConfig BoardMusicCardConfig;

        public BoardConfig(BoardTokenConfig tokenResources, BoardMusicCardConfig boardMusicCardConfig)
        {
            this.TokenResources = tokenResources;
            this.BoardMusicCardConfig = boardMusicCardConfig;
        }
    }

    public class BoardTokenConfig
    {
        public int InitialMelody;
        public int InitialHarmony;
        public int InitialRhythm;
        public int InitialInstrumentation;
        public int InitialDynamics;
        public int InitialInspiration;

        public BoardTokenConfig(int initialMelody, int initialHarmony, int initialRhythm, int initialInstrumentation, int initialDynamics, int initialInspiration)
        {
            this.InitialMelody = initialMelody;
            this.InitialHarmony = initialHarmony;
            this.InitialRhythm = initialRhythm;
            this.InitialInstrumentation = initialInstrumentation;
            this.InitialDynamics = initialDynamics;
            this.InitialInspiration = initialInspiration;
        }
    }   

    public class BoardMusicCardConfig
    {
        public string[] Level1SlotsCardIds;
        public string[] Level2SlotsCardIds;
        public string[] Level3SlotsCardIds;

        public List<string> CardInDecks;

        public BoardMusicCardConfig(string[] level1SlotsCardIds, string[] level2SlotsCardIds, string[] level3SlotsCardIds, 
                                    List<string> cardInDecks)
        {
            this.Level1SlotsCardIds = level1SlotsCardIds;
            this.Level2SlotsCardIds = level2SlotsCardIds;
            this.Level3SlotsCardIds = level3SlotsCardIds;

            this.CardInDecks = cardInDecks;
        }
    }

    public class PlayerConfig
    {
        public string PlayerId;
        public string PlayerName;

        public PlayerConfig(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }
}