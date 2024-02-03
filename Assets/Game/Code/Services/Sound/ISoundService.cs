using Game.Code.Data.Enums;
using Game.Code.Data.StaticData.Sounds;
using Game.Code.Services.SaveLoad;

namespace Game.Code.Services.Sound
{
    public interface ISoundService
    {
        bool EffectsMuted { get; set; }
        float Volume { get; }
        void Construct(ISaveLoad saveLoad, SoundData soundData);
        void PlayBackgroundMusic();
        void PlayEffectSound(SoundId soundId);
        void SetEffectsVolume(float volume);
    }
}