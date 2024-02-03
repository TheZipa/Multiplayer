using Game.Code.Data.StaticData;
using Game.Code.Data.StaticData.Sounds;

namespace Game.Code.Services.StaticData.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        SoundData LoadSoundData();
        GameConfiguration LoadGameConfiguration();
    }
}