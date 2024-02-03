using MultiplayerGame.Code.Data.Enums;
using MultiplayerGame.Code.Data.StaticData.Sounds;
using MultiplayerGame.Code.Services.SaveLoad;

namespace MultiplayerGame.Code.Services.Sound
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