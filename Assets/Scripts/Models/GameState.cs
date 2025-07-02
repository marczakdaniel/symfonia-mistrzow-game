using System.Collections.Generic;

namespace States
{
    public interface IGameStateReader
    {
        //PlayerState GetPlayerCount(string id);
        string GetCurrentPlayerId();
        int GetPlayerScore(int playerId);
        int GetPlayerHealth(int playerId);
    }

    public interface IGameStateWriter
    {

    }

    public class GameState
    {
        
    }
}