namespace UI.PlayerResourcesWindow
{
    public class PlayerResourcesWindowViewModel
    {
        public bool IsCurrentPlayer { get; private set; }

        public void SetIsCurrentPlayer(bool isCurrentPlayer)
        {
            IsCurrentPlayer = isCurrentPlayer;
        }
    }
}