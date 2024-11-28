using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField]
        Slider musicVolumeSlider;

        [SerializeField]
        Slider sfxVolumeSlider;

        private void Awake()
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

            if (AudioManager.Instance == null)
                return;

            musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
            sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;
        }

        private void Start()
        {
            if (AudioManager.Instance == null)
                return;

            musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
            sfxVolumeSlider.value = AudioManager.Instance.SfxVolume;
        }

        private void OnEnable()
        {
        }

        public void OnMusicVolumeChanged(float value)
        {
            if (AudioManager.Instance == null)
                return;

            AudioManager.Instance.MusicVolume = value;
        }
        public void OnSfxVolumeChanged(float value)
        {
            if (AudioManager.Instance == null)
                return;

            AudioManager.Instance.SfxVolume = value;
        }
    }
}
