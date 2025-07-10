using UnityEngine;
using Managers;
using Models;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;
using System;

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
            var gameConfig = new GameConfig(musicCardDatas, new PlayerConfig[] { new PlayerConfig(Guid.NewGuid().ToString(), "Player 1") });
            gameManager.InitalizeGame(gameConfig);

            gameManager.StartGame().Forget();
        }
    }
}