using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Trophies.Maptek
{
    public class TrophiesImageTarget : MonoBehaviour, ITrackableEventHandler, ITrackableAudioHandler
    {
        public event AudioEventHandler AudioTargetFoundStartEvent;
        public event AudioEventHandler AudioTargetFoundStopEvent;
        public event AudioEventHandler AudioTargetLostStartEvent;
        public event AudioEventHandler AudioTargetLostStopEvent;

        public bool isFound;
        protected TrackableBehaviour mTrackableBehaviour;
        public bool isActive;
        public bool isTrackeable;
        public bool isTrackerOnScreen;
        public bool isFreezed;


        public bool isGroupAnimating;
        public bool isRotating;
        public bool isMovingToCenter;
        Quaternion LookPosition;
        Quaternion FromLookPosition;
        Vector3 lookCenter;
        float rotationSpeed;
        float grupAnimMoveSpeed;

        float rotationTimer;

        public Transform ChildTransform;

        public AnimationCurve animTrackLost;
        public AnimationCurve animTrackFind;

        [Header("Interferencia")]
        // Indica el tiempo de espera antes de desactivar el modelo al momento de
        // no detectar el marcador.
        public float waitTimeRecoverTracking = 1f;
        public bool isWaitingRecover = false;

        private ModelARControl _mARControl;
        private TouchRotation _touchRotation;

        Vector3 initialScale;

        protected virtual void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
                mTrackableBehaviour.RegisterTrackableEventHandler(this);

            initialScale = ChildTransform.localScale;

            _mARControl = GetComponent<ModelARControl>();
            _touchRotation = GetComponent<TouchRotation>();
        }

        protected virtual void OnDestroy()
        {
            if (mTrackableBehaviour)
                mTrackableBehaviour.UnregisterTrackableEventHandler(this);
            //ARCameraManager.TargetDestroy(this);
        }

        void Update()
        {
            if (!isGroupAnimating)
            {
                U_OnEnterAnimation();
                U_OnEndAnimation();
            }
            if (isGroupAnimating)
            {
                if (isRotating)
                {
                    rotationTimer -= Time.deltaTime;
                    float step = rotationSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up, step);

                    if (rotationTimer <= 0)
                    {
                        isRotating = false;
                        isMovingToCenter = true;
                    }

                }
                else if (isMovingToCenter)
                {
                    float step = grupAnimMoveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, lookCenter, step);

                    if (Vector3.Distance(transform.position, lookCenter) < 0.01)
                    {
                        isMovingToCenter = false;
                        isGroupAnimating = false;
                    }
                }

            }
        }

        bool OnEnterAnimation = false;
        enum EnterEndAnimationStates
        {
            NONE,
            ANIMATING,
            END
        }
        EnterEndAnimationStates enterAnimationState = EnterEndAnimationStates.NONE;
        float enterAnimationTimer;
        float enterAnimatorMaxTimer;

        void U_OnEnterAnimation()
        {
            if (!OnEnterAnimation) return;
            //Animation setup
            if (enterAnimationState == EnterEndAnimationStates.NONE)
            {
                SetAudioFound();
                ChildTransform.localScale = initialScale;
                if (!ChildTransform.gameObject.activeInHierarchy)
                {
                    ChildTransform.gameObject.SetActive(true);
                }
                enterAnimatorMaxTimer = animTrackFind.keys[animTrackFind.length - 1].time;
                enterAnimationTimer = 0;
                float scale = animTrackFind.keys[0].value;
                ChildTransform.localScale = new Vector3(scale, scale, scale);

                enterAnimationState = EnterEndAnimationStates.ANIMATING;
            }
            else if (enterAnimationState == EnterEndAnimationStates.ANIMATING)
            {
                enterAnimationTimer += Time.deltaTime;
                if (enterAnimationTimer >= enterAnimatorMaxTimer)
                {
                    enterAnimationState = EnterEndAnimationStates.END;
                }
                else
                {
                    ChildTransform.localScale = initialScale * animTrackFind.Evaluate(enterAnimationTimer);
                }
            }
            else if (enterAnimationState == EnterEndAnimationStates.END)
            {
                SetAudioFound(false);
                ChildTransform.localScale = initialScale;
                OnEnterAnimation = false;
                enterAnimationState = EnterEndAnimationStates.NONE;
                //ARCameraManager.TargetFound(this);
            }
        }

        bool OnEndAnimation = false;
        EnterEndAnimationStates endAnimationState = EnterEndAnimationStates.NONE;
        void U_OnEndAnimation()
        {
            if (!OnEndAnimation) return;
            //Animation setup
            if (endAnimationState == EnterEndAnimationStates.NONE)
            {
                SetAudioLost();
                ChildTransform.localScale = initialScale;
                enterAnimatorMaxTimer = animTrackLost.keys[animTrackLost.length - 1].time;
                enterAnimationTimer = 0;

                endAnimationState = EnterEndAnimationStates.ANIMATING;
            }
            else if (endAnimationState == EnterEndAnimationStates.ANIMATING)
            {
                enterAnimationTimer += Time.deltaTime;
                if (enterAnimationTimer >= enterAnimatorMaxTimer)
                {
                    endAnimationState = EnterEndAnimationStates.END;
                }
                else
                {
                    ChildTransform.localScale = initialScale * animTrackLost.Evaluate(enterAnimationTimer);
                }
            }
            else if (endAnimationState == EnterEndAnimationStates.END)
            {
                SetAudioLost(false);
                SetComponents(false);
                ChildTransform.localScale = initialScale;
                OnEndAnimation = false;
                endAnimationState = EnterEndAnimationStates.NONE;
                ChildTransform.gameObject.SetActive(false);
            }
        }


        /// <summary>
        ///     Implementation of the ITrackableEventHandler function called when the
        ///     tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
            TrackableBehaviour.Status previousStatus,
            TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                isFound = true;
                Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
                OnTrackingFound();
            }
            else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                     newStatus == TrackableBehaviour.Status.NO_POSE)
            {
                isFound = false;
                Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
                WaitRecoverTracking();
                //OnTrackingLost();
            }
            else
            {
                // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
                // Vuforia is starting, but tracking has not been lost or found yet
                // Call OnTrackingLost() to hide the augmentations
                Debug.Log("trackingStart");
                OnTrackingStart();
            }
        }

        protected void OnTrackingStart()
        {
            Debug.Log("OnTrackingStart");
            SetComponents(false);
            //ARCameraManager.AddTargetReference(this);       
        }

        protected virtual void OnTrackingFound()
        {
            ARManager.Instance.AddTargetReference(this);         

            Debug.Log("OnTrackingFound: " + ARManager.Instance.name);
            if (isTrackeable) return;

            if (_mARControl != null)
                _mARControl.Init();

            if (_touchRotation != null)
            {
                _touchRotation.canRotate = true;
            }

            TimerController.Instance.CancellAll();

            //If end animation is running reset
            OnEndAnimation = false;
            ChildTransform.localScale = initialScale;

            OnEnterAnimation = true;
            SetComponents(true);
            // Inform ARCameraManager

        }

        public void WaitRecoverTracking()
        {
            Debug.Log("WaitRecoverTracking");
            if (_touchRotation != null)
            {
                _touchRotation.canRotate = false;
            }

            isWaitingRecover = true;

            ARManager.Instance.WaitTarget();

            // Congelar animacion
            TimerController.Instance.AddTimer(new TimerController.CustomTimer(waitTimeRecoverTracking, () =>
            {
                if (!isTrackeable)
                {
                    OnTrackingLost();
                }
            }));

        }

        protected virtual void OnTrackingLost()
        {
            Debug.Log("OnTrackingLost");
            if (isFreezed) return;
            //Debug.Log("TrackLost");
            //If Initial animation is running reset
            isWaitingRecover = false;
            OnEnterAnimation = false;
            ChildTransform.localScale = initialScale;


            OnEndAnimation = true;
            //ARCameraManager.TargetLost(this);
            ARManager.Instance.OnTargetLost(this);
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

        public void FreezeTracking(bool freezeStatus = true)
        {
            if (freezeStatus)
            {
                isFreezed = true;
                isTrackeable = false;
                mTrackableBehaviour.enabled = false;
            }
            else
            {
                isFreezed = false;
                isTrackeable = true;
            }
        }

        public void LookAndGoToPosition(Vector3 positionToGo, float rotationTime, float speedRotation, float speedMovement)
        {
            lookCenter = positionToGo;
            grupAnimMoveSpeed = speedMovement;
            rotationSpeed = speedRotation;
            //Quaternion auxLookPosition = transform.rotation;
            //transform.Rotate(Vector3.up * 90);
            //transform.LookAt(positionToGo);
            //LookPosition = transform.rotation;
            //transform.rotation = auxLookPosition;
            isGroupAnimating = true;
            //isMovingToCenter = true;
            isRotating = true;
            rotationTimer = rotationTime;
        }

        public void StopTracking()
        {
            isTrackeable = false;
        }

        public void Hide()
        {
            SetComponents(false);
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
    }
}