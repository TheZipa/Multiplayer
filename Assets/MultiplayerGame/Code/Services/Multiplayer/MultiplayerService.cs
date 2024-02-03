using MultiplayerGame.Code.Services.Factories.GameFactory;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Multiplayer
{
    public class MultiplayerService : MonoBehaviourPunCallbacks, IMultiplayerService
    {
        private IGameFactory _gameFactory;
        
        public void Construct(IGameFactory gameFactory) => _gameFactory = gameFactory;

        public void Connect()
        {
            Debug.Log("Start connection");
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions(maxPlayers: 4), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Connect finished");
        }
    }
}