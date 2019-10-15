using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.SwissDigital
{
    public class UIFade : MonoBehaviour
    {
        public float timeFade = .3f;

        private void Start()
        {
        }

        public void Fade(float to, float time, OnFinishFade onFinish = null)
        {
            LeanTween.alpha(gameObject.GetComponent<RectTransform>(), to, time).setOnComplete(() =>
            {
                if (onFinish != null)
                    onFinish();
            });
        }

        public void Fade(float to)
        {
            LeanTween.alpha(gameObject.GetComponent<RectTransform>(), to, timeFade);
        }

        // Indica si se ha detectado el suelo
        public delegate void OnFinishFade();
        public static event OnFinishFade onFinishFade;
    }
}