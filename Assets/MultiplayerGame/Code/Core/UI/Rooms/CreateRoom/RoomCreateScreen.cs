using System;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Rooms.CreateRoom
{
    public class RoomCreateScreen : FadeBaseWindow, IFactoryEntity
    {
        public event Action<string, int> OnRoomCreated;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private TMP_InputField _roomNameInputField;
        [SerializeField] private TextMeshProUGUI _createRoomErrorMessage;
        [SerializeField] private int _minRoomNameCharacters;
        private ISoundService _soundService;
        private MapSelectPanel _mapSelectPanel;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(Hide);
            _createRoomButton.onClick.AddListener(ValidateRoomName);
        }

        public void Construct(ISoundService soundService, MapSelectPanel mapSelectPanel)
        {
            _soundService = soundService;
            _mapSelectPanel = mapSelectPanel;
        }

        public override void Show()
        {
            _createRoomErrorMessage.gameObject.SetActive(false);
            base.Show();
        }

        private void ValidateRoomName()
        {
            if (_roomNameInputField.text.Length < _minRoomNameCharacters)
                ShowRoomErrorMessage("Invalid room name");
            else if (_mapSelectPanel.SelectedMapId == -1)
                ShowRoomErrorMessage("Please, select the map");
            else
                OnRoomCreated?.Invoke(_roomNameInputField.text, _mapSelectPanel.SelectedMapId);
        }

        private void ShowRoomErrorMessage(string message)
        {
            _createRoomErrorMessage.text = message;
            _createRoomErrorMessage.gameObject.SetActive(true);
        }
    }
}