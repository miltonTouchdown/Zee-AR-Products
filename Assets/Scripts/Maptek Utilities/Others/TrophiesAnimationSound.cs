using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Trophies.Maptek
{
    public class TrophiesAnimationSound : MonoBehaviour
    {


        AudioSource audioSource;

        public AudioClip audioClip;
        public AudioMixerGroup output;

        [Range(0, 256)]
        public int priority = 128;

        [Range(0, 1)]
        public float volume = 1;

        [Range(0, 3)]
        public float pitch = 1;

        [Range(-1, 1)]
        public float stereoPan = 0;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = output;
            audioSource.priority = priority;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.panStereo = stereoPan;

        }

        private void Start()
        {

        }

        void AudioStart()
        {
            audioSource.Play();
        }

    }
}