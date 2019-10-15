using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.SwissDigital
{
    public class ZoneInternalBag : ZoneBag
    {
        public float focusTransparency = .2f;

        protected override void Start()
        {
            base.Start();

            ModeView = ModeBagView.Focus;

            // Ocultar (alpha)
            matBag.SetFloat("Vector1_FAB99759", 0f);
            IsShow = false;
        }

        /// <summary>
        /// Cambiar el material del objecto para visualizacion
        /// </summary>
        /// <param name="modeView">Modo de visualizacion</param>
        public override void ChangeModeView(ModeBagView modeView)
        {
            base.ChangeModeView(modeView);

            switch (modeView)
            {
                case ModeBagView.Focus:
                    {
                        FocusTarget(false);
                        break;
                    }
                case ModeBagView.Normal:
                    {
                        FocusTarget(false);
                        break;
                    }
            }
        }

        public override void FocusTarget(bool value)
        {
            float focus = (value) ? 1f : 0f;
            float transparency = (value) ? 1f : focusTransparency;

            matBag.SetFloat("Boolean_DAFC24A6", focus);

            matBag.SetVector("Vector2_B5455978", new Vector4(transparency, 0f, 0f, 0f));
        }

        public override void SetAlpha(bool isVisible, OnTaskComplete onTask = null)
        {
            float to = 0f, from = 0f;

            // Desaparecer o aparecer elemento dependiendo de su estado Focus
            float transparency = (matBag.GetFloat("Boolean_DAFC24A6") == 1f) ? 1f : focusTransparency;
            float currTransparency = matBag.GetVector("Vector2_B5455978").x;

            to = (isVisible) ? transparency : 0f;
            from = (isVisible) ? 0f : currTransparency;

            LeanTween.value(this.gameObject, (f) =>
            {
            // Cambiar alpha
            matBag.SetVector("Vector2_B5455978", new Vector4(f, 0f, 0f, 0f));
            }, from, to, TimeTransition).setOnComplete(() =>
            {
                IsShow = isVisible;

                if (onTask != null)
                    onTask();
            });
        }

        public override void Hide(OnTaskComplete onTask = null)
        {
            SetAlpha(false);
        }

        public override void Show()
        {
            SetAlpha(true);
        }
    }
}