using System;
using System.Collections.Generic;
using Photon.Realtime;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public interface IMultiplayerService
    {
        void Connect();
        event Action OnRoomJoined;
        event Action<List<RoomInfo>> OnRoomsUpdated;
        event Action OnConnectingSuccess;
        event Action<DisconnectCause> OnConnectionClosed;
        event Action<string> OnRoomJoinFailed;
        void JoinToRoom(string roomName);
        void CreateAndJoinRoom(string roomName, int maxPlayers);
    }
}