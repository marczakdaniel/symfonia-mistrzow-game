using UnityEngine;
using Managers;
using Models;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Data;

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
            var gameConfig = new GameConfig(musicCardDatas);
            gameManager.InitalizeGame(gameConfig);

            gameManager.StartGame().Forget();
        }
    }
}