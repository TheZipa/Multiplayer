using UnityEngine;

namespace Game.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "StaticData/GameConfiguration")]
    public class GameConfiguration : ScriptableObject
    {
        public int StartBalance;
        public float DefaultSoundVolume;
    }
}