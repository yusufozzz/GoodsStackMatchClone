using UnityEngine;

namespace Slots
{
    public abstract class SlotBase<T> : MonoBehaviour
    {
        public bool IsOccupied => Content != null;
        public T Content { get; private set; }
        
        public virtual void Clear()
        {
            if (!IsOccupied)
            {
                Debug.LogWarning("Slot is already empty.");
                return;
            }
            Content = default;
        }
        
        /// <summary>
        /// Sets the content of the slot.
        /// </summary>
        /// <param name="o"></param>
        public virtual void SetContent(T o)
        {
            if (IsOccupied)
            {
                Debug.LogWarning("Slot is already occupied.");
                return;
            }

            if (o == null)
            {
                Debug.LogWarning("Cannot place null content in the slot.");
                return;
            }

            Content = o;
        }
    }
}