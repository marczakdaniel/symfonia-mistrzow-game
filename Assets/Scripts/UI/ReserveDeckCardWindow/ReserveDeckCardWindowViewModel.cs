namespace UI.ReserveDeckCardWindow
{
    public class ReserveDeckCardWindowViewModel
    {
        public int CardLevel { get; set; }

        public void SetCardLevel(int cardLevel)
        {
            CardLevel = cardLevel;
        }
    }
}