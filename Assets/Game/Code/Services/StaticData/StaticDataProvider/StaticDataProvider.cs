using Game.Code.Data.StaticData;
using Game.Code.Data.StaticData.Sounds;
using UnityEngine;

namespace Game.Code.Services.StaticData.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string SoundDataPath = "StaticData/SoundData";
        private const string GameConfigurationPath = "StaticData/GameConfiguration";
        
        public SoundData LoadSoundData() => Resources.Load<SoundData>(SoundDataPath);

        public GameConfiguration LoadGameConfiguration() => Resources.Load<GameConfiguration>(GameConfigurationPath);
    }
}