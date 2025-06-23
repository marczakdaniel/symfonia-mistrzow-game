namespace DefaultNamespace
{
    public class GameContext
    {
        public ActionManager ActionManager { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public TokenManager TokenPanelManager { get; private set; }
        public BoardManager BoardManager { get; private set; }

        public void Initialize()
        {
            ActionManager = new ActionManager();
            TurnManager = new TurnManager();
            TokenPanelManager = new TokenManager();
            //BoardManager = new BoardManager();
        }
    }
}