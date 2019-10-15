using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Trophies.Rappi
{
    public class Container : MonoBehaviour
    {
        [Header("Config")]
        public int initPos = 0;

        public GameObject currPrize;

        public bool IsSelected = false;
        public bool HasPrize = false;
        public bool IsMoving = false;

        [Header("Audio")]
        public AudioMixerGroup audioOutput;
        public AudioClip clipOnSelected;
        public AudioClip clipOnMove;
        private AudioSource _aSource;

        [Header("Events")]
        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;

        void Start()
        {
            // Config audio
            _aSource = gameObject.AddComponent<AudioSource>();
            _aSource.playOnAwake = false;
            _aSource.outputAudioMixerGroup = audioOutput;

            HasPrize = (currPrize != null);
            OnDeselectedContainer();
        }

        public void Move(LTSpline spline, float vel, PepitoMinigameControl.OnFinishShuffleCallback onFinish = null)
        {
            IsMoving = true;

            PlaySound(clipOnMove);

            LeanTween.move(gameObject, spline, vel).setOnComplete(() =>
            {
                IsMoving = false;

                if (onFinish != null)
                    onFinish();
            }
            );
        }

        public void PauseMove(bool value)
        {
            if (value) LeanTween.pause(gameObject);
            else LeanTween.resume(gameObject);

            Debug.Log("Is pause " + gameObject.name + ": " + LeanTween.isPaused(gameObject));
        }

        /// <summary>
        /// Obtener index inicial del objeto en el juego.
        /// </summary>
        /// <returns>Retonar entero que indica la posicion inicial</returns>
        public int GetInitPos()
        {
            return initPos;
        }

        public void OnSelectContainer()
        {
            OnSelected.Invoke();

            IsSelected = true;

            PlaySound(clipOnSelected);
        }

        public void OnDeselectedContainer()
        {
            OnDeselected.Invoke();

            IsSelected = false;

            // Desactivar interfaz (flecha)
        }

        public Transform GetPositionPrize()
        {
            return transform.GetChild(0);
        }

        public GameObject CreatePrize(Transform parent = null)
        {
            GameObject prize = null;

            prize = Instantiate(currPrize, parent);
            prize.transform.localScale = Vector3.zero;
            prize.transform.position = parent.position;
            prize.name = "Prize";
            prize.transform.rotation = parent.localRotation;

            return prize;
        }

        private void PlaySound(AudioClip clip)
        {
            _aSource.PlayOneShot(clip);
        }
    }
}