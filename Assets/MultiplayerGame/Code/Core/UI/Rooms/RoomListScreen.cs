using System;
using System.Collections.Generic;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomListScreen : FadeBaseWindow, IFactoryEntity
    {
        public event Action<string> OnRoomConnect;
        public event Action OnRoomCreateClick;
        public Transform RoomsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        private IObjectPool<RoomConnectField> _roomFieldsPool;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(Hide);
            _createRoomButton.onClick.AddListener(() => OnRoomCreateClick?.Invoke());
        }

        public void Construct(IObjectPool<RoomConnectField> roomFieldsPool) => _roomFieldsPool = roomFieldsPool;

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

        private void AddRoomToList(RoomInfo roomInfo)
        {
            RoomConnectField roomConnectField = _roomFieldsPool.Get();
            roomConnectField.UpdateRoomData(roomInfo);
            roomConnectField.OnRoomConnectPressed += SendRoomConnect;
            _rooms.Add(roomInfo.Name, roomConnectField);
        }

        private void RemoveRoomFromList(RoomInfo roomInfo)
        {
            RoomConnectField roomConnectField = _rooms[roomInfo.Name];
            roomConnectField.OnRoomConnectPressed -= SendRoomConnect;
            _roomFieldsPool.Release(roomConnectField);
            _rooms.Remove(roomInfo.Name);
        }

        private void SendRoomConnect(string roomName) => OnRoomConnect?.Invoke(roomName);

        private void OnDestroy()
        {
            foreach (RoomConnectField room in _rooms.Values)
            {
                _roomFieldsPool.Release(room);
                room.OnRoomConnectPressed -= SendRoomConnect;
            }
            _rooms.Clear();
            _roomFieldsPool.Clear();
        }
    }
}