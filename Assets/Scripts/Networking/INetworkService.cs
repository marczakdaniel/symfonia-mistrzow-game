using System;

namespace SymfoniaMistrzow.Networking
{
    /// <summary>
    /// Interface for network operations, abstracting the specific implementation (e.g., Photon).
    /// </summary>
    public interface INetworkService : IDisposable
    {
        void Connect();
        void Disconnect();
        void JoinRoom(string roomName);
        void LeaveRoom();
        void SendData(byte eventCode, object data); // Example method
        // Add events for connection status, received data, etc.
    }
} 