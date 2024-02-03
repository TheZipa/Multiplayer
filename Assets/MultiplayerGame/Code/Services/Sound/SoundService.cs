using System.Collections.Generic;
using System.Linq;
using MultiplayerGame.Code.Data.Enums;
using MultiplayerGame.Code.Data.StaticData.Sounds;
using MultiplayerGame.Code.Services.CoroutineRunner;
using MultiplayerGame.Code.Services.SaveLoad;
using UnityEngine;

namespace MultiplayerGame.Code.Services.Sound
{
    public class SoundService : MonoBehaviour, ISoundService, ICoroutineRunner
    {
        public bool EffectsMuted
        {
            get => _effectsSource.mute;
            set
            {
                _effectsSource.mute = !value;
                _musicSource.mute = !value;
            }
        }

        public float Volume => _effectsSource.volume;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsSource;
        
        private Dictionary<SoundId, AudioClipData> _sounds;
        private ISaveLoad _saveLoad;

        public void Construct(ISaveLoad saveLoad, SoundData soundData)
        {
            _saveLoad = saveLoad;
            _sounds = soundData.AudioEffectClips.ToDictionary(s => s.Id);
            _musicSource.clip = soundData.BackgroundMusic;
            SetEffectsVolume(_saveLoad.Progress.Settings.SoundVolume);
        }

        public void PlayBackgroundMusic() => _musicSource.Play();

        public void PlayEffectSound(SoundId soundId) =>
            _effectsSource.PlayOneShot(_sounds[soundId].Clip);

        public void SetEffectsVolume(float volume)
        {
            _effectsSource.volume = volume;
            _musicSource.volume = volume;
            _saveLoad.Progress.Settings.SoundVolume = volume;
        }
    }
}