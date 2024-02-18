using System;
using System.Collections.Generic;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomScreen : FadeBaseWindow, IFactoryEntity
    {
        public event Action OnRoomLeft;
        public event Action OnStartGame;
        public Transform PlayerFieldContent;
        [SerializeField] private Button _leaveRoomButton;
        [SerializeField] private Button _startGameButton;

        private IMultiplayerService _multiplayerService;
        private Dictionary<string, RoomPlayerField> _roomPlayerFields;
        private Stack<RoomPlayerField> _fields;
        private int _minPlayers;

        protected override void OnAwake()
        {
            base.OnAwake();
            _startGameButton.onClick.AddListener(TryStartGame);
            _leaveRoomButton.onClick.AddListener(() =>
            {
                OnRoomLeft?.Invoke();
                ClearPlayersList();
                Hide();
            });
        }

        public void Construct(IMultiplayerService multiplayerService, Stack<RoomPlayerField> roomPlayerFields, 
            int maxPlayers, int minPlayers)
        {
            _roomPlayerFields = new Dictionary<string, RoomPlayerField>(maxPlayers);
            _fields = roomPlayerFields;
            _multiplayerService = multiplayerService;
            _multiplayerService.OnPlayerRoomJoin += AddPlayerToRoom;
            _multiplayerService.OnPlayerRoomLeft += RemovePlayerFromRoom;
            _minPlayers = minPlayers;
        }

        public void SetupRoom()
        {
            foreach (Photon.Realtime.Player player in _multiplayerService.GetPlayersInRoom()) AddPlayerToRoom(player);
            _startGameButton.gameObject.SetActive(_multiplayerService.IsMasterPlayer());
        }

        private void TryStartGame()
        {
            if (_multiplayerService.GetPlayersInRoom().Length < _minPlayers) return;
            OnStartGame?.Invoke();
        }

        private void AddPlayerToRoom(Photon.Realtime.Player player)
        {
            RoomPlayerField roomPlayerField = _fields.Pop();
            roomPlayerField.ShowAndSetup(player.NickName, player.IsMasterClient);
            _roomPlayerFields.Add(player.NickName, roomPlayerField);
        }

        private void RemovePlayerFromRoom(Photon.Realtime.Player player)
        {
            RoomPlayerField roomPlayerField = _roomPlayerFields[player.NickName];
            roomPlayerField.Hide();
            _fields.Push(roomPlayerField);
            _roomPlayerFields.Remove(player.NickName);
        }

        private void ClearPlayersList()
        {
            foreach (RoomPlayerField playerField in _roomPlayerFields.Values)
            {
                playerField.Hide();
                _fields.Push(playerField);
            }
            _roomPlayerFields.Clear();
        }

        private void OnDestroy()
        {
            _multiplayerService.OnPlayerRoomJoin -= AddPlayerToRoom;
            _multiplayerService.OnPlayerRoomLeft -= RemovePlayerFromRoom;
        }
    }
}