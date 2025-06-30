using R3;
using DefaultNamespace.Models;

namespace DefaultNamespace.MusicCardDetailsPanel
{
    public enum MusicCardDetailsPanelState
    {
        Active,
        Inactive,
    }

    public class MusicCardDetailsPanelModel
    {
        public MusicCardModel MusicCardModel { get; private set; }
        private readonly ReactiveProperty<MusicCardDetailsPanelState> state = new ReactiveProperty<MusicCardDetailsPanelState>(MusicCardDetailsPanelState.Inactive);

        public ReadOnlyReactiveProperty<MusicCardDetailsPanelState> State => state;

        
        public MusicCardDetailsPanelModel()
        {
        }

        public void ActivatePanel(MusicCardModel musicCardModel)
        {
            this.MusicCardModel = musicCardModel;
            state.Value = MusicCardDetailsPanelState.Active;
        }
        
        public void DeactivatePanel()
        {
            state.Value = MusicCardDetailsPanelState.Inactive;
        }
    }
}