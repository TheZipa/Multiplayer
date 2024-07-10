using System;
using System.Collections.Generic;
using System.Linq;
using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Quality;
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
        [SerializeField] private Button _closeButton;

        private IQualityService _qualityService;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(Hide);
            _isFullscreenToggle.onValueChanged.AddListener(_qualityService.SetFullscreen);
            _resolutionDropdown.onValueChanged.AddListener(resolutionIndex => 
                _qualityService.SetResolution(resolutionIndex, _isFullscreenToggle.isOn));
            _qualityDropdown.onValueChanged.AddListener(_qualityService.SetQuality);
        }

        public void Construct(IQualityService qualityService)
        {
            _qualityService = qualityService;
            _isFullscreenToggle.isOn = Screen.fullScreen;
            ConstructResolutions();
            ConstructQualities();
        }

        private void ConstructResolutions()
        {
            _resolutionDropdown.ClearOptions();
            List<string> resolutionOptions = new List<string>(15);
            foreach (Resolution resolution in _qualityService.Resolutions)
                resolutionOptions.Add($"{resolution.width}x{resolution.height} {resolution.refreshRateRatio} Hz");
            _resolutionDropdown.AddOptions(resolutionOptions);
            _resolutionDropdown.SetValueWithoutNotify(Array.IndexOf(_qualityService.Resolutions, Screen.currentResolution));
            _resolutionDropdown.RefreshShownValue();
        }

        private void ConstructQualities()
        {
            _qualityDropdown.ClearOptions();
            _qualityDropdown.AddOptions(QualitySettings.names.ToList());
            _qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
            _qualityDropdown.RefreshShownValue();
        }
    }
}