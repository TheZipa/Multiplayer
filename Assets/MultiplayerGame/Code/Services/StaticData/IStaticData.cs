using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Data.StaticData.Sounds;

namespace MultiplayerGame.Code.Services.StaticData
{
    public interface IStaticData
    {
        SoundData SoundData { get; }
        GameConfiguration GameConfiguration { get; }
        WorldData WorldData { get; }
        void LoadStaticData();
    }
}