using System.Collections.Generic;
using System.Linq;

namespace Models
{   
    public class GameModel
    {
        private static GameModel _instance;
        public static GameModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameModel("default", "default");
                }
                return _instance;
            }
        }

        public string GameId { get; private set; }
        public string GameName { get; private set; }
        public int PlayerCount { get; private set; }


        
        private List<PlayerModel> players { get; }
        public string CurrentPlayerId { get; private set; }
        public BoardModel Board { get; private set; }

        private GameModel(string gameId, string gameName)
        {
            GameId = gameId;
            GameName = gameName;
            Board = new BoardModel();
            players = new List<PlayerModel>();
        }

        public static void Initialize(string gameId, string gameName)
        {
            _instance = new GameModel("default", "default");
        }

        public void AddPlayer(PlayerModel player)
        {
            if (player == null || players.Contains(player))
            {
                return;
            }

            players.Add(player);

        }

        public PlayerModel GetPlayer(string playerId)
        {
            return players.FirstOrDefault(p => p.PlayerId == playerId);
        }
        
    }
}