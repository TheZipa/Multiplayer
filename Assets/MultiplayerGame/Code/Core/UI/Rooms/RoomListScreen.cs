using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Data;
using MultiplayerGame.Code.Data.StaticData;
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
        public event Action<RoomInfo> OnRoomConnect;
        public event Action OnRoomCreateClick;
        public Transform RoomsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        private IMultiplayerService _multiplayerService;
        private IObjectPool<RoomConnectField> _roomFieldsPool;
        private MapData[] _mapData;
        private Coroutine _refreshRoomListRoutine;

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

        public void Construct(IMultiplayerService multiplayerService, IObjectPool<RoomConnectField> roomFieldsPool, MapData[] mapData)
        {
            _mapData = mapData;
            _roomFieldsPool = roomFieldsPool;
            _multiplayerService = multiplayerService;
            _multiplayerService.OnRoomsUpdated += RefreshRoomList;
        }

        public void Clear()
        {
            foreach (RoomConnectField room in _rooms.Values)
            {
                _roomFieldsPool.Release(room);
                room.OnRoomConnectPressed -= SendRoomConnect;
            }
            _rooms.Clear();
        }
        
        public void StartRoomRefreshing() => _refreshRoomListRoutine = StartCoroutine(RefreshRoomList());

        public void StopRoomRefreshing() => StopCoroutine(_refreshRoomListRoutine);

        private void OnEnable() => _multiplayerService.LoadRoomList();

        private void RefreshRoomList(List<RoomInfo> roomInfos)
        {
            foreach (string roomName in _rooms.Keys.ToArray())
            {
                if(roomInfos.All(r => r.Name != roomName)) RemoveRoomFromList(roomName);
            }
            
            foreach (RoomInfo roomInfo in roomInfos)
            {
                if (_rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField))
                    roomField.UpdateRoomData(roomInfo, GetMapDataFromRoom(roomInfo));
                else
                    AddRoomToList(roomInfo);
            }
        }

        private void AddRoomToList(RoomInfo roomInfo)
        {
            RoomConnectField roomConnectField = _rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField) 
                ? roomField : _roomFieldsPool.Get();
            roomConnectField.UpdateRoomData(roomInfo, GetMapDataFromRoom(roomInfo));
            roomConnectField.OnRoomConnectPressed += SendRoomConnect;
            _rooms.Add(roomInfo.Name, roomConnectField);
        }

        private void RemoveRoomFromList(string roomName)
        {
            if (!_rooms.TryGetValue(roomName, out RoomConnectField roomConnectField)) return;
            roomConnectField.OnRoomConnectPressed -= SendRoomConnect;
            _roomFieldsPool.Release(roomConnectField);
            _rooms.Remove(roomName);
        }

        private void SendRoomConnect(RoomInfo roomInfo) => OnRoomConnect?.Invoke(roomInfo);

        private MapData GetMapDataFromRoom(RoomInfo roomInfo) => _mapData[(int)roomInfo.CustomProperties[RoomCustomDataKeys.MapId]];

        private IEnumerator RefreshRoomList()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                _multiplayerService.LoadRoomList();
            }
        }

        private void OnDestroy()
        {
            _multiplayerService.OnRoomsUpdated -= RefreshRoomList;
            Clear();
        }
    }
}