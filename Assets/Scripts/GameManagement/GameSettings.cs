using Boxes;
using UI;
using UnityEngine;
using Utility;

namespace GameManagement
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Management/Game Settings", order = 1)]
    public class GameSettings : ScriptableSingletonMono<GameSettings>
    {
        [field: SerializeField]
        public int BoxSlotCount { get; private set; } = 4;

        [field: SerializeField]
        public int ItemSlotCount { get; private set; } = 15;

        [field: SerializeField]
        public int BoxCount { get; private set; } = 9;
        
        [field: SerializeField]
        public float OperationDuration { get; private set; } = 0.25f;

        [field: SerializeField]
        public BoxSettings BoxSettings { get; private set; }

        [field: SerializeField]
        public UISettings UISettings { get; private set; }
    }
}