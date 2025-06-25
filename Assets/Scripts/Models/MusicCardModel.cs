using DefaultNamespace.Data;
using R3;
namespace DefaultNamespace.Models
{
    public enum MusicCardState
    {
        InDeck,
        OnBoard,
        Reserved,
        InPlayerResources,
    }


    public class MusicCardModel
    {
        private readonly MusicCardData data;
        private readonly ReactiveProperty<MusicCardState> state = new ReactiveProperty<MusicCardState>(MusicCardState.InDeck);

        // Additional context information
        private readonly ReactiveProperty<string> ownerId = new ReactiveProperty<string>(null);
        private readonly ReactiveProperty<int> boardPosition = new ReactiveProperty<int>(-1);


        public ReadOnlyReactiveProperty<MusicCardState> State => state;
        public ReadOnlyReactiveProperty<string> OwnerId => ownerId;
        public ReadOnlyReactiveProperty<int> BoardPosition => boardPosition;

        public MusicCardModel(MusicCardData data)
        {
            this.data = data;
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
            ownerId.Value = playerId;
            this.boardPosition.Value = boardPosition;
            SetState(MusicCardState.OnBoard);
        }

        public void SetReserved(string playerId)
        {
            ownerId.Value = playerId;
            SetState(MusicCardState.Reserved);
        }

        public void SetInPlayerResources(string playerId)
        {
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