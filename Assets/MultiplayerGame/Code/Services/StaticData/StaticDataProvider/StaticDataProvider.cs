using MultiplayerGame.Code.Data.StaticData;
using MultiplayerGame.Code.Data.StaticData.Sounds;
using UnityEngine;

namespace MultiplayerGame.Code.Services.StaticData.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string SoundDataPath = "StaticData/SoundData";
        private const string GameConfigurationPath = "StaticData/GameConfiguration";
        private const string WorldDataPath = "StaticData/WorldData";
        
        public SoundData LoadSoundData() => Resources.Load<SoundData>(SoundDataPath);

        public GameConfiguration LoadGameConfiguration() => Resources.Load<GameConfiguration>(GameConfigurationPath);

        public WorldData LoadLocationData() => Resources.Load<WorldData>(WorldDataPath);
    }
}