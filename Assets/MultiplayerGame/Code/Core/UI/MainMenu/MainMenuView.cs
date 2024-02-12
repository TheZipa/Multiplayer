using System;
using MultiplayerGame.Code.Services.EntityContainer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour, IFactoryEntity
    {
        public event Action OnPlayClick;

        [SerializeField] private TMP_InputField _nicknameInputField;
        [SerializeField] private GameObject _wrongNameHint;
        [SerializeField] private Button _playButton;

        private void Awake()
        {
            _playButton.onClick.AddListener(() => OnPlayClick?.Invoke());
            _nicknameInputField.onSelect.AddListener(_ => _wrongNameHint.SetActive(false));
            _wrongNameHint.SetActive(false);
        }

        public void SetSavedNickname(string nickname) => _nicknameInputField.text = nickname;

        public bool ValidatePlayer(out string nickname)
        {
            if (string.IsNullOrEmpty(_nicknameInputField.text))
            {
                nickname = String.Empty;
                return false;
            }
            _wrongNameHint.SetActive(true);
            nickname = _nicknameInputField.text;
            return true;
        }
    }
}