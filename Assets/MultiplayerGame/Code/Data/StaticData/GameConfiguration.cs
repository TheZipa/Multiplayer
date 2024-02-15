using UnityEngine;

namespace MultiplayerGame.Code.Data.StaticData
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "StaticData/GameConfiguration")]
    public class GameConfiguration : ScriptableObject
    {
        public int StartBalance;
        public int MaxPlayers;
        public int MinPlayersForStart;
        public float DefaultSoundVolume;
    }
}