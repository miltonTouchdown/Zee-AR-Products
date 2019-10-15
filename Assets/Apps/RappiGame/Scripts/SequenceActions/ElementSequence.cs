using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Trophies.Rappi
{
    public abstract class ElementSequence : MonoBehaviour
    {
        private SequenceControl.OnFinishElementActionCallback _onFinishActionElement = null;

        public AudioMixerGroup audioOutput;
        private AudioSource _aSource;

        public UnityEvent OnStart;
        public UnityEvent OnFinish;
        public UnityEvent OnCancel;

        protected virtual void Start()
        {
            _aSource = gameObject.AddComponent<AudioSource>();
            _aSource.playOnAwake = false;
            _aSource.outputAudioMixerGroup = audioOutput;
        }

        public virtual void StartElementAction(SequenceControl.OnFinishElementActionCallback onFinish = null)
        {
            OnStart.Invoke();

            if (onFinish != null)
            {
                _onFinishActionElement = onFinish;
            }
        }

        public virtual void FinishElementAction()
        {
            OnFinish.Invoke();

            if (_onFinishActionElement != null)
            {
                _onFinishActionElement();
            }

            _onFinishActionElement = null;
        }

        public virtual void CancelElementAction()
        {
            OnCancel.Invoke();

            _onFinishActionElement = null;
        }

        protected void PlaySound(AudioClip clip)
        {
            _aSource.PlayOneShot(clip);
        }

        protected LTSpline GetSplineFromTranform(Transform ts)
        {
            Vector3[] arrVectors = new Vector3[ts.childCount];

            for (int i = 0; i < ts.childCount; i++)
            {
                arrVectors[i] = ts.GetChild(i).position;
            }

            return new LTSpline(arrVectors);
        }
    }
}