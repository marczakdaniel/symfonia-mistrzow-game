using UnityEngine;
using Managers;
using Models;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Test
{
    public class GameTestManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MusicCardData[] musicCardDatas;
        
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
            var boardMusicCardConfig = new BoardMusicCardConfig(new string[] { }, new string[] { }, new string[] { }, musicCardDatas.Select(card => card.Id).ToList());
            var boardConfig = new BoardConfig(new BoardTokenConfig(7, 7, 7, 7, 7, 5), boardMusicCardConfig);
            var gameConfig = new GameConfig(musicCardDatas, new PlayerConfig[] { new PlayerConfig(Guid.NewGuid().ToString(), "Player 1") }, boardConfig);
            return gameConfig;
        }
    }
}