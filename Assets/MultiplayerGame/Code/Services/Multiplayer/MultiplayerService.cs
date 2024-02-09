using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public class MultiplayerService : MonoBehaviourPunCallbacks, IMultiplayerService
    {
        public event Action OnRoomJoined;

        public void Connect()
        {
            Debug.Log("Start connection");
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions(maxPlayers: 8), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Connect finished");
            OnRoomJoined?.Invoke();
        }
    }
}