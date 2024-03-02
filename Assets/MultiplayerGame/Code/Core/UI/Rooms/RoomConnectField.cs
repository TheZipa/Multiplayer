using System;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Data.StaticData;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomConnectField : BaseWindow
    {
        public event Action<RoomInfo> OnRoomConnectPressed;
        
        [SerializeField] private TextMeshProUGUI _roomName;
        [SerializeField] private TextMeshProUGUI _roomPlayersCount;
        [SerializeField] private Image _mapPreview;
        [SerializeField] private Button _connectButton;
        private RoomInfo _roomInfo;

        private void Awake() => _connectButton.onClick.AddListener(() => OnRoomConnectPressed?.Invoke(_roomInfo));

        public void UpdateRoomData(RoomInfo roomInfo, MapData mapData)
        {
            bool isRoomFulled = roomInfo.PlayerCount == roomInfo.MaxPlayers;
            _roomInfo = roomInfo;
            _roomName.text = roomInfo.Name;
            _roomPlayersCount.color = isRoomFulled ? Color.red : Color.green;
            _roomPlayersCount.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            _connectButton.interactable = !isRoomFulled;
            _mapPreview.sprite = mapData.MapPreview;
        }
    }
}