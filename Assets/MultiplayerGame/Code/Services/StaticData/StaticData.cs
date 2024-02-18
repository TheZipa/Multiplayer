using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Data.StaticData.Sounds;
using MultiplayerGame.Code.Services.StaticData.StaticDataProvider;

namespace MultiplayerGame.Code.Services.StaticData
{
    public class StaticData : IStaticData
    {
        public SoundData SoundData { get; private set; }
        public GameConfiguration GameConfiguration { get; private set; }
        public LocationData LocationData { get; private set; }

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
            LocationData = _staticDataProvider.LoadLocationData();
        }
    }
}