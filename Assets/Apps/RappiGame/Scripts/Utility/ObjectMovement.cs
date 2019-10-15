using UnityEngine;

namespace Trophies.Rappi
{
    public class ObjectMovement : MonoBehaviour
    {

        private Vector3 vRotation = Vector3.up;
        private float velRotation = 0;
        public bool isRotating = false;

        void Start()
        {
            //StartRotation(5f, Vector3.up);
        }

        void Update()
        {
            if (isRotating)
            {
                transform.Rotate(vRotation * velRotation);
            }
        }

        public void StartRotation(float vel, Vector3 vDir)
        {
            vRotation = vDir;
            velRotation = vel;

            isRotating = true;
        }

        public void StopRotation()
        {
            isRotating = false;
        }

        public void RotateTo(float vel, Vector3 vectorDirection, float time, OnFinishMovement onFinish = null)
        {
            LeanTween.rotate(gameObject, vectorDirection, time);
        }

        public void MoveTo(LTSpline spline, float time, OnFinishMovement onFinish = null)
        {
            LeanTween.move(gameObject, spline, time).setOnComplete(() =>
            {
                if (onFinish != null)
                    onFinish();
            });
        }

        public void MoveTo(Vector3 pos, float time, OnFinishMovement onFinish = null)
        {
            LeanTween.move(gameObject, pos, time).setOnComplete(() =>
            {
                if (onFinish != null)
                    onFinish();
            });
        }

        public void ScaleTo(Vector3 scale, float time, OnFinishMovement onFinish = null)
        {
            LeanTween.scale(gameObject, scale, time).setOnComplete(() =>
            {
                if (onFinish != null)
                    onFinish();
            });
        }

        public void FadeTo(float fade, float time, OnFinishMovement onFinish = null)
        {
            LeanTween.alpha(gameObject, fade, time).setOnComplete(() =>
            {
                if (onFinish != null)
                    onFinish();
            });
        }

        public delegate void OnFinishMovement();
        public static event OnFinishMovement onFinishMovement;
    }
}