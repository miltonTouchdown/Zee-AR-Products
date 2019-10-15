using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trophies.SwissDigital
{
    public class UITitleEffectTransition : MonoBehaviour
    {
        public RectTransform finalPos;
        public Text title;
        public UIFade titleMenuFade;
        public float timeTransition = .2f;

        void Start()
        {
            LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, 0f);
        }

        public void setText(string text)
        {
            title.text = text;
        }

        public void StartTransition(RectTransform initPos)
        {
            titleMenuFade.Fade(0f);

            initPos.GetComponent<UIFade>().Fade(0);

            gameObject.GetComponent<RectTransform>().position = initPos.position;

            LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 1f, .1f).setOnComplete(() =>
            {

                LeanTween.moveLocal(gameObject, finalPos.localPosition, timeTransition).setOnComplete(() =>
                {

                    titleMenuFade.Fade(1f);

                    LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, .3f);
                });
            });

        }
    }
}