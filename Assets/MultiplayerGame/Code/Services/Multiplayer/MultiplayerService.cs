using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using MultiplayerGame.Code.Data;
using MultiplayerGame.Code.Extensions;
using Newtonsoft.Json;
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
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinToRoom(string roomName) => PhotonNetwork.JoinRoom(roomName);

        public void CreateAndJoinRoom(string roomName, int mapId, int maxPlayers, bool isVisible)
        {
            RoomOptions roomOptions = new()
            {
                IsVisible = isVisible,
                MaxPlayers = maxPlayers,
                CustomRoomProperties = new Hashtable() { [RoomCustomDataKeys.MapId] = mapId}
            };
            Debug.Log(JsonConvert.SerializeObject(roomOptions.CustomRoomProperties));
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public Player[] GetPlayersInRoom() => PhotonNetwork.CurrentRoom.Players.Values.ToArray();

        public bool IsMasterPlayer() => PhotonNetwork.IsMasterClient;

        public int GetCurrentPlayerId() => PhotonNetwork.LocalPlayer.ActorNumber;

        public void SendEvent(byte eventCode) => PhotonNetwork.RaiseEvent(eventCode, null, _eventOptions, SendOptions.SendReliable);

        public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(TypedLobby.Default);

        public override void OnJoinedLobby() => OnConnectingSuccess?.Invoke();

        public override void OnDisconnected(DisconnectCause cause) => OnConnectionClosed?.Invoke(cause);

        public override void OnJoinedRoom() => OnRoomJoined?.Invoke();

        public override void OnPlayerEnteredRoom(Player newPlayer) => OnPlayerRoomJoin?.Invoke(newPlayer);

        public override void OnPlayerLeftRoom(Player otherPlayer) => OnPlayerRoomLeft?.Invoke(otherPlayer);

        public override void OnJoinRoomFailed(short returnCode, string message) => OnRoomJoinFailed?.Invoke(message);

        public override void OnRoomListUpdate(List<RoomInfo> roomList) => OnRoomsUpdated?.Invoke(roomList);
    }
}