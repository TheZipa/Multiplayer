using UnityEngine;

namespace MultiplayerGame.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "StaticData/LocationData")]
    public class LocationData : ScriptableObject
    {
        public Vector3 PlayersSpawnLocation;
        public float PlayerSpawnOffset;
    }
}