using UnityEngine;

namespace MultiplayerGame.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "WorldData", menuName = "StaticData/WorldData")]
    public class WorldData : ScriptableObject
    {
        public float PlayerSpawnRadius;
        public MapData[] Maps;
    }
}