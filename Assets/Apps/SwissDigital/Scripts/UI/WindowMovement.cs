using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Trophies.SwissDigital
{
    public class WindowMovement : MonoBehaviour
    {
        public string Id;
        public enum WindowType { Move, Scale, Fade, None }
        public WindowType windowType;
        public bool isActive = false;

        public RectTransform posInit, posFinal;
        public float fade = 1f;
        public float time = 1f;

        public UnityEvent onBeginShowPos, onFinishShowPos, onBeginHidePos, onFinishHidePos;

        public virtual void setActiveWindow(bool value)
        {
            isActive = value;

            onBeginTransition(value);

            switch (windowType)
            {
                case WindowType.Fade:
                    {
                        float alpha = (value) ? fade : 0f;
                        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), alpha, time).setOnComplete(() =>
                        {
                            onFinishTransition(value);
                        });
                        break;
                    }
                case WindowType.Scale:
                    {
                        Vector3 scale = (value) ? Vector3.one : Vector3.zero;
                        LeanTween.scale(gameObject.GetComponent<RectTransform>(), scale, time).setOnComplete(() =>
                        {
                            onFinishTransition(value);
                        });
                        break;
                    }
                case WindowType.Move:
                    {
                        Vector3 pos = (value) ? posFinal.localPosition : posInit.localPosition;
                        LeanTween.moveLocal(gameObject, pos, time).setOnComplete(() =>
                        {
                            onFinishTransition(value);
                        });
                        break;
                    }
                case WindowType.None:
                    {
                        onFinishTransition(value);
                        break;
                    }
            }
        }

        protected void onFinishTransition(bool value)
        {
            if (value) onFinishShowPos.Invoke();
            else onFinishHidePos.Invoke();
        }

        protected void onBeginTransition(bool value)
        {
            if (value) onBeginShowPos.Invoke();
            else onBeginHidePos.Invoke();
        }
    }
}