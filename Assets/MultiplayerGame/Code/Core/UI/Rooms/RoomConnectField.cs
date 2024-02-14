using System;
using MultiplayerGame.Code.Core.UI.Base;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomConnectField : BaseWindow
    {
        public event Action<string> OnRoomConnectPressed;
        
        [SerializeField] private TextMeshProUGUI _roomName;
        [SerializeField] private TextMeshProUGUI _roomPlayersCount;
        [SerializeField] private Button _connectButton;

        private void Awake() => _connectButton.onClick.AddListener(() => OnRoomConnectPressed?.Invoke(_roomName.text));

        public void UpdateRoomData(RoomInfo roomInfo) => 
            UpdateRoomData(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers);

        public void UpdateRoomData(string roomName, int playersInRoom, int maxPlayers)
        {
            _roomName.text = roomName;
            _roomPlayersCount.color = playersInRoom == maxPlayers ? Color.red : Color.green;
            _roomPlayersCount.text = $"{playersInRoom}/{maxPlayers}";
        }
    }
}