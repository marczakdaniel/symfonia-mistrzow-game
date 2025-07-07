using DefaultNamespace.Data;
using R3;
using UnityEngine;
namespace UI.Board.BoardMusicCardPanel.BoardMusicCard 
{
    public enum BoardMusicCardState {
        Disabled,
        DuringPutOnBoardAnimation,
        Hidden,
        DuringRevealAnimation,
        Visible,
        DuringOpenMusicCardDetailsPanel,
        DuringMovingToPlayerResources,
    }

    public class BoardMusicCardViewModel {
        public ReactiveProperty<BoardMusicCardState> State { get; private set; } = new ReactiveProperty<BoardMusicCardState>(BoardMusicCardState.Disabled);
        public ReactiveProperty<IMusicCardDataReader> MusicCardData { get; private set; } = new ReactiveProperty<IMusicCardDataReader>();
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public BoardMusicCardViewModel(int level, int position) {
            Level = level;
            Position = position;
        }

        public bool PutCardOnBoard(string musicCardId, IMusicCardDataReader musicCardData) {
            if (musicCardData == null) {
                Debug.LogError($"[BoardMusicCard] Cannot put card on board with null music card data");
                return false;
            }

            if (!CanPutCardOnBoard()) {
                Debug.LogError($"[BoardMusicCard] Cannot put card on board in state: {State.Value}");
                return false;
            }

            SetMusicCardData(musicCardId, musicCardData);
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

        public bool DisableCardWhenOpenMusicCardDetailsPanel() {
            if (!CanDisableCardWhenOpenMusicCardDetailsPanel()) {
                Debug.LogError($"[BoardMusicCard] Cannot disable card when open music card details panel in state: {State.Value}");
                return false;
            }
            SetState(BoardMusicCardState.DuringOpenMusicCardDetailsPanel);
            return true;
        }

        public bool CanDisableCardWhenOpenMusicCardDetailsPanel() {
            return State.Value == BoardMusicCardState.Visible;
        }

        public bool AfterCloseMusicCardDetailsPanel() {
            if (!CanAfterCloseMusicCardDetailsPanel()) {
                Debug.LogError($"[BoardMusicCard] Cannot after close music card details panel in state: {State.Value}");
                return false;
            }
            SetState(BoardMusicCardState.Visible);
            return true;
        }

        public bool CanAfterCloseMusicCardDetailsPanel() {
            return State.Value == BoardMusicCardState.DuringOpenMusicCardDetailsPanel;
        }

        public bool AfterBuyMusicCard() {
            if (!CanAfterBuyMusicCard()) {
                Debug.LogError($"[BoardMusicCard] Cannot after buy music card in state: {State.Value}");
                return false;
            }
            SetState(BoardMusicCardState.Disabled);
            return true;
        }   

        public bool CanAfterBuyMusicCard() {
            return State.Value == BoardMusicCardState.DuringOpenMusicCardDetailsPanel;
        }

        public bool AfterReserveMusicCard() {
            if (!CanAfterReserveMusicCard()) {
                Debug.LogError($"[BoardMusicCard] Cannot after reserve music card in state: {State.Value}");
                return false;
            }
            SetState(BoardMusicCardState.Disabled);
            return true;
        }       

        public bool CanAfterReserveMusicCard() {
            return State.Value == BoardMusicCardState.DuringOpenMusicCardDetailsPanel;
        }

        public void CompleteOpenMusicCardDetailsPanel() {
            if (State.Value != BoardMusicCardState.DuringOpenMusicCardDetailsPanel) {
                Debug.LogError($"[BoardMusicCard] Cannot complete open music card details panel in state: {State.Value}");
                return;
            }
            SetState(BoardMusicCardState.Visible);
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
            ClearMusicCardData();
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

        private void SetMusicCardData(string musicCardId, IMusicCardDataReader musicCardData) {
            MusicCardData.Value = musicCardData;
            MusicCardId = musicCardId;
        }

        public void ClearMusicCardData() {
            SetMusicCardData("", null);
        }

        private void SetState(BoardMusicCardState state) {
            State.Value = state;
        }
    }
}