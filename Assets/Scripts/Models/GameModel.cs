using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Data;
using Managers;
using UnityEngine;

namespace Models
{   
    public class GameConfig
    {
        public MusicCardData[] musicCardDatas;
        public PlayerConfig[] playerConfigs;

        public GameConfig(MusicCardData[] musicCardDatas, PlayerConfig[] playerConfigs)
        {
            this.musicCardDatas = musicCardDatas;
            this.playerConfigs = playerConfigs;
        }
    }

    public class PlayerConfig
    {
        public string PlayerId;
        public string PlayerName;

        public PlayerConfig(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }

    public interface IGameModelReader
    {
        IMusicCardDataReader[,] GetCurrentBoardCards();
        IBoardSlotReader GetBoardSlot(int level, int position);
    }

    public class GameModel : IGameModelReader
    {
        public string GameId { get; private set; }
        public string GameName { get; private set; }
        public int PlayerCount { get; private set; }


        
        private List<PlayerModel> players { get; }
        public string CurrentPlayerId { get; private set; }
        public BoardModel board { get; private set; }

        public GameModel()
        {
            //GameId = Guid.NewGuid().ToString();
            //GameName = gameName;
            board = new BoardModel();
            players = new List<PlayerModel>();
        }

        private bool isInitialized = false;

        public void Initialize(GameConfig gameConfig)
        {
            foreach (var playerConfig in gameConfig.playerConfigs)
            {
                var player = new PlayerModel(playerConfig);
                AddPlayer(player);
            }

            isInitialized = true;
        }

        public void AddPlayer(PlayerModel player)
        {
            if (player == null || players.Contains(player))
            {
                return;
            }

            players.Add(player);

        }

        public PlayerModel GetPlayer(string playerId)
        {
            return players.FirstOrDefault(p => p.PlayerId == playerId);
        }

        public bool IsPlayerTurn(string playerId)
        {
            return CurrentPlayerId == playerId;
        }

        public bool IsPlayerExists(string playerId)
        {
            return players.Any(p => p.PlayerId == playerId);
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
            InitializeBoard();
            return true;
        }

        // Board Management
        public bool InitializeBoard()
        {
            board.Initialize();

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
            player.RemoveTokens();

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
            board.RemoveCardFromBoard(cardId);
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
            return board.GetCurrentBoardCards();
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
            return board.GetLevel(level).GetSlot(position);
        }
    }
}