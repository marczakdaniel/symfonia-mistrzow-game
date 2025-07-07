using DefaultNamespace.Data;
using R3;
using UnityEngine;

namespace UI.MusicCardDetailsPanel {
    public enum MusicCardDetailsPanelState {
            Closed,
            DuringOpenAnimation,
            Opened,
            DuringCloseAnimation,
            DuringBuyAnimation,
            DuringReserveAnimation,
    }

    public class MusicCardDetailsPanelViewModel {
        public ReactiveProperty<MusicCardDetailsPanelState> State { get; private set; } = new ReactiveProperty<MusicCardDetailsPanelState>(MusicCardDetailsPanelState.Closed);
        public ReactiveProperty<MusicCardData> MusicCardData { get; private set; } = new ReactiveProperty<MusicCardData>(null);
        public string MusicCardId { get; private set; }
        public string PlayerId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public MusicCardDetailsPanelViewModel() {
            
        }

        public void SetPlayerId(string playerId) {
            PlayerId = playerId;
        }

        public bool OpenCardDetailsPanel(string musicCardId, MusicCardData musicCardData, int level, int position) { 
            if (musicCardData == null) {
                Debug.LogError("[DetailsPanel] Cannot open panel with null music card data");
                return false;
            }

            if (!CanOpen()) {
                Debug.LogError($"[DetailsPanel] Cannot open panel in state: {State.Value}");
                return false;
            }

            Level = level;
            Position = position;

            SetMusicCardData(musicCardId, musicCardData);
            SetState(MusicCardDetailsPanelState.DuringOpenAnimation);
            return true;
        }

        public bool CloseCardDetailsPanel() {
            if (!CanClose()) {
                Debug.LogError($"[DetailsPanel] Cannot close panel in state: {State.Value}");
                return false;
            }

            SetState(MusicCardDetailsPanelState.DuringCloseAnimation);
            return true;
        }

        public bool StartBuyAnimation() {
            if (!CanPerformAction()) {
                Debug.LogError($"[DetailsPanel] Cannot start buy animation in state: {State.Value}");
                return false;
            }

            SetState(MusicCardDetailsPanelState.DuringBuyAnimation);
            return true;
        }

        public bool StartReserveAnimation() {
            if (!CanPerformAction()) {
                Debug.LogError($"[DetailsPanel] Cannot start reserve animation in state: {State.Value}");
                return false;
            }

            SetState(MusicCardDetailsPanelState.DuringReserveAnimation);
            return true;
        }

        public void CompleteOpenAnimation() {
            if (State.Value != MusicCardDetailsPanelState.DuringOpenAnimation) {
                Debug.LogError($"[DetailsPanel] Cannot complete open animation in state: {State.Value}");
                return;
            }
            SetState(MusicCardDetailsPanelState.Opened);
        }

        public void CompleteCloseAnimation() {
            if (State.Value != MusicCardDetailsPanelState.DuringCloseAnimation) {
                Debug.LogError($"[DetailsPanel] Cannot complete close animation in state: {State.Value}");
                return;
            }
            SetState(MusicCardDetailsPanelState.Closed);
            ClearMusicCardData();
        }   

        public void CompleteBuyAnimation() {
            if (State.Value != MusicCardDetailsPanelState.DuringBuyAnimation) {
                Debug.LogError($"[DetailsPanel] Cannot complete buy animation in state: {State.Value}");
                return;
            }
            SetState(MusicCardDetailsPanelState.Closed);
            ClearMusicCardData();
        }

        public void CompleteReserveAnimation() {
            if (State.Value != MusicCardDetailsPanelState.DuringReserveAnimation) {
                Debug.LogError($"[DetailsPanel] Cannot complete reserve animation in state: {State.Value}");
                return;
            }
            SetState(MusicCardDetailsPanelState.Closed);
            ClearMusicCardData();
        }

        private bool CanOpen() {
            return State.Value == MusicCardDetailsPanelState.Closed;
        }

        private bool CanClose() {
            return State.Value == MusicCardDetailsPanelState.Opened;
        }

        private bool CanPerformAction() {
            return State.Value == MusicCardDetailsPanelState.Opened;
        }

        private void SetMusicCardData(string musicCardId, MusicCardData musicCardData) {
            MusicCardData.Value = musicCardData;
            MusicCardId = musicCardId;
        }

        public void ClearMusicCardData() {
            SetMusicCardData("", null);
        }

        private void SetState(MusicCardDetailsPanelState state) {
            State.Value = state;
        }
    }
}