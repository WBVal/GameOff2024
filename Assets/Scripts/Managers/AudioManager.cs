using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Threading.Tasks;
using Utils;
using Managers;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        #region Serialized Fields
        [SerializeField]
        AudioMixer audioMixer;

        [SerializeField]
        AudioSource musicSource;
        [SerializeField]
        bool hasIntro;
		[SerializeField]
		AudioClip musicIntro;
		[SerializeField]
        AudioClip musicloop;
		[SerializeField]
		AudioClip musicChase;

		[SerializeField]
        List<AudioSource> pausableAudioSources = new List<AudioSource>();

        [Header("Specific sources")]
        [SerializeField]
        AudioSource cheatSource;
		[SerializeField]
		AudioSource upgradeSource;
		[SerializeField]
		AudioSource trackingSource;
		[SerializeField]
		AudioSource detectionSource;
		#endregion

		#region Variables
		const string MUSIC_MIXER = "MusicVolume";
        const string SFX_MIXER = "SFXVolume";

        public const string MUSIC_KEY = "MusicVolume";
        public const string SFX_KEY = "SFXVolume";

        float musicVolume = 1f;
        public float MusicVolume
        {
            get { return musicVolume; }
            set
            {
                // Use VolumeSettings class to change value via sliders
                musicVolume = value;

                if (audioMixer != null)
                    audioMixer.SetFloat(MUSIC_MIXER, ConvertToLog(musicVolume));
            }

        }
        float sfxVolume = 1f;
        public float SfxVolume
        {
            get { return sfxVolume; }
            set
            {
                // Use VolumeSettings class to change value via sliders
                sfxVolume = value;

                if (audioMixer != null)
                    audioMixer.SetFloat(SFX_MIXER, ConvertToLog(sfxVolume));
            }

        }

        Coroutine introCoroutine;
        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if(cheatSource != null)
                pausableAudioSources.Add(cheatSource);
			if (upgradeSource != null)
				pausableAudioSources.Add(upgradeSource);
			if (trackingSource != null)
				pausableAudioSources.Add(trackingSource);
			if (detectionSource != null)
				pausableAudioSources.Add(detectionSource);
		}

        private void Start()
        {
            LoadVolume();

            if (hasIntro)
            {
                introCoroutine = StartCoroutine(PlayMusicWithIntro());
            }
            else
			{
				musicSource.clip = musicloop;
				musicSource.Play();
			}
        }

		private void OnDisable()
		{
			SceneFlowManager.Instance.GlobalMusicVolume = musicVolume;
            SceneFlowManager.Instance.GlobalSfxVolume = sfxVolume;
		}
		#endregion

		#region Private Methods
		private void LoadVolume()
        {
            musicVolume = SceneFlowManager.Instance.GlobalMusicVolume;
			sfxVolume = SceneFlowManager.Instance.GlobalSfxVolume;
			audioMixer.SetFloat(MUSIC_MIXER, ConvertToLog(musicVolume));
            audioMixer.SetFloat(SFX_MIXER, ConvertToLog(sfxVolume));
        }

        private float ConvertToLog(float input)
        {
            return Mathf.Log10(input) * 20f;
        }
		IEnumerator PlayMusicWithIntro()
		{
			musicSource.clip = musicIntro;
			musicSource.loop = false;
			musicSource.Play();
			yield return new WaitForSecondsRealtime(31.65f);
			musicSource.clip = musicloop;
			musicSource.loop = true;
			musicSource.Play();
		}
		#endregion

		#region Public Methods
		public void PlaySources()
        {
            foreach (AudioSource source in pausableAudioSources)
            {
                source.UnPause();
            }
        }
        public void PauseSources()
        {
            foreach (AudioSource source in pausableAudioSources)
            {
                source.Pause();
            }
		}
		public void StopMusic()
		{
            if(introCoroutine != null)
                StopCoroutine(introCoroutine);

            musicSource.Stop();
		}

		public void PlayChaseMusic()
        {
            musicSource.clip = musicChase;
            musicSource.loop = true;
            musicSource.Play();

        }
		#endregion

		#region Play Specific Audio methods
		public void PlayCheatSound()
        {
            cheatSource?.Play();

		}
		public void PlayUpgradeSound()
		{
			upgradeSource?.Play();

		}
        public void PlayTrackingSound()
        {
            trackingSource?.Play();
		}
		public void PlayDetectionSound()
		{
			detectionSource?.Play();
		}
		#endregion
	}
}
