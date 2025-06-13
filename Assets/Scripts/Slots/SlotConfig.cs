using UnityEngine;

namespace Slots
{
    [System.Serializable]
    public class SlotConfig<T> where T : MonoBehaviour
    {
        [field: SerializeField]
        public Transform Parent { get; private set; }

        [field: SerializeField]
        public T Prefab { get; private set; }

        [field: SerializeField]
        public float Spacing { get; private set; }

        [field: SerializeField]
        public float RowSpacing { get; private set; }
    }
}