using MultiplayerGame.Code.Services.SaveLoad;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Quality
{
    public class QualityService : IQualityService
    {
        public Resolution[] Resolutions => Screen.resolutions;
        private readonly ISaveLoad _saveLoad;

        public QualityService(ISaveLoad saveLoad)
        {
            _saveLoad = saveLoad;
        }

        public void SetResolution(int resolutionIndex, bool isFullscreen)
        {
            _saveLoad.Progress.Settings.Resolution = resolutionIndex;
            Resolution resolution = Resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        }

        public void SetQuality(int qualityIndex)
        {
            _saveLoad.Progress.Settings.Quality = qualityIndex;
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen) =>
            Screen.fullScreen = _saveLoad.Progress.Settings.IsFullscreen = isFullscreen;

        public void ApplySavedSettings()
        {
            QualitySettings.SetQualityLevel(_saveLoad.Progress.Settings.Quality);
            Resolution resolution = Screen.resolutions[_saveLoad.Progress.Settings.Resolution];
            Screen.SetResolution(resolution.width, resolution.height, _saveLoad.Progress.Settings.IsFullscreen);
        }
    }
}