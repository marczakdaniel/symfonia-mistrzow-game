using ObservableCollections;

namespace DefaultNamespace.Models {
    public class PlayerCardsModel {
        private ObservableList<MusicCardModel> reservedCards = new ObservableList<MusicCardModel>();
        private ObservableList<MusicCardModel> purchasedCards = new ObservableList<MusicCardModel>();
    }
}