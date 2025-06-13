using System;
using UnityEngine;

namespace UI
{
    [Serializable]
    public class UISettings
    {
        [field: SerializeField]
        public float CanvasShowHideDuration { get; private set; } = 0.25f;
    }
}