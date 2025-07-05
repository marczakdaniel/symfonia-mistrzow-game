using UI.Board.BoardMusicCardPanel.BoardCardDeck;
using UI.Board.BoardMusicCardPanel.BoardMusicCard;
using UnityEngine;

namespace UI.Board.BoardMusicCardPanel
{
    public class BoardMusicCardPanelView : MonoBehaviour
    {
        [SerializeField] private BoardMusicCardView[] level1Cards;
        [SerializeField] private BoardMusicCardView[] level2Cards;
        [SerializeField] private BoardMusicCardView[] level3Cards;

        [SerializeField] private BoardCardDeckView level1CardDeck;
        [SerializeField] private BoardCardDeckView level2CardDeck;
        [SerializeField] private BoardCardDeckView level3CardDeck;

        public BoardMusicCardView[] Level1Cards => level1Cards;
        public BoardMusicCardView[] Level2Cards => level2Cards;
        public BoardMusicCardView[] Level3Cards => level3Cards;

        public BoardCardDeckView Level1CardDeck => level1CardDeck;
        public BoardCardDeckView Level2CardDeck => level2CardDeck;
        public BoardCardDeckView Level3CardDeck => level3CardDeck;
    }
}