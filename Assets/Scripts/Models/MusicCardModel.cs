using DefaultNamespace.Data;
using R3;
using System;

namespace DefaultNamespace.Models
{
    public enum MusicCardState
    {
        InDeck,
        OnBoard,
        Reserved,
        InPlayerResources,
    }

    public class MusicCardModel : IDisposable
    {
        private readonly MusicCardData data;
        private readonly ReactiveProperty<MusicCardState> state = new ReactiveProperty<MusicCardState>(MusicCardState.InDeck);

        // Additional context information
        private readonly ReactiveProperty<string> ownerId = new ReactiveProperty<string>(null);
        private readonly ReactiveProperty<int> boardPosition = new ReactiveProperty<int>(-1);

        public ReadOnlyReactiveProperty<MusicCardState> State => state;
        public ReadOnlyReactiveProperty<string> OwnerId => ownerId;
        public ReadOnlyReactiveProperty<int> BoardPosition => boardPosition;

        // Data properties
        public string Id => data.id;
        public MusicCardData Data => data;

        public MusicCardModel(MusicCardData data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public void SetState(MusicCardState newState)
        {
            var previousState = state.Value;
            state.Value = newState;

            ClearContextForStateChange(previousState, newState);
        }

        private void ClearContextForStateChange(MusicCardState previousState, MusicCardState newState)
        {
            if (newState == MusicCardState.Reserved || newState == MusicCardState.InPlayerResources)
            {
                boardPosition.Value = -1;
            }
        }

        // State change logic
        public void SetOnBoard(string playerId, int boardPosition)
        {
            if (string.IsNullOrEmpty(playerId))
                throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));
            if (boardPosition < 0)
                throw new ArgumentException("Board position must be non-negative", nameof(boardPosition));

            ownerId.Value = playerId;
            this.boardPosition.Value = boardPosition;
            SetState(MusicCardState.OnBoard);
        }

        public void SetReserved(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
                throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));

            ownerId.Value = playerId;
            SetState(MusicCardState.Reserved);
        }

        public void SetInPlayerResources(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
                throw new ArgumentException("Player ID cannot be null or empty", nameof(playerId));

            ownerId.Value = playerId;
            SetState(MusicCardState.InPlayerResources);
        }

        public void Dispose()
        {
            state?.Dispose();
            ownerId?.Dispose();
            boardPosition?.Dispose();
        }
    }
}