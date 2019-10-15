using UnityEngine;
using UnityEngine.EventSystems;

namespace Trophies.Maptek
{
    public class TouchRotation : MonoBehaviour
    {
        public Transform model;
        public float velRotate = .1f;
        public bool canRotate = false;

        void Start()
        {
#if UNITY_EDITOR
            gameObject.AddComponent<BoxCollider>();
#endif
        }

        void Update()
        {
            if (!canRotate)
                return;

#if UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(0))
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    model.Rotate(touch.deltaPosition.y * velRotate, -touch.deltaPosition.x * velRotate, 0, Space.World);
                }
            }
#endif
        }

        private void OnMouseDrag()
        {
            if (!canRotate)
                return;

#if UNITY_EDITOR
            float axisX = Input.GetAxis("Mouse X");
            float axisY = Input.GetAxis("Mouse Y");

            model.Rotate(axisY * velRotate, axisX * velRotate, 0, Space.World);
#endif
        }
    }
}