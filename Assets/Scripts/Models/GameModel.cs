namespace Models
{
    public interface IGameModelReader
    {

    }

    public interface IGameModelWriter
    {

    }
    
    public class GameModel
    {
        public string GameId { get; private set; }
        public string GameName { get; private set; }
        public int PlayerCount { get; private set; }
        
    }
}