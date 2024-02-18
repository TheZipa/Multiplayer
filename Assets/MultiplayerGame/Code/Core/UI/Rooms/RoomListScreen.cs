using System;
using System.Collections.Generic;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Multiplayer;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomListScreen : FadeBaseWindow, IFactoryEntity
    {
        public event Action OnRoomListClose;
        public event Action<string> OnRoomConnect;
        public event Action OnRoomCreateClick;
        public Transform RoomsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        private IMultiplayerService _multiplayerService;
        private IObjectPool<RoomConnectField> _roomFieldsPool;

        protected override void OnAwake()
        {
            base.OnAwake();
            _createRoomButton.onClick.AddListener(() => OnRoomCreateClick?.Invoke());
            _closeButton.onClick.AddListener(() =>
            {
                Hide();
                OnRoomListClose?.Invoke();
            });
        }

        public void Construct(IMultiplayerService multiplayerService, IObjectPool<RoomConnectField> roomFieldsPool)
        {
            _multiplayerService = multiplayerService;
            _multiplayerService.OnRoomsUpdated += RefreshRoomList;
            _roomFieldsPool = roomFieldsPool;
        }

        public void RefreshRoomList(List<RoomInfo> roomInfos)
        {
            foreach (RoomInfo roomInfo in roomInfos)
            {
                if (roomInfo.RemovedFromList) 
                    RemoveRoomFromList(roomInfo);
                else if (_rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField))
                    roomField.UpdateRoomData(roomInfo);
                else
                    AddRoomToList(roomInfo);
            }
        }

        public void Clear()
        {
            foreach (RoomConnectField room in _rooms.Values)
            {
                _roomFieldsPool.Release(room);
                room.OnRoomConnectPressed -= SendRoomConnect;
            }
            _rooms.Clear();
            _roomFieldsPool.Clear();
        }

        private void AddRoomToList(RoomInfo roomInfo)
        {
            RoomConnectField roomConnectField = _rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField) 
                ? roomField : _roomFieldsPool.Get();
            roomConnectField.UpdateRoomData(roomInfo);
            roomConnectField.OnRoomConnectPressed += SendRoomConnect;
            _rooms.Add(roomInfo.Name, roomConnectField);
        }

        private void RemoveRoomFromList(RoomInfo roomInfo)
        {
            if (!_rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomConnectField)) return;
            roomConnectField.OnRoomConnectPressed -= SendRoomConnect;
            _roomFieldsPool.Release(roomConnectField);
            _rooms.Remove(roomInfo.Name);
        }

        private void SendRoomConnect(string roomName) => OnRoomConnect?.Invoke(roomName);

        private void OnDestroy()
        {
            _multiplayerService.OnRoomsUpdated -= RefreshRoomList;
            Clear();
        }
    }
}