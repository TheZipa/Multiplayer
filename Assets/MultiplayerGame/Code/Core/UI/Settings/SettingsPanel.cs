using System;
using System.Collections.Generic;
using System.Linq;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Settings
{
    public class SettingsPanel : FadeBaseWindow, IFactoryEntity
    {
        [SerializeField] private Toggle _isFullscreenToggle;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _qualityDropdown;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _closeButton;

        private ISaveLoad _saveLoad;
        private Resolution[] _allResolutions;

        protected override void OnAwake()
        {
            base.OnAwake();
            _saveButton.onClick.AddListener(SaveSettings);
            _closeButton.onClick.AddListener(Hide);
        }

        public void Construct(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
            _allResolutions = Screen.resolutions;
            _isFullscreenToggle.isOn = Screen.fullScreen;
            ConstructResolutions();
            ConstructQualities();
        }

        private void ConstructResolutions()
        {
            _resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>(15);
            foreach (Resolution resolution in _allResolutions)
                resolutionOptions.Add($"{resolution.width}x{resolution.height} {resolution.refreshRate} Hz");
            _resolutionDropdown.AddOptions(resolutionOptions);
            _resolutionDropdown.SetValueWithoutNotify(Array.IndexOf(_allResolutions, Screen.currentResolution));
            _resolutionDropdown.RefreshShownValue();
        }

        private void ConstructQualities()
        {
            _qualityDropdown.ClearOptions();
            _qualityDropdown.AddOptions(QualitySettings.names.ToList());
            _qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
            _qualityDropdown.RefreshShownValue();
        }
        
        private void SaveSettings()
        {
            _saveLoad.Progress.Settings.IsFullscreen = _isFullscreenToggle.isOn;
            _saveLoad.Progress.Settings.Resolution = _resolutionDropdown.value;
            _saveLoad.Progress.Settings.Quality = _qualityDropdown.value;
            QualitySettings.SetQualityLevel(_qualityDropdown.value);
            Resolution resolution = _allResolutions[_resolutionDropdown.value];
            Screen.SetResolution(resolution.width, resolution.height, _isFullscreenToggle.isOn);
        }
    }
}