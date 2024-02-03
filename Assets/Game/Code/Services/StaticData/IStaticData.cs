using Game.Code.Data.StaticData;
using Game.Code.Data.StaticData.Sounds;

namespace Game.Code.Services.StaticData
{
    public interface IStaticData
    {
        SoundData SoundData { get; }
        GameConfiguration GameConfiguration { get; }
        void LoadStaticData();
    }
}