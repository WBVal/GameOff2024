using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace Audio
{
    // CustomAudioSource requires the GameObject to have a AudioSource component
    [RequireComponent(typeof(AudioSource))]
    public class CustomAudioSource : MonoBehaviour
    {
        [SerializeField]
        List<AudioClip> clipList = new List<AudioClip>();

        [SerializeField]
        bool playRandomly;

        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(int soundIndex = 0)
        {
            if (clipList.Count <= 0)
            {
                Debug.LogError("There are no sounds to be played");
                return;
            }

            if (playRandomly)
            {
                audioSource.PlayOneShot(clipList[Random.Range(0, clipList.Count)]);
            }
            else
            {
                audioSource.PlayOneShot(clipList[soundIndex]);
            }
        }

        public void Mute(bool muted)
        {
            if (audioSource != null)
                audioSource.mute = muted;
        }
    }
}
