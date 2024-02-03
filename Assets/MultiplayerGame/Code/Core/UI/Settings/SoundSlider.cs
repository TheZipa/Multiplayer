using System;
using MultiplayerGame.Code.Core.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI.Settings
{
    public class SoundSlider : FadeBaseWindow
    {
        public event Action<float> OnVolumeChanged;
        
        [SerializeField] private Slider _soundVolumeSlider;

        public void Construct(float volume) => _soundVolumeSlider.value = volume;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _soundVolumeSlider.onValueChanged.AddListener(volume => OnVolumeChanged?.Invoke(volume));
        }
    }
}