using System;
using Game.Code.Core.UI.Base;
using Game.Code.Core.UI.Settings;
using Game.Code.Services.EntityContainer;
using Game.Code.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Core.UI
{
    public class TopPanelView : BaseWindow, IFactoryEntity
    {
        [SerializeField] private SoundSlider _soundSlider;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backButton;
        private Action _backClickAction;

        private ISoundService _soundService;

        public void Construct(ISoundService soundService)
        {
            _soundService = soundService;
            _soundSlider.Construct(soundService.Volume);
            _soundSlider.OnVolumeChanged += _soundService.SetEffectsVolume;
        }

        public void SetBackAction(Action backClickAction) => _backClickAction = backClickAction;

        public void ToggleMainMenuStateView()
        {
            _backButton.gameObject.SetActive(false);
        }

        public void ToggleGameplayStateView()
        {
            _backButton.gameObject.SetActive(true);
        }

        private void Awake()
        {
            _settingsButton.onClick.AddListener(SwitchSoundSlider);
            _backButton.onClick.AddListener(() => _backClickAction?.Invoke());
        }

        private void SwitchSoundSlider()
        {
            if(_soundSlider.gameObject.activeSelf) _soundSlider.Hide();
            else _soundSlider.Show();
        }
    }
}