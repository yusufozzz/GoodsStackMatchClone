using System.Collections.Generic;
using Items;

namespace Slots
{
    public sealed class SlotOperationsHandler
    {
        private readonly List<ItemSlot> _cachedEmptyItemSlots;
        private readonly List<ItemSlot> _cachedOccupiedItemSlots;
        private readonly List<BoxSlot> _cachedCompletedBoxSlots;
        private readonly List<Item> _cachedDifferentItems;

        private BoxSlot[] _boxSlotsArray;
        private ItemSlot[] _itemSlotsArray;
        private int _boxSlotsCount;
        private int _itemSlotsCount;

        /// <summary>
        /// Initializes cached lists with predefined capacities for performance optimization.
        /// </summary>
        public SlotOperationsHandler()
        {
            _cachedEmptyItemSlots = new List<ItemSlot>(32);
            _cachedOccupiedItemSlots = new List<ItemSlot>(32);
            _cachedCompletedBoxSlots = new List<BoxSlot>(32);
            _cachedDifferentItems = new List<Item>(16);
        }
        
        /// <summary>
        /// Sets up slot arrays and caches their lengths for performance.
        /// </summary>
        /// <param name="boxSlots">Array of box slots to operate on</param>
        /// <param name="itemSlots">Array of item slots to operate on</param>
        public void Initialize(BoxSlot[] boxSlots, ItemSlot[] itemSlots)
        {
            _boxSlotsArray = boxSlots;
            _itemSlotsArray = itemSlots;
            _boxSlotsCount = boxSlots.Length;
            _itemSlotsCount = itemSlots.Length;
        }

        /// <summary>
        /// Moves different items from boxes to available item slots for sorting.
        /// </summary>
        /// <returns>True if any items were moved, false otherwise</returns>
        public bool MoveItemsFromBoxesToSlots()
        {
            var changesMade = false;
            var emptyItemSlotsCount = 0;

            // Cache all empty item slots
            _cachedEmptyItemSlots.Clear();
            for (var i = 0; i < _itemSlotsCount; ++i)
            {
                var itemSlot = _itemSlotsArray[i];
                if (!itemSlot.IsOccupied)
                {
                    _cachedEmptyItemSlots.Add(itemSlot);
                    ++emptyItemSlotsCount;
                }
            }

            if (emptyItemSlotsCount == 0) return false;

            // Process each box and move different items to slots
            for (var i = 0; i < _boxSlotsCount; ++i)
            {
                var boxSlot = _boxSlotsArray[i];
                if (!boxSlot.IsOccupied) continue;

                var box = boxSlot.Content;
                if (box.IsCompleted) continue;

                _cachedDifferentItems.Clear();
                var boxDifferentItems = box.GetDifferentItems();
                var differentItemsCount = boxDifferentItems.Count;

                if (differentItemsCount == 0) continue;

                _cachedDifferentItems.AddRange(boxDifferentItems);

                var maxItemsToMove = emptyItemSlotsCount < differentItemsCount
                    ? emptyItemSlotsCount
                    : differentItemsCount;

                // Move items from box to available slots
                for (var j = differentItemsCount - 1; j >= differentItemsCount - maxItemsToMove; --j)
                {
                    var item = _cachedDifferentItems[j];
                    var itemSlot = _cachedEmptyItemSlots[--emptyItemSlotsCount];

                    itemSlot.SetContent(item);
                    box.RemoveItem(item);
                    changesMade = true;

                    if (emptyItemSlotsCount == 0) break;
                }

                if (emptyItemSlotsCount == 0) break;
            }

            return changesMade;
        }

        /// <summary>
        /// Moves items from slots back to boxes where they can be accepted.
        /// </summary>
        /// <returns>True if any items were moved, false otherwise</returns>
        public bool MoveItemsFromSlotsToBoxes()
        {
            var changesMade = false;
            var occupiedItemSlotsCount = 0;

            // Cache all occupied item slots
            _cachedOccupiedItemSlots.Clear();
            for (var i = 0; i < _itemSlotsCount; ++i)
            {
                var itemSlot = _itemSlotsArray[i];
                if (itemSlot.IsOccupied)
                {
                    _cachedOccupiedItemSlots.Add(itemSlot);
                    ++occupiedItemSlotsCount;
                }
            }

            if (occupiedItemSlotsCount == 0) return false;

            // Try to place each item from slots into compatible boxes
            for (var i = 0; i < occupiedItemSlotsCount; ++i)
            {
                var itemSlot = _cachedOccupiedItemSlots[i];
                var item = itemSlot.Content;

                for (var j = 0; j < _boxSlotsCount; ++j)
                {
                    var boxSlot = _boxSlotsArray[j];
                    if (!boxSlot.IsOccupied) continue;

                    var box = boxSlot.Content;
                    if (box.IsCompleted) continue;

                    if (box.CanAcceptItem(item))
                    {
                        box.AddItem(item);
                        itemSlot.Clear();
                        changesMade = true;
                        break;
                    }
                }
            }

            return changesMade;
        }

        /// <summary>
        /// Removes completed boxes from their slots and triggers their quit sequence.
        /// </summary>
        /// <returns>True if any boxes were destroyed, false otherwise</returns>
        public bool DestroyCompletedBoxes()
        {
            var completedBoxSlotsCount = 0;

            // Cache all slots with completed boxes
            _cachedCompletedBoxSlots.Clear();
            for (var i = 0; i < _boxSlotsCount; ++i)
            {
                var boxSlot = _boxSlotsArray[i];
                if (boxSlot.IsOccupied && boxSlot.Content.IsCompleted)
                {
                    _cachedCompletedBoxSlots.Add(boxSlot);
                    ++completedBoxSlotsCount;
                }
            }

            if (completedBoxSlotsCount == 0) return false;

            // Remove completed boxes and trigger their quit sequence
            for (var i = 0; i < completedBoxSlotsCount; ++i)
            {
                var boxSlot = _cachedCompletedBoxSlots[i];
                var box = boxSlot.Content;
                boxSlot.Clear();
                box.QuitGame();
            }

            return true;
        }
    }
}