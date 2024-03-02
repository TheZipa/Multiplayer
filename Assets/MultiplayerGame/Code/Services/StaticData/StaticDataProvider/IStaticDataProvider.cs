using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Data.StaticData.Sounds;

namespace MultiplayerGame.Code.Services.StaticData.StaticDataProvider
{
    public interface IStaticDataProvider
    {
        SoundData LoadSoundData();
        GameConfiguration LoadGameConfiguration();
        WorldData LoadLocationData();
    }
}