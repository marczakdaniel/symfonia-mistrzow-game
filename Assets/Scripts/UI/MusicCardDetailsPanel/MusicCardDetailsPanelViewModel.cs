using DefaultNamespace.Data;
using R3;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {

    public class MusicCardDetailsPanelViewModel {
        public MusicCardData MusicCardData { get; private set; }

        public MusicCardDetailsPanelViewModel() {
        }

        public void SetMusicCardData(MusicCardData musicCardData) {
            MusicCardData = musicCardData;
        }

        public void ClearMusicCardData() {
            MusicCardData = null;
        }
    }
}