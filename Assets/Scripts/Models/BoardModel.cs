using DefaultNamespace.Data;
using ObservableCollections;

namespace DefaultNamespace.Models
{
    public class BoardModel
    {
        private readonly ObservableList<MusicCardData> _level1Cards = new ObservableList<MusicCardData>(4);
        private readonly ObservableList<MusicCardData> _level2Cards = new ObservableList<MusicCardData>(4);
        private readonly ObservableList<MusicCardData> _level3Cards = new ObservableList<MusicCardData>(4);

        public ObservableList<MusicCardData> Level1Cards => _level1Cards;
        public ObservableList<MusicCardData> Level2Cards => _level2Cards;
        public ObservableList<MusicCardData> Level3Cards => _level3Cards;

        public void AddCard(MusicCardData card)
        {
            _level1Cards.Add(card);
            Level1Cards.Insert(0, card);
        }
    }
}