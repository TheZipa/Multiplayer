using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
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
        event Action<EventData> OnEventReceived;
        event Action<Player> OnPlayerRoomJoin;
        event Action<Player> OnPlayerRoomLeft;
        void JoinToRoom(string roomName);
        void CreateAndJoinRoom(string roomName, int mapId, int maxPlayers, bool isVisible);
        void SetNickname(string nickname);
        void LoadRoomList();
        Player[] GetPlayersInRoom();
        bool IsMasterPlayer();
        void SendEvent(byte eventCode);
        int GetCurrentPlayerId();
        void LeaveRoom();
    }
}