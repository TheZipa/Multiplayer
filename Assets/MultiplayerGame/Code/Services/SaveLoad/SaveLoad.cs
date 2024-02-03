using MultiplayerGame.Code.Data.Progress;
using MultiplayerGame.Code.Extensions;
using UnityEngine;

namespace MultiplayerGame.Code.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        public UserProgress Progress { get; private set; }
        private const string ProgressKey = "Progress";

        public void Load(int startBalance, float defaultSoundVolume)
        {
            string progressJson = PlayerPrefs.GetString(ProgressKey);
            Progress = progressJson.ToDeserialized<UserProgress>() ?? new UserProgress(startBalance, defaultSoundVolume);
            Progress.Prepare();
            Progress.OnPropertyChanged += SaveProgress;
        }

        private void SaveProgress() => PlayerPrefs.SetString(ProgressKey, Progress.ToJson());
    }
}