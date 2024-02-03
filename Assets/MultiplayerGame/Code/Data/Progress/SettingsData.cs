using System;

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
        
        private float _soundVolume;

        public SettingsData(float defaultSoundVolume) => SoundVolume = defaultSoundVolume;

    }
}