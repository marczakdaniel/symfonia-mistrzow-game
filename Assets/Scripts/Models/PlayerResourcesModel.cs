using DefaultNamespace.Data;
using ObservableCollections;
using R3;

namespace DefaultNamespace.Models
{
    public class PlayerResourcesModel
    {
        private ReactiveProperty<int> melody = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> harmony = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> rhythm = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> instrumentation = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> dynamics = new ReactiveProperty<int>(0);
        private ReactiveProperty<int> inspiration = new ReactiveProperty<int>(0);

        private ObservableList<MusicCardModel> reservedCards = new ObservableList<MusicCardModel>();
        private ObservableList<MusicCardModel> purchasedCards = new ObservableList<MusicCardModel>();

        public bool CanAfford(ResourceCost cost)
        {
            return true;
        }

        public PlayerResourcesModel()
        {
            
        }
        
    }
}