using UnityEngine;

namespace Trophies.Rappi
{
    public class LookAt : MonoBehaviour
    {
        public Transform target;
        public float delay = 1f;
        public Vector3 axisRotation;
        public bool isLook { get; set; }

        void Start()
        {
            var lookPos = target.position - transform.position;
            lookPos.x = axisRotation.x * lookPos.x;
            lookPos.y = axisRotation.y * lookPos.y;
            lookPos.z = axisRotation.z * lookPos.z;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }

        void Update()
        {
            if (!isLook)
                return;

            var lookPos = target.position - transform.position;
            lookPos.x = axisRotation.x * lookPos.x;
            lookPos.y = axisRotation.y * lookPos.y;
            lookPos.z = axisRotation.z * lookPos.z;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * delay);
        }
    }
}