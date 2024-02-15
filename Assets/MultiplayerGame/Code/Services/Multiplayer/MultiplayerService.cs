using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public class MultiplayerService : MonoBehaviourPunCallbacks, IMultiplayerService
    {
        public event Action<EventData> OnEventReceived;
        public event Action OnRoomJoined;
        public event Action<string> OnRoomJoinFailed;
        public event Action OnConnectingSuccess;
        public event Action<Player> OnPlayerRoomJoin;
        public event Action<Player> OnPlayerRoomLeft;
        public event Action<DisconnectCause> OnConnectionClosed;
        public event Action<List<RoomInfo>> OnRoomsUpdated;

        private readonly RaiseEventOptions _eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        private void Awake() => PhotonNetwork.NetworkingClient.EventReceived += eventData => OnEventReceived?.Invoke(eventData);

        public void Connect()
        {
            Debug.Log("Start connection");
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinToRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

        public void CreateAndJoinRoom(string roomName, int maxPlayers) =>
            PhotonNetwork.CreateRoom(roomName, new RoomOptions(maxPlayers: maxPlayers), TypedLobby.Default);

        public Player[] GetPlayersInRoom() => PhotonNetwork.CurrentRoom.Players.Values.ToArray();

        public bool IsMasterPlayer() => PhotonNetwork.IsMasterClient;

        public void SendEvent(byte eventCode) => PhotonNetwork.RaiseEvent(eventCode, null, _eventOptions, SendOptions.SendReliable);

        public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

        public override void OnJoinedLobby()
        {
            Debug.Log("Connected to lobby");
            OnConnectingSuccess?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause) => OnConnectionClosed?.Invoke(cause);

        public override void OnJoinedRoom()
        {
            Debug.Log("Connected to room");
            OnRoomJoined?.Invoke();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) => OnPlayerRoomJoin?.Invoke(newPlayer);

        public override void OnPlayerLeftRoom(Player otherPlayer) => OnPlayerRoomLeft?.Invoke(otherPlayer);

        public override void OnJoinRoomFailed(short returnCode, string message) => OnRoomJoinFailed?.Invoke(message);

        public override void OnRoomListUpdate(List<RoomInfo> roomList) => OnRoomsUpdated?.Invoke(roomList);
    }
}