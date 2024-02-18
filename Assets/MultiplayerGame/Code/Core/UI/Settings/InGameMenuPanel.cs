using System;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Extensions;
using MultiplayerGame.Code.Services.EntityContainer;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Settings
{
    public class InGameMenuPanel : FadeBaseWindow, IFactoryEntity
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnReturnToMainMenu;
        
        [SerializeField] private Button _returnToMainMenuButton;
        [SerializeField] private Button _yesReturnButton;
        [SerializeField] private Button _noReturnButton;
        [SerializeField] private GameObject _returnToMainMenuPopup;

        protected override void OnAwake()
        {
            base.OnAwake();
            _returnToMainMenuButton.onClick.AddListener(() => _returnToMainMenuPopup.SetActive(true));
            _yesReturnButton.onClick.AddListener(() => OnReturnToMainMenu?.Invoke());
            _noReturnButton.onClick.AddListener(() => _returnToMainMenuPopup.SetActive(false));
        }

        public override void Hide()
        {
            base.Hide();
            GameExtensions.DisableCursor();
            OnHide?.Invoke();
        }

        public override void Show()
        {
            base.Show();
            GameExtensions.EnableCursor();
            OnShow?.Invoke();
        }

        public void ToggleEnabled()
        {
            _returnToMainMenuPopup.gameObject.SetActive(false);
            if(gameObject.activeSelf) Hide();
            else Show();
        }
    }
}