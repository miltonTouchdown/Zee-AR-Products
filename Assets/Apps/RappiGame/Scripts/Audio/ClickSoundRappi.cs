using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Trophies.Rappi
{
    [RequireComponent(typeof(Button))]
    public class ClickSoundRappi : MonoBehaviour
    {
        public AudioClip sound;
        public AudioMixerGroup output;

        private Button button { get { return GetComponent<Button>(); } }
        private AudioSource source { get { return GetComponent<AudioSource>(); } }

        void Start()
        {
            gameObject.AddComponent<AudioSource>();
            source.clip = sound;
            source.playOnAwake = false;
            source.outputAudioMixerGroup = output;

            button.onClick.AddListener(() => PlaySound());
        }

        void PlaySound()
        {
            source.PlayOneShot(sound);
        }

    }
}