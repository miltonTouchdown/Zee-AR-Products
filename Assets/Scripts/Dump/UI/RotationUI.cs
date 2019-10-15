using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trophies.PackagesApps
{
    public class RotationUI : MonoBehaviour
    {
        public float velRotation = 1.5f;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            _rectTransform.Rotate(Vector3.back * velRotation);
        }
    }
}