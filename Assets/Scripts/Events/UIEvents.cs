namespace Events
{
    public class MusicCardDetailsPanelOpenedEvent : GameEvent
    {
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public MusicCardDetailsPanelOpenedEvent(string musicCardId, int level, int position)
        {
            MusicCardId = musicCardId;
            Level = level;
            Position = position;
        }
    }

    public class MusicCardDetailsPanelClosedEvent : GameEvent
    {
        public string MusicCardId { get; private set; }

        public MusicCardDetailsPanelClosedEvent(string musicCardId)
        {
            MusicCardId = musicCardId;
        }
    }

    public class MusicCardDetailsPanelAnimationFinishedEvent : GameEvent
    {
        public string MusicCardId { get; private set; }

        public MusicCardDetailsPanelAnimationFinishedEvent(string musicCardId)
        {
            MusicCardId = musicCardId;
        }
    }
}