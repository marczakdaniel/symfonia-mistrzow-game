namespace Models
{
    public class PlayerModel
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public int Points { get; private set; }
        public int Health { get; private set; }
        public ResourceCollectionModel Resources { get; private set; }
        

        public PlayerModel(string playerId, string playerName, int score, int health)
        {
            PlayerId = playerId;
        }
    }
}