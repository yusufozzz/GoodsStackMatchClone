using System.Collections.Generic;
using Boxes;
using DG.Tweening;
using GameManagement;
using GameManagement.Managers;
using Items;
using Slots;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace LevelSystem
{
    public class LevelManager : ManagerBase
    {
        private readonly List<ItemData> _activeBoxItemDataList = new();
        private readonly Dictionary<ItemData, int> _activeItemMap = new();

        /// <summary>
        /// Initializes the level manager and creates the level.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();
            CreateLevel();
        }

        /// <summary>
        /// Creates a new level by calling the level generation logic.
        /// </summary>
        private void CreateLevel()
        {
            GenerateLevel();
        }

        /// <summary>
        /// Generates a complete level with boxes, items, and starts the game.
        /// </summary>
        private void GenerateLevel()
        {
            var boxes = ManagerAccess.Get<SlotManager>().GetActiveBoxes();
            GetRandomBoxItemData(boxes.Count);
            SetUpBoxes(boxes);
            SetItemMap();
            SpawnBoxItems(boxes);
            DOVirtual.DelayedCall(0.5f, StartGame);
        }

        /// <summary>
        /// Transitions the game state to waiting for player input.
        /// </summary>
        private void StartGame()
        {
            var gameStateManager = ManagerAccess.Get<GameStateManager>();
            gameStateManager.SetGameState(GameState.WaitingForInput);
        }

        /// <summary>
        /// Randomly selects item data for each box without duplicates until all items are used.
        /// </summary>
        /// <param name="boxCount">Number of boxes to generate items for</param>
        private void GetRandomBoxItemData(int boxCount)
        {
            var itemData = ItemDataHolder.Instance.ItemData;
            _activeBoxItemDataList.Clear();
            var availableItemData = new List<ItemData>(itemData);

            for (int i = 0; i < boxCount; i++)
            {
                if (availableItemData.Count == 0)
                {
                    availableItemData.AddRange(itemData);
                }

                var randomIndex = Random.Range(0, availableItemData.Count);
                var selectedItemData = availableItemData[randomIndex];
                _activeBoxItemDataList.Add(selectedItemData);

                availableItemData.RemoveAt(randomIndex);
            }
        }

        /// <summary>
        /// Assigns selected item data to each box for setup.
        /// </summary>
        /// <param name="boxes">List of boxes to set up</param>
        private void SetUpBoxes(List<Box> boxes)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                var itemData = _activeBoxItemDataList[i];
                box.SetUp(itemData);
            }
        }

        /// <summary>
        /// Creates a map of item types with their total quantities (6 items per type).
        /// </summary>
        private void SetItemMap()
        {
            foreach (var itemData in _activeBoxItemDataList)
            {
                if (_activeItemMap.TryGetValue(itemData, out var count))
                {
                    _activeItemMap[itemData] = count + 6;
                }
                else
                {
                    _activeItemMap[itemData] = 6;
                }
            }
        }

        /// <summary>
        /// Distributes items to boxes with specific constraints and restarts scene if distribution fails.
        /// </summary>
        /// <param name="boxes">List of boxes to spawn items in</param>
        private void SpawnBoxItems(List<Box> boxes)
        {
            bool success = false;
            int attempts = 0;

            while (!success && attempts < 10)
            {
                attempts++;
                var boxAssignments = new Dictionary<Box, Dictionary<ItemData, int>>();

                foreach (var box in boxes)
                {
                    boxAssignments[box] = new Dictionary<ItemData, int>();
                }

                var itemPairs = new List<ItemData>();
                foreach (var kvp in _activeItemMap)
                {
                    for (int i = 0; i < kvp.Value / 2; i++)
                    {
                        itemPairs.Add(kvp.Key);
                    }
                }

                // Shuffle item pairs
                for (int i = itemPairs.Count - 1; i > 0; i--)
                {
                    var randomIndex = Random.Range(0, i + 1);
                    (itemPairs[i], itemPairs[randomIndex]) = (itemPairs[randomIndex], itemPairs[i]);
                }

                int assignedPairs = 0;
                // Assign items to boxes with constraints: max 2 matching items, max 4 non-matching items per box
                for (int slot = 0; slot < 3; slot++)
                {
                    foreach (var box in boxes)
                    {
                        if (assignedPairs >= itemPairs.Count) break;

                        for (int pairIndex = assignedPairs; pairIndex < itemPairs.Count; pairIndex++)
                        {
                            var candidateItem = itemPairs[pairIndex];
                            var currentCount = boxAssignments[box].GetValueOrDefault(candidateItem, 0);
                            bool isBoxColor = candidateItem == box.ItemData;
                            bool canAssign = isBoxColor ? currentCount + 2 <= 2 : currentCount + 2 <= 4;

                            if (canAssign)
                            {
                                boxAssignments[box][candidateItem] = currentCount + 2;

                                if (pairIndex != assignedPairs)
                                {
                                    (itemPairs[assignedPairs], itemPairs[pairIndex]) =
                                        (itemPairs[pairIndex], itemPairs[assignedPairs]);
                                }

                                assignedPairs++;
                                break;
                            }
                        }
                    }
                }

                if (assignedPairs == itemPairs.Count)
                {
                    success = true;

                    foreach (var kvp in boxAssignments)
                    {
                        var box = kvp.Key;
                        var itemMap = kvp.Value;
                        box.SpawnItem(itemMap);

                        foreach (var itemKvp in itemMap)
                        {
                            _activeItemMap[itemKvp.Key] -= itemKvp.Value;
                            if (_activeItemMap[itemKvp.Key] <= 0)
                            {
                                _activeItemMap.Remove(itemKvp.Key);
                            }
                        }
                    }
                }
            }

            if (!success)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Handles editor input for scene reloading during development.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
#endif
    }
}