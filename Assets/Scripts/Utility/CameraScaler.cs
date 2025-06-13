using System;
using UnityEngine;

namespace Utility
{
    [DefaultExecutionOrder(-100)]
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField]
        private float referenceWidth = 1080f;

        [SerializeField]
        private float referenceHeight = 1920f;

        [SerializeField]
        private float referenceSize = 19f;

        private Camera _cachedCamera;

        private void Start()
        {
            _cachedCamera = Camera.main;
            ApplyScale();
        }

        private void ApplyScale()
        {
            float currentAspect = (float)Screen.width / Screen.height;
            float referenceAspect = referenceWidth / referenceHeight;

            float scaleFactor = referenceAspect / currentAspect;
            float size = referenceSize * scaleFactor;

            _cachedCamera.orthographicSize = size;
        }
    }
}