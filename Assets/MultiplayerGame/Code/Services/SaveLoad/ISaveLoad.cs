using MultiplayerGame.Code.Data.Progress;

namespace MultiplayerGame.Code.Services.SaveLoad
{
    public interface ISaveLoad
    {
        UserProgress Progress { get; }
        void Load(int startBalance, float defaultSoundVolume);
    }
}