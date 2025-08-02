using UnityEngine;
using Managers;
using Models;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Data;

namespace Test
{
    /*
    public class GameTestManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MusicCardDeckData musicCardDeckData;
        [SerializeField] private ConcertCardDeckData concertCardDeckData;

        public void Start()
        {
            TestStartGame();
        }

        public void TestStartGame()
        {
            var gameConfig = CreateTestGameConfig();
            gameManager.InitalizeGame(gameConfig);

            gameManager.StartGame().Forget();
        }

        private GameConfig CreateTestGameConfig()
        {
            var boardMusicCardConfig = new BoardMusicCardConfig(new string[] { }, new string[] { }, new string[] { }, musicCardDeckData.Cards.Select(card => card.Id).ToList());
            var boardConfig = new BoardConfig(new BoardTokenConfig(7, 7, 7, 7, 7, 5), boardMusicCardConfig);
            var playerConfig = new PlayerConfig[] { new PlayerConfig(Guid.NewGuid().ToString(), "Player 1"), new PlayerConfig(Guid.NewGuid().ToString(), "Player 2") };
            var concertCardsConfig = new ConcertCardsConfig(concertCardDeckData.ConcertCards.Take(5).ToList());
            var gameConfig = new GameConfig(musicCardDeckData.Cards, playerConfig, boardConfig, concertCardsConfig);
            return gameConfig;
        }
    }
    */
}