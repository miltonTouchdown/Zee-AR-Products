using UnityEngine;
using Vuforia;

namespace Trophies.Rappi
{
    public class ARRappiTarget : MonoBehaviour, ITrackableEventHandler, IRappiTrackableAudioHandler
    {
        public event AudioEventHandler AudioTargetFoundStartEvent;
        public event AudioEventHandler AudioTargetFoundStopEvent;
        public event AudioEventHandler AudioTargetLostStartEvent;
        public event AudioEventHandler AudioTargetLostStopEvent;

        protected TrackableBehaviour mTrackableBehaviour;

        public bool isFound = false;

        protected virtual void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        protected virtual void OnDestroy()
        {
            if (mTrackableBehaviour)
                mTrackableBehaviour.UnregisterTrackableEventHandler(this);
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
        newStatus == TrackableBehaviour.Status.TRACKED ||
        newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                isFound = true;
                //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
                OnTrackingFound();
            }
            else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                     newStatus == TrackableBehaviour.Status.NO_POSE)
            {
                isFound = false;
                //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
                OnTrackingLost();
            }
            else
            {
                // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
                // Vuforia is starting, but tracking has not been lost or found yet
                // Call OnTrackingLost() to hide the augmentations
                //Debug.Log("trackingStart");
                OnTrackingStart();
            }
        }

        public void SetAudioFound(bool set = true)
        {
            if (set && AudioTargetFoundStartEvent != null)
            {
                AudioTargetFoundStartEvent();
            }

            if (!set && AudioTargetFoundStopEvent != null)
            {
                AudioTargetFoundStopEvent();
            }
        }

        public void SetAudioLost(bool set = true)
        {
            if (set && AudioTargetLostStartEvent != null)
            {
                AudioTargetLostStartEvent();
            }

            if (!set && AudioTargetLostStopEvent != null)
            {
                AudioTargetLostStopEvent();
            }
        }

        protected void OnTrackingStart()
        {
            //Debug.Log("OnTrackingStart");
            SetComponents(false);
        }

        protected virtual void OnTrackingFound()
        {
            //Debug.Log("OnTrackingFound");

            ARManagerRappi.Instance.AddTargetReference();
            SetAudioFound();
            SetComponents(true);
        }
        protected virtual void OnTrackingLost()
        {
            //Debug.Log("OnTrackingLost");

            ARManagerRappi.Instance.OnTargetLost();
            SetAudioLost();
        }

        private void SetComponents(bool setStatus)
        {
            var rendererComponents = GetComponentsInChildren<Renderer>(setStatus);
            var meshRendererComponents = GetComponentsInChildren<SkinnedMeshRenderer>(setStatus);
            var AnimatorComponents = GetComponentsInChildren<Animator>(setStatus);
            var colliderComponents = GetComponentsInChildren<Collider>(setStatus);
            var canvasComponents = GetComponentsInChildren<Canvas>(setStatus);



            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = setStatus;

            // Disable rendering:
            foreach (var component in meshRendererComponents)
                component.enabled = setStatus;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = setStatus;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = setStatus;

            // Stop Anims':
            foreach (var component in AnimatorComponents)
            {
                if (!setStatus) component.enabled = false;
                if (setStatus) component.enabled = true;
            }

        }
    }
}