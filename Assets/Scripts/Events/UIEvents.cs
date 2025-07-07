namespace Events
{
    public class MusicCardDetailsPanelOpenedEvent : GameEvent
    {
        public string MusicCardId { get; private set; }

        public MusicCardDetailsPanelOpenedEvent(string musicCardId)
        {
            MusicCardId = musicCardId;
        }
    }

    public class MusicCardDetailsPanelClosedEvent : GameEvent
    {
        public MusicCardDetailsPanelClosedEvent()
        {

        }
    }

}