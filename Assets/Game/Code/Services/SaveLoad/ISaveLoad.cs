using Game.Code.Data.Progress;

namespace Game.Code.Services.SaveLoad
{
    public interface ISaveLoad
    {
        UserProgress Progress { get; }
        void Load(int startBalance, float defaultSoundVolume);
    }
}