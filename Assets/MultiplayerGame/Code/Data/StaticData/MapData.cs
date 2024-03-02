using System;
using UnityEngine;

namespace MultiplayerGame.Code.Data.StaticData
{
    [Serializable]
    public class MapData
    {
        public Vector3 PlayersSpawnLocation;
        public Sprite MapPreview;
        public string Name;
        public string SceneName;
    }
}