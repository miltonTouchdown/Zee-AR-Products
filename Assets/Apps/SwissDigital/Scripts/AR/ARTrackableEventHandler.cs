using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Trophies.SwissDigital
{
    public class ARTrackableEventHandler : DefaultTrackableEventHandler
    {
        private bool m_IsPlaced = false;
        [SerializeField]
        private ShaderBagControl m_shaderControl;

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnTrackingFound()
        {
            //base.OnTrackingFound();

            if (mTrackableBehaviour)
            {
                LeanTween.cancel(this.gameObject);

                GetComponentInChildren<LookAt>().LookAtOnce();

                m_IsPlaced = true;

                ARManager.Instance.OnTrackingFound();

                m_shaderControl.ShowObject();

                base.OnTrackingFound();
            }
        }

        protected override void OnTrackingLost()
        {
            base.OnTrackingLost();

            if (mTrackableBehaviour)
            {
                m_IsPlaced = false;

                ARManager.Instance.OnTrackingLost();

                m_shaderControl.HideObject();

                // Desactivar despues de ocultar el objeto a traves de shader
                LeanTween.delayedCall(this.gameObject, m_shaderControl.TimeTransition, () =>
                {
                    var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
                    var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
                    var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

                    // Disable rendering:
                    foreach (var component in rendererComponents)
                        component.enabled = false;

                    // Disable colliders:
                    foreach (var component in colliderComponents)
                        component.enabled = false;

                    // Disable canvas':
                    foreach (var component in canvasComponents)
                        component.enabled = false;
                });
            }
        }

    }
}