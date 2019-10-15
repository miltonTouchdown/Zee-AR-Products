using UnityEngine;

namespace Trophies.Rappi
{
    public class RotationMovement : MonoBehaviour
    {
        private Vector3 vectorRotation = Vector3.up;
        private float velocityRotation = 0;
        public bool isRotating = false;

        void Update()
        {
            if (isRotating)
            {
                transform.Rotate(vectorRotation * velocityRotation, Space.World);
            }
        }

        public void StartRotation(float velRotation, Vector3 vRotation)
        {
            vectorRotation = vRotation;
            velocityRotation = velRotation;

            isRotating = true;
        }

        public void StopRotation()
        {
            isRotating = false;
        }
    }
}