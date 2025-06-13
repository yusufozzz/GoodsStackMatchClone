using System;
using Slots;
using System.Collections.Generic;
using Boxes.Components;
using DG.Tweening;
using GameManagement;
using Items;
using Particles;
using UnityEngine;

namespace Boxes
{
    public class Box : MonoBehaviour
    {
        public event Action OnPlaced;
        public ItemData ItemData { get; private set; }
        public bool IsPlaced { get; private set; }
        public bool CanMove { get; private set; } = true;
        public bool IsCompleted => _boxItemHolder.IsCompleted;

        private BoxMovement _boxMovement;
        private BoxVisual _boxVisual;
        private BoxItemHolder _boxItemHolder;

        public void SetUp(ItemData boxItemData)
        {
            ItemData = boxItemData;
            IsPlaced = true;
            SetComponents();
        }

        // Initializes the box with the provided item data and sets up its components.
        private void SetComponents()
        {
            _boxItemHolder = GetComponent<BoxItemHolder>();
            _boxItemHolder.Initialize(this);
            _boxVisual = GetComponent<BoxVisual>();
            _boxVisual.Initialize(this);
            _boxMovement = GetComponent<BoxMovement>();
            _boxMovement.Initialize(this);
        }

        // Spawns items in the box based on the provided item data list from LevelManager.
        public void SpawnItem(Dictionary<ItemData, int> itemDataList)
        {
            _boxItemHolder.CreateItems(itemDataList);
        }

        // Places the box into the specified slot and updates its state.
        public void PlaceToSlot(BoxSlot boxSlot)
        {
            PlayBoxPlacedParticle();
            IsPlaced = true;
            CanMove = false;
            transform.SetParent(boxSlot.transform);
            transform.localPosition = Vector3.zero;
            boxSlot.SetContent(this);
            OnPlaced?.Invoke();
        }

        // Plays a particle effect at the box's position when it is placed in a slot.
        private void PlayBoxPlacedParticle()
        {
            var particlePosition = transform.position + Vector3.up * 0.5f;
            ManagerAccess.Get<ParticleManager>().PlayParticle(ParticleType.BoxPlaced, particlePosition, Vector3.right * 90);
        }

        // Sets the sorting order of the box to control rendering order.
        public void SetSortOrder(int order)
        {
            _boxVisual.SetSortOrder(order);
        }

        // Returns a list of different items in the box that do not match the box's item data.
        public List<Item> GetDifferentItems()
        {
            return _boxItemHolder.DifferentItems;
        }

        // Removes item from box
        public void RemoveItem(Item item)
        {
            _boxItemHolder.RemoveItem(item);
        }

        // Checks if the box can accept the specified item based on its item data and available slots.
        public bool CanAcceptItem(Item item)
        {
            return item.ItemData == ItemData && _boxItemHolder.EmptySlotCount > 0;
        }

        // Add item to box
        public void AddItem(Item item)
        {
            _boxItemHolder.AddItem(item);
        }

        // Sends the box to quit position
        public void QuitGame()
        {
            _boxMovement.QuitGame();
        }
    }
}