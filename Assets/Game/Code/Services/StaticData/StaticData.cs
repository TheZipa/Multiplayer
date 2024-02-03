using Game.Code.Data.StaticData;
using Game.Code.Data.StaticData.Sounds;
using Game.Code.Services.StaticData.StaticDataProvider;

namespace Game.Code.Services.StaticData
{
    public class StaticData : IStaticData
    {
        public SoundData SoundData { get; private set; }
        public GameConfiguration GameConfiguration { get; private set; }

        private readonly IStaticDataProvider _staticDataProvider;

        public StaticData(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
            LoadStaticData();
        }

        public void LoadStaticData()
        {
            SoundData = _staticDataProvider.LoadSoundData();
            GameConfiguration = _staticDataProvider.LoadGameConfiguration();
        }
    }
}