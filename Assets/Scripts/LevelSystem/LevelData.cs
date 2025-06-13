using UnityEngine;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level System/Level Data")]
    public class LevelData: ScriptableObject
    {
        [field: SerializeField]
        public int BoxSlotCount { get; private set; } = 4;
    }
}