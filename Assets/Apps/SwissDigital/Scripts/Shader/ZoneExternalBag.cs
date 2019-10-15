using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.SwissDigital
{
    public class ZoneExternalBag : ZoneBag
    {
        protected override void Start()
        {
            base.Start();

            ModeView = ModeBagView.Normal;

            IsShow = false;

            //CurrTexture = (Texture2D)matBag.GetTexture("Texture2D_F572E918");
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
                        SetAlpha(false);
                        break;
                    }
                case ModeBagView.Normal:
                    {
                        SetAlpha(true);
                        break;
                    }
            }
        }

        public override void SetTexture(Texture2D texture)
        {
            if (IsSetTexture)
                return;

            IsSetTexture = true;

            CurrTexture = texture;

            // Desaparecer (0f) objeto cambiando el alpha en el shader. Luego cambiar la texture. Aparecer (1f) el objeto.
            SetAlpha(false, () =>
            {
            // El nombre de la variable esta en el shader. Es generado por Unity
            matBag.SetTexture("Texture2D_F572E918", texture);

                LeanTween.delayedCall(.3f, () =>
                {
                    SetAlpha(true, () => { IsSetTexture = false; });
                });
            });
        }

        public override void SetAlpha(bool isVisible, OnTaskComplete onTask = null)
        {
            float to = 0f, from = 0f;

            switch (ModeView)
            {
                case ModeBagView.Normal:
                    {
                        to = (isVisible) ? 0f : 1f;
                        from = (isVisible) ? 1f : 0f;

                        LeanTween.value(this.gameObject, (f) =>
                        {
                        // Cambiar alpha
                        matBag.SetFloat("Vector1_FAB99759", f);
                        }, from, to, TimeTransition).setOnComplete(() =>
                        {
                            IsShow = isVisible;

                            if (onTask != null)
                                onTask();
                        });

                        break;
                    }
                case ModeBagView.Focus:
                    {
                        break;
                    }
            }
        }

        public override void Hide(OnTaskComplete onTask = null)
        {
            if (m_currModeView != ModeView)
                return;

            SetAlpha(false, onTask);
        }

        public override void Show()
        {
            if (m_currModeView != ModeView)
                return;

            SetAlpha(true);
        }
    }
}