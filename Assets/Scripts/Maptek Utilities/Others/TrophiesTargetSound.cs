using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Trophies.Maptek
{
    public class TrophiesTargetSound : MonoBehaviour
    {

        public enum AudioType
        {
            NONE,
            TARGET_ENTER,
            TARGET_EXIT
        }

        public AudioType audioType;

        AudioSource audioSource;
        ITrackableAudioHandler audioTracker;
        bool componentError;

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

        public bool StopOnEventEnds;



        private void Awake()
        {
            audioTracker = GetComponent<ITrackableAudioHandler>();
            if (audioTracker == null) componentError = true;

            if (!componentError)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.clip = audioClip;
                audioSource.outputAudioMixerGroup = output;
                audioSource.priority = priority;
                audioSource.volume = volume;
                audioSource.pitch = pitch;
                audioSource.panStereo = stereoPan;

                audioTracker.AudioTargetFoundStartEvent += OnEnterTargetAudioStart;
                audioTracker.AudioTargetFoundStopEvent += OnEnterTargetAudioStop;

                audioTracker.AudioTargetLostStartEvent += OnLostTargetAudioStart;
                audioTracker.AudioTargetLostStopEvent += OnLostTargetAudioStop;

            }
        }

        private void Start()
        {

        }

        void OnEnterTargetAudioStart()
        {
            if (audioType == AudioType.TARGET_ENTER && audioClip != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                else
                {
                    audioSource.Stop();
                    audioSource.Play();
                }
            }
        }

        void OnEnterTargetAudioStop()
        {
            if (audioType == AudioType.TARGET_ENTER && audioClip != null)
            {
                if (audioSource.isPlaying && StopOnEventEnds)
                {
                    audioSource.Stop();
                }
            }
        }

        void OnLostTargetAudioStart()
        {
            if (audioType == AudioType.TARGET_EXIT && audioClip != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                else
                {
                    audioSource.Stop();
                    audioSource.Play();
                }
            }
        }

        void OnLostTargetAudioStop()
        {
            if (audioType == AudioType.TARGET_EXIT && audioClip != null)
            {
                if (audioSource.isPlaying && StopOnEventEnds)
                {
                    audioSource.Stop();
                }
            }
        }

    }
}