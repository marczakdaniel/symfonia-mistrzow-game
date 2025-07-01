using System.Collections.Generic;

namespace DefaultNamespace.State
{
    public class GameState
    {
        public List<PlayerState> Players = new List<PlayerState>(4);
        public string CurrentPlayerId { get; set;}

    }
}