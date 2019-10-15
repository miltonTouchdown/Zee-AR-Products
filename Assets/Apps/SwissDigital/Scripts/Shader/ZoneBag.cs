using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.SwissDigital
{
    public abstract class ZoneBag : MonoBehaviour
    {
        public int id;
        public string nameZone;

        public float TimeTransition = 1f;
        public Texture2D CurrTexture;
        public Material matBag;

        protected ModeBagView m_currModeView;

        public bool IsShow = false;
        public bool IsSetTexture = false;

        protected Renderer m_renderer;
        // Indica el modo vista al cual pertenece
        public ModeBagView ModeView;
        protected ShaderBagControl m_shaderControl;

        protected virtual void Start()
        {
            m_renderer = GetComponent<Renderer>();

            m_renderer.sharedMaterial = matBag;
        }

        /// <summary>
        /// Cambiar el material del objecto para visualizacion
        /// </summary>
        /// <param name="modeView">Modo de visualizacion</param>
        public virtual void ChangeModeView(ModeBagView modeView)
        {
            m_currModeView = modeView;
        }

        public virtual void SetTexture(Texture2D texture)
        {

        }

        public virtual void FocusTarget(bool value)
        {

        }

        public virtual void SetAlpha(bool isVisible, OnTaskComplete onTask = null)
        {

        }

        public virtual void Hide(OnTaskComplete onTask = null)
        {

        }

        public virtual void Show()
        {

        }

        public void SetShaderControl(ShaderBagControl sc)
        {
            m_shaderControl = sc;
        }

        public Texture2D GetTexture2D()
        {
            return CurrTexture;
        }

        // Indica si se ha detectado el suelo
        public delegate void OnTaskComplete();
        public static event OnTaskComplete onTaskComplete;
    }
}