using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Models
{

    public interface IBoardSlotReader
    {
        int Position { get; }
        int Level { get; }
        string CardId { get; }
        bool IsEmpty { get; }
        bool IsOccupied { get; }
        IMusicCardDataReader GetMusicCardData();
    }
    public class BoardSlot : IBoardSlotReader
    {
        public int Position { get; private set; }
        public int Level { get; private set; }
        public string CardId { get; private set; }
        public bool IsEmpty => string.IsNullOrEmpty(CardId);
        public bool IsOccupied => !IsEmpty;

        public BoardSlot(int position, int level)
        {
            Position = position;
            Level = level;
            CardId = null;
        }

        public bool PlaceCard(string cardId)
        {
            if (IsOccupied) 
            {
                Debug.LogWarning($"[BoardSlot] Cannot place card at position {Position} because it is already occupied.");
                return false;
            }

            if (string.IsNullOrEmpty(cardId))
            {
                Debug.LogWarning($"[BoardSlot] Cannot place card at position {Position} because cardId is null or empty.");
                return false;
            }

            CardId = cardId;
            Debug.Log($"[BoardSlot] Placed card {cardId} at position {Position}.");
            return true;
        }

        public string RemoveCard()
        {
            if (IsEmpty)
            {
                Debug.LogWarning($"[BoardSlot] Cannot remove card at position {Position} because it is empty.");
                return null;
            }

            var removedCardId = CardId;
            CardId = null;
            Debug.Log($"[BoardSlot] Removed card from position {Position}.");
            return removedCardId;
        }

        public MusicCardData GetMusicCard()
        {
            return IsEmpty ? null : MusicCardRepository.Instance.GetCard(CardId);
        }

        public override string ToString()
        {
            return $"BoardSlot(Position: {Position}, Level: {Level}, CardId: {CardId}, IsEmpty: {IsEmpty})";
        }

        public IMusicCardDataReader GetMusicCardData()
        {
            return IsEmpty ? null : MusicCardRepository.Instance.GetCard(CardId);
        }
    }

    public class BoardLevel
    {
        public int Level { get; private set; }
        public BoardSlot[] Slots { get; private set; } 
        public MusicCardCollectionModel Deck { get; private set; }

        public int SlotCount => Slots.Length;
        public int OccupiedSlotCount => Slots.Count(slot => slot.IsOccupied);
        public int EmptySlotCount => Slots.Count(slot => slot.IsEmpty);

        public BoardLevel(int level)
        {
            Level = level;
            Slots = new BoardSlot[4];
            
            for (int i = 0; i < 4; i++)
            {
                Slots[i] = new BoardSlot(i, level);
            }

            Deck = new MusicCardCollectionModel();
        }

        public bool RemoveCard(string cardId)
        {
            var slot = FindSlotWithCard(cardId);
            if (slot == null || slot.IsEmpty)
            {
                Debug.LogWarning($"[BoardLevel] Cannot remove card {cardId} from level: {Level} because it is not on the board.");
                return false;
            }

            return slot.RemoveCard() == cardId;
        }

        public BoardSlot GetSlot(int position)
        {
            if (position < 0 || position >= Slots.Length)
            {
                Debug.LogWarning($"[BoardLevel] Invalid slot position: {position}");
                return null;
            }

            return Slots[position];
        }

        public BoardSlot FindSlotWithCard(string cardId)
        {
            return Slots.FirstOrDefault(slot => slot.IsOccupied && slot.CardId == cardId);
        }

        public IEnumerable<BoardSlot> GetOccupiedSlots()
        {
            return Slots.Where(slot => slot.IsOccupied);
        }

        public IEnumerable<BoardSlot> GetEmptySlots()
        {
            return Slots.Where(slot => slot.IsEmpty);
        }

        public string PurchaseCardFromPosition(int position)
        {
            var slot = GetSlot(position);
            if (slot == null || slot.IsEmpty)
            {
                Debug.LogWarning($"[BoardLevel] Cannot purchase card from level: {Level} position: {position} because it is empty.");
                return null;
            }

            var purchasedCardId = slot.RemoveCard();
            
            return purchasedCardId;
        }

        public bool RefillSlot(BoardSlot slot)
        {
            if (slot.IsOccupied)
            {
                Debug.LogWarning($"[BoardLevel] Cannot refill slot at level: {Level} because it is already occupied.");
                return false;
            }

            if (Deck.IsEmpty)
            {
                Debug.LogWarning($"[BoardLevel] Cannot refill slot at level: {Level} because deck is empty.");
                return false;
            }

            var nextCardId = Deck.GetRandomCardId();
            if (string.IsNullOrEmpty(nextCardId))
            {
                Debug.LogWarning($"[BoardLevel] Cannot refill slot at level: {Level} because deck is empty.");
                return false;
            }

            Deck.RemoveCard(nextCardId);
            return slot.PlaceCard(nextCardId);
        }

        public void RefillAllSlots()
        {
            foreach (var slot in GetEmptySlots().ToList())
            {
                RefillSlot(slot);
            }
        }

        public override string ToString()
        {
            return $"BoardLevel(Level: {Level}, Slots: {string.Join(", ", Slots.Select(slot => slot.ToString()))}, Deck: {Deck.Count})";
        }

        public void AddCardToDeck(string cardId)
        {
            Deck.AddCard(cardId);
        }
    }
    public class BoardModel
    {
        public BoardLevel[] Levels { get; private set; }
        public ResourceCollectionModel TokenResources { get; private set; }

        private const int numberOfLevels = 3;

        // Create BoardModel
        public BoardModel()
        {
            Levels = new BoardLevel[numberOfLevels];
            for (int i = 0; i < numberOfLevels; i++)
            {
                Levels[i] = new BoardLevel(i + 1);
            }
            TokenResources = new ResourceCollectionModel();
        
        }

        // Initialize BoardModel
        public void Initialize(BoardConfig boardConfig)
        {
            InitializeTokenResources(boardConfig.TokenResources);
            InitializeBoardMusicCard(boardConfig.BoardMusicCardConfig);
        }

        private void InitializeTokenResources(BoardTokenConfig boardTokenConfig)
        {
            TokenResources.AddResource(ResourceType.Melody, boardTokenConfig.InitialMelody);
            TokenResources.AddResource(ResourceType.Harmony, boardTokenConfig.InitialHarmony);
            TokenResources.AddResource(ResourceType.Rhythm, boardTokenConfig.InitialRhythm);
            TokenResources.AddResource(ResourceType.Instrumentation, boardTokenConfig.InitialInstrumentation);
            TokenResources.AddResource(ResourceType.Dynamics, boardTokenConfig.InitialDynamics);
            TokenResources.AddResource(ResourceType.Inspiration, boardTokenConfig.InitialInspiration);
        }

        private void InitializeBoardMusicCard(BoardMusicCardConfig boardMusicCardConfig)
        {
            InitializeDeck(boardMusicCardConfig.CardInDecks);
            InitializeSlots(1, boardMusicCardConfig.Level1SlotsCardIds);
            InitializeSlots(2, boardMusicCardConfig.Level2SlotsCardIds);
            InitializeSlots(3, boardMusicCardConfig.Level3SlotsCardIds);
        }

        private void InitializeDeck(List<string> cardInDecks)
        {
            foreach (var cardId in cardInDecks)
            {
                AddCardToDecks(cardId);
            }
        }

        private void AddCardToDecks(string cardId)
        {
            var card = MusicCardRepository.Instance.GetCard(cardId);
            var level = GetLevel(card.level);
            if (level != null)
            {
                level.AddCardToDeck(cardId);
            }
        }

        private void InitializeSlots(int level, string[] cardIds)
        {
            for (int i = 0; i < cardIds.Length; i++)
            {
                var slot = GetLevel(level).GetSlot(i);
                if (slot != null)
                {
                    slot.PlaceCard(cardIds[i]);
                }
            }
        }


        // Start Game
        public void StartGame()
        {
            RefillAllEmptySlots();
        }

        public BoardLevel GetLevel(int level)
        {
            if (level < 1 || level > numberOfLevels)
            {
                Debug.LogWarning($"[BoardModel] Invalid level: {level}");
                return null;
            }

            return Levels[level - 1];
        }

        public bool RemoveCardFromBoard(string cardId)
        {
            var cardModel = MusicCardRepository.Instance.GetCard(cardId);
            if (cardModel == null)
            {
                return false;
            }

            var level = GetLevel(cardModel.level);
            if (level == null)
            {
                return false;
            }

            return level.RemoveCard(cardId);
        }
        
        // Card Management



        public string PurchaseCard(int level, int position)
        {
            var boardLevel = GetLevel(level);
            if (boardLevel == null) return null;
            
            return boardLevel.PurchaseCardFromPosition(position);
        }

        public (int level, int position) FindCard(string cardId)
        {
            for (int levelIndex = 0; levelIndex < Levels.Length; levelIndex++)
            {
                var boardLevel = Levels[levelIndex];
                var slot = boardLevel.FindSlotWithCard(cardId);
                if (slot != null)
                {
                    return (boardLevel.Level, slot.Position);
                }
            }

            return (-1, -1);
        }

        public MusicCardData[,] GetCurrentBoardCards()
        {
            var boardCards = new MusicCardData[numberOfLevels, 4];
            
            for (int level = 0; level < numberOfLevels; level++)
            {
                for (int position = 0; position < 4; position++)
                {
                    var slot = Levels[level].GetSlot(position);
                    boardCards[level, position] = slot?.GetMusicCard();
                }
            }
            
            return boardCards;
        }

        private void RefillAllEmptySlots()
        {
            foreach (var level in Levels)
            {
                level.RefillAllSlots();
            }
        }

        public int GetTokenCount(ResourceType resourceType)
        {
            return TokenResources.GetCount(resourceType);
        }

        public bool AddTokens(ResourceCollectionModel tokens)
        {
            TokenResources.Add(tokens);
            return true;
        }

        public bool RemoveTokens(ResourceCollectionModel tokens)
        {
            TokenResources.Subtract(tokens);
            return true;
        }
    }
}