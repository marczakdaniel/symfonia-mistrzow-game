using DefaultNamespace.MusicCardDetailsPanel;

namespace DefaultNamespace.Models {
    public class BoardMusicCardModel {
        public MusicCardModel MusicCardModel { get; private set; }
        public MusicCardDetailsPanelModel MusicCardDetailsPanelModel { get; private set; }

        public BoardMusicCardModel(MusicCardModel musicCardModel, MusicCardDetailsPanelModel musicCardDetailsPanelModel) {
            MusicCardModel = musicCardModel;
            MusicCardDetailsPanelModel = musicCardDetailsPanelModel;
        }
    }
}