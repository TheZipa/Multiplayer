using UnityEngine;

namespace MultiplayerGame.Code.Services.Quality
{
    public interface IQualityService
    {
        void ApplySavedSettings();
        Resolution[] Resolutions { get; }
        void SetResolution(int resolutionIndex, bool isFullscreen);
        void SetQuality(int qualityIndex);
        void SetFullscreen(bool isFullscreen);
    }
}