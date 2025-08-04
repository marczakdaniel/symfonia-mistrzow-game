namespace UI.Board.BoardMusicCardPanel.BoardCardDeck
{
    public class BoardCardDeckViewModel
    {
        public BoardCardDeckViewModel(int level)
        {
            Level = level;
        }

        public int Level { get; private set; }
    }
}