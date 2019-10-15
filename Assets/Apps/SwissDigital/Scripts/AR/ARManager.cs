using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

namespace Trophies.SwissDigital
{

    public class ARManager : MonoBehaviour
    {
        // Indica si se ha colisionado con el suelo para ubicar objetos
        public bool IsHitTest { get; private set; }

        // Indica si el objeto ha sido puesto en la escena
        public bool IsObjectPlaced { get; private set; }

        private UIMenuAR m_UIMenuAR;

        private static ARManager _instance;
        public static ARManager Instance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {
            IsHitTest = false;
            IsObjectPlaced = false;

            m_UIMenuAR = FindObjectOfType<UIMenuAR>();
            //DeviceTrackerARController.Instance.RegisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        }

        void OnDestroy()
        {
            Debug.Log("OnDestroy() called.");

            //DeviceTrackerARController.Instance.UnregisterDevicePoseStatusChangedCallback(OnDevicePoseStatusChanged);
        }

        public void HandleInteractiveHitTest(HitTestResult result)
        {
            if (result == null)
            {
                if (IsHitTest)
                {
                    IsHitTest = false;

                    if (onHitTest != null)
                        onHitTest(IsHitTest);
                }

                return;
            }
            else
            {
                if (!IsHitTest)
                {
                    IsHitTest = true;

                    if (onHitTest != null)
                        onHitTest(IsHitTest);
                }
            }
        }

        void OnDevicePoseStatusChanged(TrackableBehaviour.Status status, TrackableBehaviour.StatusInfo statusInfo)
        {
            switch (statusInfo)
            {
                case TrackableBehaviour.StatusInfo.NORMAL:
                    break;
                case TrackableBehaviour.StatusInfo.UNKNOWN:
                    break;
                case TrackableBehaviour.StatusInfo.INITIALIZING:
                    break;
                case TrackableBehaviour.StatusInfo.EXCESSIVE_MOTION:
                    break;
                case TrackableBehaviour.StatusInfo.INSUFFICIENT_FEATURES:
                    break;
                case TrackableBehaviour.StatusInfo.INSUFFICIENT_LIGHT:
                    break;
                case TrackableBehaviour.StatusInfo.RELOCALIZING:
                    // Display a relocalization message in the UI if:
                    // * No AnchorBehaviours are being tracked
                    // * None of the active/tracked AnchorBehaviours are in TRACKED status

                    StateManager stateManager = TrackerManager.Instance.GetStateManager();
                    if (stateManager != null)
                    {
                        // Cycle through all of the active AnchorBehaviours first.
                        foreach (TrackableBehaviour behaviour in stateManager.GetActiveTrackableBehaviours())
                        {
                            if (behaviour is AnchorBehaviour)
                            {
                                if (behaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED)
                                {
                                    // If at least one of the AnchorBehaviours has Tracked status,
                                    // then don't display the relocalization message.
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void OnTrackingLost()
        {
            IsObjectPlaced = false;

            m_UIMenuAR.SetActiveMessagePutObject(true);
        }

        public void OnTrackingFound()
        {
            IsObjectPlaced = true;

            m_UIMenuAR.SetActiveMessagePutObject(false);
        }

        // Indica si se ha detectado el suelo
        public delegate void OnHitTest(bool value);
        public static event OnHitTest onHitTest;
    }
}