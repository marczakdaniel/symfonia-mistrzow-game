using System.Collections.Generic;
using Assets.Scripts.Data;
using DefaultNamespace.Data;
using Models;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Events
{
    public class MusicCardDetailsPanelOpenedEvent : GameEvent
    {
        public MusicCardData MusicCardData { get; private set; }
        public int Level { get; private set; }
        public int Position { get; private set; }
        public bool CanBePurchased { get; private set; }

        public MusicCardDetailsPanelOpenedEvent(MusicCardData musicCardData, int level, int position, bool canBePurchased)
        {
            MusicCardData = musicCardData;
            Level = level;
            Position = position;
            CanBePurchased = canBePurchased;
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
        public ResourceType? ResourceType { get; private set; }

        public Dictionary<ResourceType, int> CurrentTokenCounts { get; private set; }

        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerCards { get; private set; }

        public TokenDetailsPanelOpenedEvent(ResourceType? resourceType, Dictionary<ResourceType, int> currentTokenCounts, Dictionary<ResourceType, int> currentPlayerTokens, Dictionary<ResourceType, int> currentPlayerCards)
        {
            ResourceType = resourceType;
            CurrentTokenCounts = currentTokenCounts;
            CurrentPlayerTokens = currentPlayerTokens;
            CurrentPlayerCards = currentPlayerCards;
        }
    }

    public class TokenDetailsPanelClosedEvent : GameEvent
    {
        public TokenDetailsPanelClosedEvent()
        {
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

    public class SelectedTokensConfirmedEvent : GameEvent
    {
        public Dictionary<ResourceType, int> BoardTokens { get; private set; }

        public SelectedTokensConfirmedEvent(Dictionary<ResourceType, int> boardTokens)
        {
            BoardTokens = boardTokens;
        }
    }

    // Return token action events
    public class ReturnTokenWindowOpenedEvent : GameEvent
    {
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public int AllPlayerTokensCount { get; private set; }
        public ReturnTokenWindowOpenedEvent(Dictionary<ResourceType, int> currentPlayerTokens, int allPlayerTokensCount)
        {
            CurrentPlayerTokens = currentPlayerTokens;  
            AllPlayerTokensCount = allPlayerTokensCount;
        }
    }

    public class ReturnTokenWindowClosedEvent : GameEvent
    {
        public ReturnTokenWindowClosedEvent()
        {
        }
    }

    public class TokenAddedToReturnTokensEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }
        public int AllPlayerTokensCount { get; private set; }
        public ResourceType?[] CurrentReturnTokens { get; private set; }

        public TokenAddedToReturnTokensEvent(ResourceType resourceType, int currentTokenCount, int allPlayerTokensCount, ResourceType?[] currentReturnTokens)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
            AllPlayerTokensCount = allPlayerTokensCount;
            CurrentReturnTokens = currentReturnTokens;
        }
    }

    public class TokenRemovedFromReturnTokensEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }
        public int AllPlayerTokensCount { get; private set; }
        public ResourceType?[] CurrentReturnTokens { get; private set; }

        public TokenRemovedFromReturnTokensEvent(ResourceType resourceType, int currentTokenCount, int allPlayerTokensCount, ResourceType?[] currentReturnTokens)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
            AllPlayerTokensCount = allPlayerTokensCount;
            CurrentReturnTokens = currentReturnTokens;
        }
    }

    public class ReturnTokensConfirmedEvent : GameEvent
    {
        public Dictionary<ResourceType, int> BoardTokens { get; private set; }

        public ReturnTokensConfirmedEvent(Dictionary<ResourceType, int> boardTokens)
        {
            BoardTokens = boardTokens;
        }
    }

    // Start Turn Window Events
    public class StartTurnWindowOpenedEvent : GameEvent
    {
        public string CurrentPlayerName { get; private set; }
        public int CurrentRound { get; private set; }
        public Sprite CurrentPlayerAvatar { get; private set; }

        public StartTurnWindowOpenedEvent(string currentPlayerName, int currentRound, Sprite currentPlayerAvatar)
        {
            CurrentPlayerName = currentPlayerName;
            CurrentRound = currentRound;
            CurrentPlayerAvatar = currentPlayerAvatar;
        }
    }

    // Reserve card action events
    public class CardReservedEvent : GameEvent
    {
        public string CardId { get; private set; }
        public int InspirationTokensOnBoard { get; private set; }
        public int PlayerIndex { get; private set; }

        public CardReservedEvent(string cardId, int inspirationTokensOnBoard, int playerIndex)
        {
            CardId = cardId;
            InspirationTokensOnBoard = inspirationTokensOnBoard;
            PlayerIndex = playerIndex;
        }
    }

    // Board Events
    public class PutCardOnBoardEvent : GameEvent
    {
        public int Level { get; private set; }
        public int Position { get; private set; }

        public MusicCardData MusicCardData { get; private set; }

        public PutCardOnBoardEvent(int level, int position, MusicCardData musicCardData)
        {
            Level = level;
            Position = position;
            MusicCardData = musicCardData;
        }
    }

    // Card purchase action events
    public class CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent : GameEvent
    {
        public MusicCardData MusicCardData { get; private set; }

        public Dictionary<ResourceType, int> InitialSelectedTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentCardTokens { get; private set; }
        public Dictionary<ResourceType, int> TokensNeededToPurchase { get; private set; }
        public bool CanBePurchased { get; private set; }

        public CardPurchaseWindowOpenedFromMusicCardDetailsPanelEvent(
            MusicCardData musicCardData, 
            Dictionary<ResourceType, int> currentPlayerTokens, 
            Dictionary<ResourceType, int> initialSelectedTokens, 
            Dictionary<ResourceType, int> currentCardTokens,
            Dictionary<ResourceType, int> tokensNeededToPurchase,
            bool canBePurchased)
        {
            MusicCardData = musicCardData;
            CurrentPlayerTokens = currentPlayerTokens;
            InitialSelectedTokens = initialSelectedTokens;
            CurrentCardTokens = currentCardTokens;
            TokensNeededToPurchase = tokensNeededToPurchase;
            CanBePurchased = canBePurchased;
        }
    }

    public class CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent : GameEvent
    {
        public CardPurchaseWindowClosedFromMusicCardDetailsPanelEvent()
        {
        }
    }

    public class CardPurchaseWindowOpenedFromReservedEvent : GameEvent
    {
        public MusicCardData MusicCardData { get; private set; }

        public Dictionary<ResourceType, int> InitialSelectedTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentCardTokens { get; private set; }
        public Dictionary<ResourceType, int> TokensNeededToPurchase { get; private set; }
        public int CardIndex { get; private set; }
        public bool CanBePurchased { get; private set; }

        public CardPurchaseWindowOpenedFromReservedEvent(
            MusicCardData musicCardData, 
            Dictionary<ResourceType, int> currentPlayerTokens, 
            Dictionary<ResourceType, int> initialSelectedTokens, 
            Dictionary<ResourceType, int> currentCardTokens,
            Dictionary<ResourceType, int> tokensNeededToPurchase,
            int cardIndex,
            bool canBePurchased)
        {
            MusicCardData = musicCardData;
            CurrentPlayerTokens = currentPlayerTokens;
            InitialSelectedTokens = initialSelectedTokens;
            CurrentCardTokens = currentCardTokens;
            TokensNeededToPurchase = tokensNeededToPurchase;
            CardIndex = cardIndex;
            CanBePurchased = canBePurchased;
        }
    }
    
    public class CardPurchaseWindowClosedFromReservedEvent : GameEvent
    {
        public int CardIndex { get; private set; }

        public CardPurchaseWindowClosedFromReservedEvent(int cardIndex)
        {
            CardIndex = cardIndex;
        }
    }

    public class TokenAddedToCardPurchaseEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }

        public TokenAddedToCardPurchaseEvent(ResourceType resourceType, int currentTokenCount)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
        }
    }

    public class TokenRemovedFromCardPurchaseEvent : GameEvent
    {
        public ResourceType ResourceType { get; private set; }
        public int CurrentTokenCount { get; private set; }

        public TokenRemovedFromCardPurchaseEvent(ResourceType resourceType, int currentTokenCount)
        {
            ResourceType = resourceType;
            CurrentTokenCount = currentTokenCount;
        }
    }

    public class CardPurchasedFromBoardEvent : GameEvent
    {
        public string CardId { get; private set; }
        public Dictionary<ResourceType, int> BoardTokens { get; private set; }
        public int PlayerIndex { get; private set; }

        public int Points { get; private set; }

        public CardPurchasedFromBoardEvent(string cardId, Dictionary<ResourceType, int> boardTokens, int playerIndex, int points)
        {
            CardId = cardId;
            BoardTokens = boardTokens;
            PlayerIndex = playerIndex;
            Points = points;
        }
    }

    public class CardPurchasedFromReserveEvent : GameEvent
    {
        public string CardId { get; private set; }

        public List<MusicCardData> ReservedMusicCards { get; private set; }
        public Dictionary<ResourceType, int> BoardTokens { get; private set; }
        public int PlayerIndex { get; private set; }

        public int Points { get; private set; }
        
        public CardPurchasedFromReserveEvent(string cardId, List<MusicCardData> reservedMusicCards, Dictionary<ResourceType, int> boardTokens, int playerIndex, int points)
        {
            CardId = cardId;
            ReservedMusicCards = reservedMusicCards;
            BoardTokens = boardTokens;
            PlayerIndex = playerIndex;
            Points = points;
        }
    }

    // Player Resources Window Events

    public class PlayerResourcesWindowOpenedEvent : GameEvent
    {
        public bool IsCurrentPlayer { get; private set; }
        public bool CanPlayerExecuteAction { get; private set; }
        public string PlayerName { get; private set; }
        public int NumberOfPoints { get; private set; }
        public Sprite PlayerAvatar { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerCards { get; private set; }
        public List<MusicCardData> ReservedMusicCards { get; private set; }
        public List<string> ReservedMusicCardsThatCanBePurchased { get; private set; }

        public PlayerResourcesWindowOpenedEvent(
            bool isCurrentPlayer, 
            bool canPlayerExecuteAction,
            string playerName,
            int numberOfPoints,
            Sprite playerAvatar,
            Dictionary<ResourceType, int> currentPlayerTokens, 
            Dictionary<ResourceType, int> currentPlayerCards, 
            List<MusicCardData> reservedMusicCards,
            List<string> reservedMusicCardsThatCanBePurchased)
        {
            IsCurrentPlayer = isCurrentPlayer;
            CanPlayerExecuteAction = canPlayerExecuteAction;
            PlayerName = playerName;
            NumberOfPoints = numberOfPoints;
            PlayerAvatar = playerAvatar;
            CurrentPlayerTokens = currentPlayerTokens;
            CurrentPlayerCards = currentPlayerCards;
            ReservedMusicCards = reservedMusicCards;
            ReservedMusicCardsThatCanBePurchased = reservedMusicCardsThatCanBePurchased;
        }
    }

    public class PlayerResourcesWindowClosedEvent : GameEvent
    {
        public PlayerResourcesWindowClosedEvent()
        {
        }
    }

    // Concert Cards Window Events
    public class ConcertCardsWindowOpenedEvent : GameEvent
    {
        public List<ConcertCardData> ConcertCards { get; private set; }
        public List<ConcertCardState> CardStates { get; private set; }
        public List<Sprite> OwnerAvatars { get; private set; }
        public ConcertCardsWindowOpenedEvent(List<ConcertCardData> concertCards, List<ConcertCardState> cardStates, List<Sprite> ownerAvatars)
        {
            ConcertCards = concertCards;
            CardStates = cardStates;
            OwnerAvatars = ownerAvatars;
        }
    }

    public class ConcertCardClaimedEvent : GameEvent
    {
        public int Points { get; private set; }

        public ConcertCardClaimedEvent(int points)
        {
            Points = points;
        }
    }

    public class ConcertCardsWindowClosedEvent : GameEvent
    {
        public ConcertCardsWindowClosedEvent()
        {
        }
    }

    // Info Window Events
    public class InfoWindowOpenedEvent : GameEvent
    {
        public string Description { get; private set; }

        public InfoWindowOpenedEvent(string description)
        {
            Description = description;
        }
    }

    public class InfoWindowClosedEvent : GameEvent
    {
        public InfoWindowClosedEvent()
        {
        }
    }

    // Menu Window Events
    public class StartPageWindowOpenedEvent : GameEvent
    {
        public StartPageWindowOpenedEvent()
        {
        }   
    }

    public class StartPageWindowClosedEvent : GameEvent
    {
        public StartPageWindowClosedEvent()
        {
        }
    }

    public class GameCreationWindowOpenedEvent : GameEvent
    {
        public GameCreationWindowOpenedEvent()
        {
        }
    }

    public class GameCreationWindowClosedEvent : GameEvent
    {
        public GameCreationWindowClosedEvent()
        {
        }
    }

    public class CreatePlayerWindowOpenedEvent : GameEvent
    {
        public List<Sprite> AvailablePlayerAvatars { get; private set; }
        public CreatePlayerWindowOpenedEvent(List<Sprite> availablePlayerAvatars)
        {
            AvailablePlayerAvatars = availablePlayerAvatars;
        }
    }

    public class CreatePlayerWindowClosedEvent : GameEvent
    {
        public CreatePlayerWindowClosedEvent()
        {
        }
    }

    public class PlayerAddedEvent : GameEvent
    {
        public List<string> PlayerNames { get; private set; }
        public List<Sprite> PlayerAvatars { get; private set; }
        public PlayerAddedEvent(List<string> playerNames, List<Sprite> playerAvatars)
        {
            PlayerNames = playerNames;
            PlayerAvatars = playerAvatars;
        }
    }

    // Reserve Deck Card Window Events
    public class ReserveDeckCardWindowOpenedEvent : GameEvent
    {
        public int CardLevel { get; private set; }

        public ReserveDeckCardWindowOpenedEvent(int cardLevel)
        {
            CardLevel = cardLevel;
        }
    }

    public class ReserveDeckCardWindowClosedEvent : GameEvent
    {
        public ReserveDeckCardWindowClosedEvent()
        {
        }
    }

    public class DeckCardReservedEvent : GameEvent
    {
        public MusicCardData MusicCardData { get; private set; }

        public DeckCardReservedEvent(MusicCardData musicCardData)
        {
            MusicCardData = musicCardData;
        }
    }

    // Deck Card Info Window Events
    public class DeckCardInfoWindowOpenedEvent : GameEvent
    {
        public MusicCardData MusicCardData { get; private set; }

        public DeckCardInfoWindowOpenedEvent(MusicCardData musicCardData)
        {
            MusicCardData = musicCardData;
        }
    }

    public class DeckCardInfoWindowClosedEvent : GameEvent
    {
        public int PlayerIndex { get; private set; }

        public DeckCardInfoWindowClosedEvent(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }

    public class PlayerResourcesUpdatedEvent : GameEvent
    {
        public string PlayerId { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerTokens { get; private set; }
        public Dictionary<ResourceType, int> CurrentPlayerCards { get; private set; }

        public PlayerResourcesUpdatedEvent(string playerId, Dictionary<ResourceType, int> currentPlayerTokens, Dictionary<ResourceType, int> currentPlayerCards)
        {
            PlayerId = playerId;
            CurrentPlayerTokens = currentPlayerTokens;
            CurrentPlayerCards = currentPlayerCards;
        }
    }

    public class ResultWindowOpenedEvent : GameEvent
    {
        public List<string> PlayerNames { get; private set; }
        public List<int> PlayerPoints { get; private set; }
        public List<Sprite> PlayerAvatars { get; private set; }

        public ResultWindowOpenedEvent(List<string> playerNames, List<int> playerPoints, List<Sprite> playerAvatars)
        {
            PlayerNames = playerNames;
            PlayerPoints = playerPoints;
            PlayerAvatars = playerAvatars;
        }
    }
}