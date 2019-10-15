using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trophies.SwissDigital
{
    public class ShaderBagControl : MonoBehaviour
    {
        public ZoneBag[] arrZoneBags;

        // Tiempo transicion show y hide
        public float TimeTransition = 1f;

        public bool IsShow = false;

        [SerializeField]
        private ModeBagView m_currModeView = ModeBagView.None;

        void Start()
        {
            arrZoneBags = FindObjectsOfType<ZoneBag>();

            // TODO asignar tiempo transicion a cada zonebag

            foreach (ZoneBag z in arrZoneBags)
            {
                z.SetShaderControl(this);
                z.Hide();
            }

            setModeView(ModeBagView.Normal);
        }

        public void ShowObject()
        {
            //SetActiveObject(true);
            if (IsShow)
                return;

            IsShow = true;

            foreach (ZoneBag zb in arrZoneBags)
            {
                zb.Show();
            }
        }

        public void HideObject()
        {
            //SetActiveObject(false);
            if (!IsShow)
                return;

            IsShow = false;

            foreach (ZoneBag zb in arrZoneBags)
            {
                zb.Hide();
            }
        }

        public void setTexture(int id, Texture2D texture)
        {
            if (m_currModeView != ModeBagView.Normal)
                return;

            foreach (ZoneBag zb in arrZoneBags)
            {
                if (zb.ModeView == ModeBagView.Normal)
                {
                    if (id == zb.id)
                    {
                        zb.SetTexture(texture);
                    }
                    else
                    {
                        zb.SetAlpha(false, () =>
                        {
                            LeanTween.delayedCall(.3f, () =>
                            {
                                zb.SetAlpha(true);
                            });
                        });
                    }
                }
            }
        }

        public void setFocus(int id)
        {
            if (m_currModeView != ModeBagView.Focus)
                return;

            foreach (ZoneBag zb in arrZoneBags)
            {
                zb.FocusTarget(id == zb.id);
            }
        }

        public void setModeView(ModeBagView modeView)
        {
            if (m_currModeView == modeView)
                return;

            m_currModeView = modeView;

            foreach (ZoneBag zb in arrZoneBags)
            {
                zb.ChangeModeView(modeView);
            }
        }

        public Texture GetTextureZoneBag(int id)
        {
            return (Texture)arrZoneBags.Single((zb) => zb.id == id).GetTexture2D();
        }
    }

    public enum ModeBagView { None, Focus, Normal }
}