public class TurnManager
{
    public bool IsPlayerTurn;
    
    public TurnManager()
    {
        
    }

    public void StartTurn()
    {
        IsPlayerTurn = true;
    }

    public void EndTurn()
    {
        IsPlayerTurn = false;
    }
}