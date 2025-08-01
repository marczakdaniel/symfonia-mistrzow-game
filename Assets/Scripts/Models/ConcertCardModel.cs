using Assets.Scripts.Data;

namespace Models
{
    public enum ConcertCardState
    {
        Available,
        ReadyToClaim,
        Claimed,
    }

    public class ConcertCardModel
    {
        public ConcertCardData ConcertCardData { get; private set; }
        public ConcertCardState State { get; private set; }

        public ConcertCardModel(ConcertCardData concertCardData)
        {
            this.ConcertCardData = concertCardData;
            this.State = ConcertCardState.Available;
        }

        public void SetState(ConcertCardState state)
        {
            State = state;
        }

        public bool CanClaim(ResourceCollectionModel playerMusicCards)
        {
            var requirements = ConcertCardData.GetRequirements();
            foreach (var requirement in requirements)
            {
                if (playerMusicCards.GetCount(requirement.Key) < requirement.Value) 
                {
                    return false;   
                }
            }

            return true;
        }
    }
}