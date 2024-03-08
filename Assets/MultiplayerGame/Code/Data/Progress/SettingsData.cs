using System;
using UnityEngine;

namespace MultiplayerGame.Code.Data.Progress
{
    [Serializable]
    public class SettingsData : IPropertyChanged
    {
        public event Action OnPropertyChanged;

        public float SoundVolume
        {
            get => _soundVolume;
            set { _soundVolume = value; OnPropertyChanged?.Invoke(); }
        }
        
        public int Resolution
        {
            get => _resolution;
            set { _resolution = value; OnPropertyChanged?.Invoke(); }
        }
        
        public int Quality
        {
            get => _quality;
            set { _quality = value; OnPropertyChanged?.Invoke(); }
        }
        
        public bool IsFullscreen
        {
            get => _isFullscreen;
            set { _isFullscreen = value; OnPropertyChanged?.Invoke(); }
        }
        
        private float _soundVolume;
        private int _resolution;
        private int _quality;
        private bool _isFullscreen;

        public SettingsData(float defaultSoundVolume)
        {
            SoundVolume = defaultSoundVolume;
            Resolution = Array.IndexOf(Screen.resolutions, Screen.currentResolution);
            Quality = QualitySettings.GetQualityLevel();
            IsFullscreen = Screen.fullScreen;
        }
    }
}