using UnityEngine;
using UnityEngine.Audio;

namespace Trophies.Rappi
{
    public class MusicControl : MonoBehaviour
    {
        public AudioMixer audioMasterOutput;
        public AudioMixerGroup audioMusicOutput;
        private AudioSource _aSource;

        // Volumen del juego
        private float _masterVolume;

        public bool hasSound = true;
        public bool hasMusica = true;

        // Indica si existe audio en el juego
        public bool hasAudio = true;

        [Header("Main Music")]
        public AudioClip clipBackground;
        public AudioClip clipVictory;
        public AudioClip clipDefeat;

        void Start()
        {
            _aSource = gameObject.AddComponent<AudioSource>();
            _aSource.playOnAwake = false;
            _aSource.loop = true;
            _aSource.outputAudioMixerGroup = audioMusicOutput;

            // Obtener el volumen
            audioMasterOutput.GetFloat("MasterVol", out _masterVolume);

            PlayMusic(clipBackground);
            SetMusicVolume(1f);
        }

        public void PlayMusic(AudioClip clip)
        {
            _aSource.clip = clip;
            _aSource.Play();
        }

        public void StopMusic()
        {
            _aSource.Stop();
            _aSource.clip = null;
        }

        public void SetMusicVolume(float vol)
        {
            _aSource.volume = vol;
        }

        public void setAudio(bool value)
        {
            this.hasAudio = value;

            float currVol = (value) ? _masterVolume : -80f;

            audioMasterOutput.SetFloat("MasterVol", currVol);
        }
    }
}