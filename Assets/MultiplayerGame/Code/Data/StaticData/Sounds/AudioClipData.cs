using System;
using MultiplayerGame.Code.Data.Enums;
using UnityEngine;

namespace MultiplayerGame.Code.Data.StaticData.Sounds
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip Clip;
        public SoundId Id;
    }
}