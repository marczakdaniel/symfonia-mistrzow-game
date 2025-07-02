using DefaultNamespace.Data;
using DefaultNamespace.MusicCardDetailsPanel;
using R3;
using UnityEngine;
namespace UI.BoardMusicCard {

    public enum BoardMusicCardState {
        Disabled,
        DuringPutOnBoardAnimation,
        Hidden,
        DuringRevealAnimation,
        Visible,
        DuringMovingToPlayerResources,
    }

    public class BoardMusicCardModel {
        public ReactiveProperty<BoardMusicCardState> State { get; private set; } = new ReactiveProperty<BoardMusicCardState>(BoardMusicCardState.Disabled);
        public ReactiveProperty<MusicCardData> MusicCardData { get; private set; } = new ReactiveProperty<MusicCardData>();

        public BoardMusicCardModel() {
        }

        public bool PutCardOnBoard(MusicCardData musicCardData) {
            if (musicCardData == null) {
                Debug.LogError($"[BoardMusicCard] Cannot put card on board with null music card data");
                return false;
            }

            if (!CanPutCardOnBoard()) {
                Debug.LogError($"[BoardMusicCard] Cannot put card on board in state: {State.Value}");
                return false;
            }

            SetMusicCardData(musicCardData);
            SetState(BoardMusicCardState.DuringPutOnBoardAnimation);
            return true;
        }

        public bool RevealCard() {
            if (!CanRevealCard()) {
                Debug.LogError($"[BoardMusicCard] Cannot reveal card in state: {State.Value}");
                return false;
            }

            SetState(BoardMusicCardState.DuringRevealAnimation);
            return true;
        }

        public bool MoveCardToPlayerResources() {
            if (!CanMoveCardToPlayerResources()) {
                Debug.LogError($"[BoardMusicCard] Cannot move card to player resources in state: {State.Value}");
                return false;
            }

            SetState(BoardMusicCardState.DuringMovingToPlayerResources);
            return true;
        }

        public void CompletePutOnBoardAnimation() {
            if (State.Value != BoardMusicCardState.DuringPutOnBoardAnimation) {
                Debug.LogError($"[BoardMusicCard] Cannot complete put on board animation in state: {State.Value}");
                return;
            }
            SetState(BoardMusicCardState.Hidden);
        }

        public void CompleteRevealAnimation() {
            if (State.Value != BoardMusicCardState.DuringRevealAnimation) {
                Debug.LogError($"[BoardMusicCard] Cannot complete reveal animation in state: {State.Value}");
                return;
            }
            SetState(BoardMusicCardState.Visible);
        }

        public void CompleteMovingToPlayerResources() {
            if (State.Value != BoardMusicCardState.DuringMovingToPlayerResources) {
                Debug.LogError($"[BoardMusicCard] Cannot complete moving to player resources in state: {State.Value}");
                return;
            }
            SetState(BoardMusicCardState.Disabled);
            SetMusicCardData(null);
        }

        private bool CanPutCardOnBoard() {
            return State.Value == BoardMusicCardState.Disabled;
        }

        private bool CanRevealCard() {
            return State.Value == BoardMusicCardState.Hidden;
        }

        private bool CanMoveCardToPlayerResources() {
            return State.Value == BoardMusicCardState.Visible;
        }

        private void SetMusicCardData(MusicCardData musicCardData) {
            MusicCardData.Value = musicCardData;
        }

        private void SetState(BoardMusicCardState state) {
            State.Value = state;
        }
    }
}