using MultiplayerGame.Code.Data.StaticData;
using UnityEngine;

namespace MultiplayerGame.Code.Services.StaticData.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string GameConfigurationPath = "StaticData/GameConfiguration";
        private const string WorldDataPath = "StaticData/WorldData";

        public GameConfiguration LoadGameConfiguration() => Resources.Load<GameConfiguration>(GameConfigurationPath);

        public WorldData LoadLocationData() => Resources.Load<WorldData>(WorldDataPath);
    }
}