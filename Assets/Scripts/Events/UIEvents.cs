using DefaultNamespace.Data;

namespace Events
{
    public class MusicCardDetailsPanelOpenedEvent : GameEvent
    {
        public string MusicCardId { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }

        public MusicCardDetailsPanelOpenedEvent(string musicCardId, int level, int position, bool canCardBePurchased, bool canCardBeReserved)
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

    // Token Action Events

    public class TokenDetailsPanelOpenedEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }

        public TokenDetailsPanelOpenedEvent(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
    }

    public class TokenDetailsPanelClosedEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }

        public TokenDetailsPanelClosedEvent(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }
    }

    public class TokenAddedToSelectedTokensEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }
        public ResourceType?[] CurrentSelectedTokens { get; private set; }

        public TokenAddedToSelectedTokensEvent(ResourceType resourceType, int currentTokenCount, ResourceType?[] currentSelectedTokens)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
            CurrentSelectedTokens = currentSelectedTokens;
        }
    }

    public class TokenRemovedFromSelectedTokensEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }
        public ResourceType?[] CurrentSelectedTokens { get; private set; }

        public TokenRemovedFromSelectedTokensEvent(ResourceType resourceType, int currentTokenCount, ResourceType?[] currentSelectedTokens)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
            CurrentSelectedTokens = currentSelectedTokens;
        }
    }
}