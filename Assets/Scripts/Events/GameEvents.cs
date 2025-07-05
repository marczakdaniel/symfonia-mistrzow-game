using DefaultNamespace.Data;
using Models;

namespace Events
{
    // Game State Events
    public class GameStartedEvent : GameEvent
    {        
        public GameStartedEvent()
        {
        }
    }

    public class GameEndedEvent : GameEvent
    {
        public string WinnerPlayerId { get; }
        
        public GameEndedEvent(string winnerPlayerId)
        {
            WinnerPlayerId = winnerPlayerId;
        }
    }

    public class PlayerTurnStartedEvent : GameEvent
    {
        public string PlayerId { get; }
        
        public PlayerTurnStartedEvent(string playerId)
        {
            PlayerId = playerId;
        }
    }

    public class PlayerTurnEndedEvent : GameEvent
    {
        public string PlayerId { get; }
        
        public PlayerTurnEndedEvent(string playerId)
        {
            PlayerId = playerId;
        }
    }

    // Card Events
    public class CardPurchasedEvent : GameEvent
    {
        public string PlayerId { get; }
        public string CardId { get; }
        public ResourceCollectionModel TokensUsed { get; }
        
        public CardPurchasedEvent(string playerId, string cardId, ResourceCollectionModel tokensUsed)
        {
            PlayerId = playerId;
            CardId = cardId;
            TokensUsed = tokensUsed;
        }
    }

    public class CardReservedEvent : GameEvent
    {
        public string PlayerId { get; }
        public string CardId { get; }
        public int FromLevel { get; }
        public bool FromDeck { get; }
        
        public CardReservedEvent(string playerId, string cardId, int fromLevel, bool fromDeck = false)
        {
            PlayerId = playerId;
            CardId = cardId;
            FromLevel = fromLevel;
            FromDeck = fromDeck;
        }
    }

    public class CardAddedToBoardEvent : GameEvent
    {
        public string CardId { get; }
        public int Level { get; }
        public int Position { get; }
        
        public CardAddedToBoardEvent(string cardId, int level, int position)
        {
            CardId = cardId;
            Level = level;
            Position = position;
        }
    }

    public class CardRemovedFromBoardEvent : GameEvent
    {
        public string CardId { get; }
        public int Level { get; }
        public int Position { get; }
        
        public CardRemovedFromBoardEvent(string cardId, int level, int position)
        {
            CardId = cardId;
            Level = level;
            Position = position;
        }
    }

    // Token Events
    public class TokensTakenEvent : GameEvent
    {
        public string PlayerId { get; }
        public ResourceCollectionModel TokensTaken { get; }
        
        public TokensTakenEvent(string playerId, ResourceCollectionModel tokensTaken)
        {
            PlayerId = playerId;
            TokensTaken = tokensTaken;
        }
    }

    public class TokensReturnedEvent : GameEvent
    {
        public string PlayerId { get; }
        public ResourceCollectionModel TokensReturned { get; }
        
        public TokensReturnedEvent(string playerId, ResourceCollectionModel tokensReturned)
        {
            PlayerId = playerId;
            TokensReturned = tokensReturned;
        }
    }

    // Board Events
    public class BoardInitializedEvent : GameEvent
    {
        public BoardModel Board { get; }
        
        public BoardInitializedEvent(BoardModel board)
        {
            Board = board;
        }
    }

    public class BoardUpdatedEvent : GameEvent
    {
        public BoardModel Board { get; }
        
        public BoardUpdatedEvent(BoardModel board)
        {
            Board = board;
        }
    }

    // Player Events
    public class PlayerAddedEvent : GameEvent
    {
        public PlayerModel Player { get; }
        
        public PlayerAddedEvent(PlayerModel player)
        {
            Player = player;
        }
    }

    public class PlayerResourcesChangedEvent : GameEvent
    {
        public string PlayerId { get; }
        public ResourceCollectionModel NewResources { get; }
        
        public PlayerResourcesChangedEvent(string playerId, ResourceCollectionModel newResources)
        {
            PlayerId = playerId;
            NewResources = newResources;
        }
    }

    public class PlayerScoreChangedEvent : GameEvent
    {
        public string PlayerId { get; }
        public int NewScore { get; }
        
        public PlayerScoreChangedEvent(string playerId, int newScore)
        {
            PlayerId = playerId;
            NewScore = newScore;
        }
    }

    // UI Events
    public class UIUpdateCompletedEvent : GameEvent
    {
        public string OriginalEventId { get; }
        public string ComponentName { get; }
        
        public UIUpdateCompletedEvent(string originalEventId, string componentName)
        {
            OriginalEventId = originalEventId;
            ComponentName = componentName;
        }
    }
} 