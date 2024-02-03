using System;
using MultiplayerGame.Code.Services.EntityContainer;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour, IFactoryEntity
    {
        public event Action OnPlayClick;

        [SerializeField] private Button _playButton;

        private void Awake()
        {
            _playButton.onClick.AddListener(() => OnPlayClick?.Invoke());
        }
    }
}