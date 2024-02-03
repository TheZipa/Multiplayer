using System;
using Game.Code.Data.Enums;
using UnityEngine;

namespace Game.Code.Data.StaticData.Sounds
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip Clip;
        public SoundId Id;
    }
}