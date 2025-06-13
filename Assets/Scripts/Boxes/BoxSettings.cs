using System;
using DG.Tweening;
using UnityEngine;

namespace Boxes
{
    [Serializable]
    public class BoxSettings
    {
        [field: SerializeField]
        public int JumpSortOrder { get; private set; } = 9;

        [field: SerializeField]
        public int DefaultSortOrder { get; private set; } = 5;

        [field: SerializeField]
        public float JumpPower { get; private set; } = 5f;

        [field: SerializeField]
        public float JumpDuration { get; private set; } = 0.5f;

        [field: SerializeField]
        public float QuitJumpHeight { get; private set; } = 2f;

        [field: SerializeField]
        public float QuitUpDuration { get; private set; } = 0.5f;

        [field: SerializeField]
        public Ease QuitUpEase { get; private set; } = Ease.OutQuad;

        [field: SerializeField]
        public Vector3 RotateVector { get; private set; } = new Vector3(90, 360, 0);

        [field: SerializeField]
        public float BoxQuitDistanceMultiplier { get; private set; } = 10f;
    }
}