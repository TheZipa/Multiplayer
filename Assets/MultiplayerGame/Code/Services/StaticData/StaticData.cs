using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Services.StaticData.StaticDataProvider;

namespace MultiplayerGame.Code.Services.StaticData
{
    public class StaticData : IStaticData
    {
        public GameConfiguration GameConfiguration { get; private set; }
        public WorldData WorldData { get; private set; }

        private readonly IStaticDataProvider _staticDataProvider;

        public StaticData(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
            LoadStaticData();
        }

        private void LoadStaticData()
        {
            GameConfiguration = _staticDataProvider.LoadGameConfiguration();
            WorldData = _staticDataProvider.LoadLocationData();
        }
    }
}