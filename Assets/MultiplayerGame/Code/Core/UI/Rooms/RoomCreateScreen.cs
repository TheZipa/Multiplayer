using System;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms
{
    public class RoomCreateScreen : FadeBaseWindow, IFactoryEntity
    {
        public event Action<string> OnRoomCreated;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private TMP_InputField _roomNameInputField;
        [SerializeField] private GameObject _invalidRoomNameHint;
        [SerializeField] private int _minRoomNameCharacters;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(Hide);
            _createRoomButton.onClick.AddListener(ValidateRoomName);
        }

        private void ValidateRoomName()
        {
            if(_roomNameInputField.text.Length < _minRoomNameCharacters)
                _invalidRoomNameHint.SetActive(true);
            else
                OnRoomCreated?.Invoke(_roomNameInputField.text);
        }
    }
}