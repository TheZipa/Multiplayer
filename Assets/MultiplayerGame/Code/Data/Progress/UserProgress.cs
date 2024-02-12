using System;

namespace MultiplayerGame.Code.Data.Progress
{
    [Serializable]
    public class UserProgress : IPropertyChanged
    {
        public event Action OnPropertyChanged;
        
        public SettingsData Settings { get; set; }

        public int Balance 
        { 
            get => _balance;
            set { _balance = value; OnPropertyChanged?.Invoke(); }
        }

        public string Nickname
        { 
            get => _nickname;
            set { _nickname = value; OnPropertyChanged?.Invoke(); }
        }

        private string _nickname;
        private int _balance;

        public UserProgress(int balance, float defaultSoundVolume)
        {
            Balance = balance;
            Settings = new SettingsData(defaultSoundVolume);
        }
        
        public void Prepare() => Settings.OnPropertyChanged += SendPropertyChanged;

        private void SendPropertyChanged() => OnPropertyChanged?.Invoke();
    }
}