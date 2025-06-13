using System.Collections.Generic;
using Boxes;
using GameManagement;
using UnityEngine;

namespace Slots
{
    public class SlotSpawner : MonoBehaviour
    {
        [SerializeField]
        private float padding = 0.5f;

        [SerializeField]
        private SpriteRenderer background;

        [Header("Configs")]
        [SerializeField]
        private SlotConfig<ItemSlot> emptyItemSlotConfig;

        [SerializeField]
        private SlotConfig<BoxSlot> emptyBoxSlotConfig;

        [SerializeField]
        private SlotConfig<Box> activeBoxConfig;

        public List<ItemSlot> ItemSlots { get; private set; } = new();
        public List<BoxSlot> BoxSlots { get; private set; } = new();
        public List<Box> ActiveBoxes { get; private set; } = new();

        /// <summary>
        /// Initializes all slot types and creates the game layout.
        /// </summary>
        public void SetUp()
        {
            CreateBoxSlots();
            CreateItemSlots();
            CreateActiveBoxSlots();
        }

        /// <summary>
        /// Creates box slots and adjusts background size accordingly.
        /// </summary>
        private void CreateBoxSlots()
        {
            var boxSlotCount = GameSettings.Instance.BoxSlotCount;
            CreateSingleRow(emptyBoxSlotConfig, boxSlotCount, 0, BoxSlots);
            SetBoxSlotBackgroundSize(emptyBoxSlotConfig, boxSlotCount);
        }

        /// <summary>
        /// Dynamically adjusts background sprite size based on box slot count and spacing.
        /// </summary>
        /// <param name="slotConfig">Configuration for box slots</param>
        /// <param name="boxSlotCount">Number of box slots to accommodate</param>
        private void SetBoxSlotBackgroundSize(SlotConfig<BoxSlot> slotConfig, int boxSlotCount)
        {
            if (background == null) return;
            float width = boxSlotCount * slotConfig.Spacing + padding;

            background.size = new Vector2(width, background.size.y);
        }

        /// <summary>
        /// Creates a single row of slots with specified parameters.
        /// </summary>
        /// <param name="config">Slot configuration containing prefab and spacing</param>
        /// <param name="count">Number of slots to create</param>
        /// <param name="yPosition">Y position for the row</param>
        /// <param name="list">List to add created slots to</param>
        private void CreateSingleRow<T>(SlotConfig<T> config, int count, float yPosition, List<T> list)
            where T : MonoBehaviour
        {
            var startX = -(count - 1) * config.Spacing * padding;

            for (var i = 0; i < count; i++)
            {
                var instance = Instantiate(config.Prefab, config.Parent);
                var position = new Vector3(startX + i * config.Spacing, 0, yPosition);
                instance.transform.localPosition = position;

                list.Add(instance);
            }
        }

        /// <summary>
        /// Creates item slots in two rows with balanced distribution.
        /// </summary>
        private void CreateItemSlots()
        {
            var itemSlotCount = GameSettings.Instance.ItemSlotCount;
            var topRowCount = Mathf.CeilToInt(itemSlotCount * padding);
            var bottomRowCount = itemSlotCount - topRowCount;

            CreateRowWithIndex(emptyItemSlotConfig, topRowCount, 0, emptyItemSlotConfig.RowSpacing * padding,
                ItemSlots);
            CreateRowWithIndex(emptyItemSlotConfig, bottomRowCount, topRowCount,
                -emptyItemSlotConfig.RowSpacing * padding,
                ItemSlots);
        }

        /// <summary>
        /// Creates a row with index tracking for sequential slot numbering.
        /// </summary>
        /// <param name="config">Slot configuration containing prefab and spacing</param>
        /// <param name="count">Number of slots to create in this row</param>
        /// <param name="startIndex">Starting index for slot numbering</param>
        /// <param name="yPosition">Y position for the row</param>
        /// <param name="list">List to add created slots to</param>
        private void CreateRowWithIndex<T>(SlotConfig<T> config, int count, int startIndex, float yPosition,
            List<T> list) where T : MonoBehaviour
        {
            if (count <= 0) return;

            var startX = -(count - 1) * config.Spacing * padding;

            for (var i = 0; i < count; i++)
            {
                var instance = Instantiate(config.Prefab, config.Parent);
                var position = new Vector3(startX + i * config.Spacing, 0, yPosition);
                instance.transform.localPosition = position;

                list.Add(instance);
            }
        }

        /// <summary>
        /// Creates active box slots in two rows for better organization.
        /// </summary>
        private void CreateActiveBoxSlots()
        {
            var boxCount = GameSettings.Instance.BoxCount;
            var topRowCount = Mathf.CeilToInt(boxCount * padding);
            var bottomRowCount = boxCount - topRowCount;

            CreateRow(activeBoxConfig, topRowCount, activeBoxConfig.RowSpacing * padding, ActiveBoxes);
            CreateRow(activeBoxConfig, bottomRowCount, -activeBoxConfig.RowSpacing * padding, ActiveBoxes);
        }

        /// <summary>
        /// Helper method to create a row without index tracking.
        /// </summary>
        /// <param name="config">Slot configuration containing prefab and spacing</param>
        /// <param name="count">Number of slots to create</param>
        /// <param name="yPosition">Y position for the row</param>
        /// <param name="list">List to add created slots to</param>
        private void CreateRow<T>(SlotConfig<T> config, int count, float yPosition, List<T> list)
            where T : MonoBehaviour
        {
            if (count <= 0) return;
            CreateSingleRow(config, count, yPosition, list);
        }
    }
}