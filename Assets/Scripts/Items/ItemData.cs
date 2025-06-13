using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 1)]
    public class ItemData: ScriptableObject
    {
        [field: SerializeField]
        public Color Color { get; private set; } = Color.white;
    }
}