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
        public event Action OnClose;
        public Transform RoomsContent;
        [SerializeField] private Button _closeButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        private IObjectPool<RoomConnectField> _roomFieldsPool;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(() => OnClose?.Invoke());
        }

        public void Construct(IObjectPool<RoomConnectField> roomFieldsPool) => _roomFieldsPool = roomFieldsPool;

        public void RefreshRoomList(List<RoomInfo> roomInfos)
        {
            foreach (RoomInfo roomInfo in roomInfos)
            {
                if (roomInfo.RemovedFromList)
                {
                    _roomFieldsPool.Release(_rooms[roomInfo.Name]);
                    _rooms.Remove(roomInfo.Name);
                }
                else if(_rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField))
                {
                    roomField.UpdateRoomData(roomInfo);
                }
                else
                {
                    RoomConnectField roomConnectField = _roomFieldsPool.Get();
                    roomConnectField.UpdateRoomData(roomInfo);
                    _rooms.Add(roomInfo.Name, roomConnectField);
                }
            }
        }
    }
}