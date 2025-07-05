using System;
using System.Collections.Generic;
using DefaultNamespace.Data;

namespace Events
{
    /// <summary>
    /// Event fired when a player buys a music card
    /// </summary>
    public class MusicCardBoughtEvent : GameEvent
    {
        public string PlayerId { get; }
        public string MusicCardId { get; }
        public Dictionary<string, int> CostPaid { get; }

        public MusicCardBoughtEvent(string playerId, string musicCardId, Dictionary<string, int> costPaid)
        {
            PlayerId = playerId;
            MusicCardId = musicCardId;
            CostPaid = costPaid ?? new Dictionary<string, int>();
        }
    }

    /// <summary>
    /// Event fired when a player reserves a music card
    /// </summary>
    public class MusicCardReservedEvent : GameEvent
    {
        public string PlayerId { get; }
        public string MusicCardId { get; }
        public bool ReceivedToken { get; }

        public MusicCardReservedEvent(string playerId, string musicCardId, bool receivedToken)
        {
            PlayerId = playerId;
            MusicCardId = musicCardId;
            ReceivedToken = receivedToken;
        }
    }

    /// <summary>
    /// Event fired when a new music card is added to the board
    /// </summary>
    public class MusicCardAddedToBoardEvent : GameEvent
    {
        public string MusicCardId { get; }
        public int Position { get; }
        public int Level { get; }

        public MusicCardAddedToBoardEvent(string musicCardId, int position, int level)
        {
            MusicCardId = musicCardId;
            Position = position;
            Level = level;
        }
    }

    /// <summary>
    /// Event fired when the current player changes
    /// </summary>
    public class PlayerTurnChangedEvent : GameEvent
    {
        public string NewCurrentPlayerId { get; }
        public string PreviousPlayerId { get; }
        public int TurnNumber { get; }

        public PlayerTurnChangedEvent(string newCurrentPlayerId, string previousPlayerId, int turnNumber)
        {
            NewCurrentPlayerId = newCurrentPlayerId;
            PreviousPlayerId = previousPlayerId;
            TurnNumber = turnNumber;
        }
    }

    /// <summary>
    /// Event fired when game state changes
    /// </summary>
    public class GameStateChangedEvent : GameEvent
    {
        public string NewState { get; }
        public string PreviousState { get; }

        public GameStateChangedEvent(string newState, string previousState)
        {
            NewState = newState;
            PreviousState = previousState;
        }
    }

    /// <summary>
    /// Event fired when a player resources change
    /// </summary>
    public class PlayerResourcesChangedEvent : GameEvent
    {
        public string PlayerId { get; }
        public Dictionary<string, int> ResourceChanges { get; }
        public Dictionary<string, int> NewResourceAmounts { get; }

        public PlayerResourcesChangedEvent(string playerId, Dictionary<string, int> resourceChanges, Dictionary<string, int> newResourceAmounts)
        {
            PlayerId = playerId;
            ResourceChanges = resourceChanges ?? new Dictionary<string, int>();
            NewResourceAmounts = newResourceAmounts ?? new Dictionary<string, int>();
        }
    }

    /// <summary>
    /// Event fired when game ends
    /// </summary>
    public class GameEndedEvent : GameEvent
    {
        public string WinnerId { get; }
        public List<string> PlayerRanking { get; }
        public Dictionary<string, int> FinalScores { get; }

        public GameEndedEvent(string winnerId, List<string> playerRanking, Dictionary<string, int> finalScores)
        {
            WinnerId = winnerId;
            PlayerRanking = playerRanking ?? new List<string>();
            FinalScores = finalScores ?? new Dictionary<string, int>();
        }
    }

    /// <summary>
    /// Event fired when an animation should be played
    /// </summary>
    public class AnimationRequestEvent : GameEvent
    {
        public string AnimationType { get; }
        public string TargetId { get; }
        public Dictionary<string, object> Parameters { get; }

        public AnimationRequestEvent(string animationType, string targetId, Dictionary<string, object> parameters = null)
        {
            AnimationType = animationType;
            TargetId = targetId;
            Parameters = parameters ?? new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// Event fired when UI should be updated
    /// </summary>
    public class UIUpdateRequestEvent : GameEvent
    {
        public string UIElementId { get; }
        public string UpdateType { get; }
        public Dictionary<string, object> Data { get; }

        public UIUpdateRequestEvent(string uiElementId, string updateType, Dictionary<string, object> data = null)
        {
            UIElementId = uiElementId;
            UpdateType = updateType;
            Data = data ?? new Dictionary<string, object>();
        }
    }

    public class StartGameEvent : GameEvent
    {
        // Light event - no data payload, just notification
        // Board data will be read from GameModel when needed
        public StartGameEvent()
        {
        }
    }
} 