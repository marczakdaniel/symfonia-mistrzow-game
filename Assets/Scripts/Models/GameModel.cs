using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using UnityEngine;

namespace Models
{   
    public interface IGameModelReader
    {
        IMusicCardDataReader[,] GetCurrentBoardCards();
        IBoardSlotReader GetBoardSlot(int level, int position);
        int GetBoardTokenCount(ResourceType resourceType);
        ITurnModelReader GetTurnModelReader();
        PlayerModel[] GetPlayers();
    }

    public class GameModel : IGameModelReader
    {
        public string GameId { get; private set; }
        public string GameName { get; private set; }
        public int PlayerCount { get; private set; }


        
        public List<PlayerModel> Players { get; }
        public BoardModel Board { get; private set; }

        public TurnModel Turn { get; private set; }

        public GameModel()
        {
            //GameId = Guid.NewGuid().ToString();
            //GameName = gameName;
            Board = new BoardModel();
            Players = new List<PlayerModel>();
            Turn = new TurnModel();
        }

        private bool isInitialized = false;

        public void Initialize(GameConfig gameConfig)
        {
            InitializePlayers(gameConfig.playerConfigs);
            InitializeBoard(gameConfig.boardConfig);

            isInitialized = true;
        }

        private void InitializePlayers(PlayerConfig[] playerConfigs)
        {
            foreach (var playerConfig in playerConfigs)
            {
                var player = new PlayerModel(playerConfig);
                AddPlayer(player);
            }
        }

        private void InitializeBoard(BoardConfig boardConfig)
        {
            Board.Initialize(boardConfig);
        }

        public void AddPlayer(PlayerModel player)
        {
            if (player == null || Players.Contains(player))
            {
                return;
            }

            Players.Add(player);

        }

        public PlayerModel GetPlayer(string playerId)
        {
            return Players.FirstOrDefault(p => p.PlayerId == playerId);
        }

        public string GetNextPlayerId(string currentPlayerId)
        {
            if (currentPlayerId == null)
            {
                return Players[0].PlayerId;
            }

            var currentIndex = Players.FindIndex(p => p.PlayerId == currentPlayerId);
            return Players[(currentIndex + 1) % Players.Count].PlayerId;
        }

        public bool IsPlayerExists(string playerId)
        {
            return Players.Any(p => p.PlayerId == playerId);
        }

        public PlayerModel[] GetPlayers()
        {
            return Players.ToArray();
        }

        // Game Flow Management

        // Start Game Command
        public bool CanStartGame()
        {
            return isInitialized;
        }

        public bool StartGame()
        {
            // 1. Initialize Board
            Board.StartGame();
            return true;
        }

        // Player Action Management

        public bool PurchaseCard(string playerId, string cardId, ResourceCollectionModel selectedTokens)
        {
            if (!ValidatePurchaseCard(playerId, cardId))
            {
                return false;
            }

            Debug.Log($"[GameModel] Purchasing card {cardId} for player {playerId}");

            try
            {
                return ExecutePurchaseCard(playerId, cardId);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error purchasing card {cardId} for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool PurchaseCardFromReserved(string playerId, string cardId)
        {
            if (!ValidatePurchaseCardFromReserved(playerId, cardId))
            {
                return false;
            }

            Debug.Log($"[GameModel] Purchasing card {cardId} from reserved for player {playerId}");

            try
            {
                return ExecutePurchaseCardFromReserved(playerId, cardId);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error purchasing card {cardId} from reserved for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool ReserveCard(string playerId, string cardId)
        {
            if (!ValidateReserveCard(playerId, cardId))
            {
                return false;
            }

            Debug.Log($"[GameModel] Reserving card {cardId} for player {playerId}");

            try
            {
                return ExecuteReserveCard(playerId, cardId);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error reserving card {cardId} for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool ReserveCardFromDeck(string playerId, int deckLevel)
        {
            if (!ValidateReserveCardFromDeck(playerId, deckLevel))
            {
                return false;
            }

            try
            {
                return ExecuteReserveCardFromDeck(playerId, deckLevel);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error reservice form deck level {deckLevel} for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool TakeTokens(string playerId, ResourceCollectionModel tokensToTake)
        {
            if (!ValidateTakeTokens(playerId, tokensToTake))
            {
                return false;
            }

            Debug.Log($"[GameModel] Taking tokens {tokensToTake} for player {playerId}");

            try
            {
                return ExecuteTakeTokens(playerId, tokensToTake);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error taking tokens {tokensToTake} for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool ReturnPlayersTokens(string playerId, ResourceCollectionModel tokensToReturn)
        {
            if (!ValidateReturnTokens(playerId, tokensToReturn))
            {
                return false;
            }

            Debug.Log($"[GameModel] Returning tokens {tokensToReturn} for player {playerId}");

            try
            {
                return ExecuteReturnTokens(playerId, tokensToReturn);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error returning tokens {tokensToReturn} for player {playerId}: {e.Message}");
                return false;
            }
        }

        public bool EndTurn(string playerId)
        {
            if (!ValidateEndTurn(playerId))
            {
                return false;
            }

            Debug.Log($"[GameModel] Ending turn for player {playerId}");

            try
            {
                return ExecuteEndTurn(playerId);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameModel] Error ending turn for player {playerId}: {e.Message}");
                return false;
            }
        }

        // Private Transaction Methods

        private bool ValidatePurchaseCard(string playerId, string cardId)
        {
            return true;
        }

        private bool ExecutePurchaseCard(string playerId, string cardId)
        {
            // 1. Remove tokens from player
            var player = GetPlayer(playerId);

            // 2. Remove card from board
            // 3. Move card from board to player collection
            // 4. Add new card to board - maybe not needed - different approach


            
            return true;
        }

        private bool ValidatePurchaseCardFromReserved(string playerId, string cardId)
        {
            return true;
        }

        private bool ExecutePurchaseCardFromReserved(string playerId, string cardId)
        {
            return true;
        }

        private bool ValidateReserveCard(string playerId, string cardId)
        {
            return true;
        }

        private bool ExecuteReserveCard(string playerId, string cardId)
        {
            // 1. Remove card from board    
            // 2. Move card from board to player reserved cards
            // 3. Add new card to board - maybe not needed - different approach - maybe not needed

            var player = GetPlayer(playerId);
            Board.RemoveCardFromBoard(cardId);
            player.AddCardToReserved(cardId);

            return true;
        }

        private bool ValidateReserveCardFromDeck(string playerId, int deckLevel)
        {
            return true;
        }

        private bool ExecuteReserveCardFromDeck(string playerId, int deckLevel)
        {
            return true;
        }

        private bool ValidateTakeTokens(string playerId, ResourceCollectionModel tokensToTake)
        {
            return true;
        }

        private bool ExecuteTakeTokens(string playerId, ResourceCollectionModel tokensToTake)
        {
            return true;
        }

        private bool ValidateReturnTokens(string playerId, ResourceCollectionModel tokensToReturn)
        {
            return true;
        }

        private bool ExecuteReturnTokens(string playerId, ResourceCollectionModel tokensToReturn)
        {
            return true;
        }

        private bool ValidateEndTurn(string playerId)
        {
            return true;
        }

        private bool ExecuteEndTurn(string playerId)
        {
            return true;
        }

        // Query

        public MusicCardData[,] GetBoard()
        {
            return Board.GetCurrentBoardCards();
        }

        // IGameModelReader implementation - returns read-only interfaces
        public IMusicCardDataReader[,] GetCurrentBoardCards()
        {
            var boardCards = GetBoard();
            var readOnlyBoardCards = new IMusicCardDataReader[boardCards.GetLength(0), boardCards.GetLength(1)];
            
            for (int i = 0; i < boardCards.GetLength(0); i++)
            {
                for (int j = 0; j < boardCards.GetLength(1); j++)
                {
                    readOnlyBoardCards[i, j] = boardCards[i, j]; // Implicit conversion to IMusicCardDataReader
                }
            }
            
            return readOnlyBoardCards;
        }

        public IBoardSlotReader GetBoardSlot(int level, int position)
        {
            return Board.GetLevel(level).GetSlot(position);
        }

        public int GetBoardTokenCount(ResourceType resourceType)
        {
            return Board.GetTokenCount(resourceType);
        }

        public TurnModel GetTurnModel()
        {
            return Turn;
        }

        public ITurnModelReader GetTurnModelReader()
        {
            return Turn;
        }
    }
}