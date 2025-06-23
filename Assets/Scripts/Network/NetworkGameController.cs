using UnityEngine;
using SplendorGame.Game;
using SplendorGame.Game.Models;
using SplendorGame.Game.Data;

namespace SplendorGame.Network
{
    /// <summary>
    /// Network-enabled game controller for multiplayer games using Photon
    /// This is prepared for future Photon Realtime integration
    /// </summary>
    public class NetworkGameController : GameController
    {
        [Header("Network Settings")]
        [SerializeField] private bool isHost = false;
        [SerializeField] private string roomName = "SplendorRoom";
        [SerializeField] private int maxPlayers = 4;
        
        // Network events and state management
        private bool _isConnectedToRoom = false;
        private bool _isGameStarted = false;
        
        #region Network Events Placeholder
        // These will be implemented when adding Photon Realtime
        
        public void ConnectToRoom()
        {
            // TODO: Implement Photon room connection
            Debug.Log($"Connecting to room: {roomName}");
            
            // For now, simulate connection
            SimulateConnection();
        }
        
        public void CreateRoom()
        {
            // TODO: Implement Photon room creation
            Debug.Log($"Creating room: {roomName} with max players: {maxPlayers}");
            isHost = true;
            
            // For now, simulate room creation
            SimulateConnection();
        }
        
        public void LeaveRoom()
        {
            // TODO: Implement leaving Photon room
            Debug.Log("Leaving room");
            _isConnectedToRoom = false;
            _isGameStarted = false;
        }
        
        private void SimulateConnection()
        {
            _isConnectedToRoom = true;
            Debug.Log("Connected to room (simulated)");
        }
        
        #endregion
        
        #region Network Game Actions
        // These methods will send network messages in the future
        
        public void NetworkTakeTokens(ResourceType[] tokens)
        {
            if (!_isConnectedToRoom) return;
            
            // TODO: Send network message for token taking
            Debug.Log($"Network: Taking tokens {string.Join(", ", tokens)}");
            
            // For now, execute locally
            // GetGameModel().TakeTokens(...); // Will need to adapt based on tokens array
        }
        
        public void NetworkBuyCard(CardData card)
        {
            if (!_isConnectedToRoom) return;
            
            // TODO: Send network message for card purchase
            Debug.Log($"Network: Buying card {card.id}");
            
            // For now, execute locally
            GetGameModel().BuyCard(card);
        }
        
        public void NetworkReserveCard(CardData card)
        {
            if (!_isConnectedToRoom) return;
            
            // TODO: Send network message for card reservation
            Debug.Log($"Network: Reserving card {card.id}");
            
            // For now, execute locally
            GetGameModel().ReserveCard(card);
        }
        
        #endregion
        
        #region Network Message Handlers
        // These will handle incoming network messages
        
        private void OnPlayerJoined(string playerName)
        {
            Debug.Log($"Player joined: {playerName}");
            AddPlayer(playerName);
        }
        
        private void OnPlayerLeft(string playerName)
        {
            Debug.Log($"Player left: {playerName}");
            // TODO: Handle player leaving
        }
        
        private void OnGameStarted()
        {
            Debug.Log("Network game started");
            _isGameStarted = true;
        }
        
        private void OnTurnChanged(int newPlayerIndex)
        {
            Debug.Log($"Turn changed to player {newPlayerIndex}");
            // Game model will handle this automatically
        }
        
        #endregion
        
        #region Public Network Interface
        
        public bool IsConnectedToRoom => _isConnectedToRoom;
        public bool IsHost => isHost;
        public bool IsGameStarted => _isGameStarted;
        public string RoomName => roomName;
        
        public void SetRoomName(string newRoomName)
        {
            roomName = newRoomName;
        }
        
        public void SetMaxPlayers(int newMaxPlayers)
        {
            maxPlayers = Mathf.Clamp(newMaxPlayers, 2, 4);
        }
        
        #endregion
        
        #if UNITY_EDITOR
        [ContextMenu("Test Connect to Room")]
        private void TestConnectToRoom()
        {
            ConnectToRoom();
        }
        
        [ContextMenu("Test Create Room")]
        private void TestCreateRoom()
        {
            CreateRoom();
        }
        
        [ContextMenu("Test Leave Room")]
        private void TestLeaveRoom()
        {
            LeaveRoom();
        }
        #endif
    }
} 