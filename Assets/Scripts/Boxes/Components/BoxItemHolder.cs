using System.Collections.Generic;
using System.Linq;
using Items;
using PoolSystem;
using Slots;
using UnityEngine;

namespace Boxes.Components
{
    public class BoxItemHolder : BoxComponent
    {
        public bool IsCompleted => _completedItemCount >= itemSlots.Count;
        public List<Item> DifferentItems => 
            itemSlots.Where(slot => slot.IsOccupied && slot.Content.ItemData != Box.ItemData)
                    .Select(slot => slot.Content).ToList();
        
        public int EmptySlotCount => itemSlots.Count(slot => !slot.IsOccupied);
        
        [SerializeField]
        private List<ItemSlot> itemSlots = new();

        private int _completedItemCount;

        // Initializes the box item holder with the box and its settings.
        public void CreateItems(Dictionary<ItemData, int> itemDataList)
        {
            var index = 0;
            foreach (var itemData in itemDataList)
            {
                var itemCount = itemData.Value;
                for (int i = 0; i < itemCount; i++)
                {
                    var itemSlot = itemSlots[index];
                    var item = PoolAccess.Get<ItemPool>().GetObject();
                    item.SetUp(itemData.Key, itemSlot.transform);
                    itemSlot.SetContent(item);
                    TrySetCompletedItem(item);
                    index++;
                }
            }
        }

        // Removes the specified item from the item slots.
        public void RemoveItem(Item item)
        {
            var slot = itemSlots.FirstOrDefault(s => s.IsOccupied && s.Content == item);
            if (slot != null)
            {
                if (item.ItemData == Box.ItemData)
                {
                    _completedItemCount--;
                }
                slot.Clear();
            }
        }

        // Adds an item to the first available empty item slot.
        public void AddItem(Item item)
        {
            var emptyItemSlot = itemSlots.FirstOrDefault(slot => !slot.IsOccupied);
            if (emptyItemSlot != null)
            {
                emptyItemSlot.SetContent(item);
                TrySetCompletedItem(item);
            }
            else
            {
                Debug.LogWarning("No empty item slot available to add the item.");
            }
        }

        // Checks if the item matches the box's item data and sets it as completed if it does.
        private void TrySetCompletedItem(Item item)
        {
            if (item.ItemData == Box.ItemData)
            {
                _completedItemCount++;
            }
        }
    }
}