using System.Collections.Generic;
using Boxes;
using GameManagement;
using GameManagement.Managers;
using UnityEngine;

namespace Slots
{
    public sealed class SlotManager : ManagerBase
    {
        [SerializeField]
        private SlotSpawner slotSpawner;
        
        private BoxSlot[] _boxSlotsArray;
        private ItemSlot[] _itemSlotsArray;
        private int _boxSlotsCount;
        private int _itemSlotsCount;

        private SlotOperationsHandler _operationsHandler;
        private SlotGameStateValidator _gameStateValidator;
        private SlotOperationScheduler _operationScheduler;
        private GameStateManager _gameStateManager;
        
        private int _completedBoxCount;

        /// <summary>
        /// Initializes the slot manager and sets up all slot-related components.
        /// </summary>
        public override void SetUp()
        {
            base.SetUp();
            InitializeComponents();
            CacheSlotArrays();
            SetCasualBoxes();
        }

        /// <summary>
        /// Initializes all handler components and subscribes to operation events.
        /// </summary>
        private void InitializeComponents()
        {
            slotSpawner.SetUp();
            _operationsHandler = new SlotOperationsHandler();
            _gameStateValidator = new SlotGameStateValidator();
            _operationScheduler = new SlotOperationScheduler();
            _gameStateManager = ManagerAccess.Get<GameStateManager>();
            _operationScheduler.OnGameOver += GameOver;
            _operationScheduler.OnOperationsFinished += OperationFinish;
        }

        /// <summary>
        /// Updates game state after operations complete and checks for level completion.
        /// </summary>
        private void OperationFinish()
        {
            _gameStateManager.SetGameState(GameState.WaitingForInput);
            if (_completedBoxCount >= slotSpawner.ActiveBoxes.Count)
            {
                _gameStateManager.SetGameState(GameState.LevelComplete);
            }
        }

        /// <summary>
        /// Converts slot lists to arrays for performance and initializes operation components.
        /// </summary>
        private void CacheSlotArrays()
        {
            var boxSlotsList = slotSpawner.BoxSlots;
            var itemSlotsList = slotSpawner.ItemSlots;
            
            _boxSlotsCount = boxSlotsList.Count;
            _itemSlotsCount = itemSlotsList.Count;
            
            _boxSlotsArray = new BoxSlot[_boxSlotsCount];
            _itemSlotsArray = new ItemSlot[_itemSlotsCount];
            
            // Convert lists to arrays for better performance
            for (var i = 0; i < _boxSlotsCount; ++i)
                _boxSlotsArray[i] = boxSlotsList[i];
                
            for (var i = 0; i < _itemSlotsCount; ++i)
                _itemSlotsArray[i] = itemSlotsList[i];

            _operationsHandler.Initialize(_boxSlotsArray, _itemSlotsArray);
            _gameStateValidator.Initialize(_boxSlotsArray);
            _operationScheduler.Initialize(this, _operationsHandler, _gameStateValidator);
        }

        /// <summary>
        /// Subscribes to placement events for all active boxes in the game.
        /// </summary>
        private void SetCasualBoxes()
        {
            var activeBoxes = slotSpawner.ActiveBoxes;
            var activeBoxCount = activeBoxes.Count;
            
            for (var i = 0; i < activeBoxCount; ++i)
            {
                activeBoxes[i].OnPlaced += HandleOperations;
            }
        }

        /// <summary>
        /// Handles box placement by incrementing counter and starting slot operations.
        /// </summary>
        private void HandleOperations()
        {
            _completedBoxCount++;
            _gameStateManager.SetGameState(GameState.SlotOperations);
            _operationScheduler.StartOperations();
        }

        /// <summary>
        /// Transitions the game to game over state when operations fail.
        /// </summary>
        private void GameOver()
        {
            _gameStateManager.SetGameState(GameState.GameOver);
        }

        /// <summary>
        /// Returns the list of active boxes from the slot spawner.
        /// </summary>
        /// <returns>List of active boxes in the game</returns>
        public List<Box> GetActiveBoxes()
        {
            return slotSpawner.ActiveBoxes;
        }
        
        /// <summary>
        /// Finds and returns the first available empty box slot.
        /// </summary>
        /// <returns>First empty box slot or null if none available</returns>
        public BoxSlot GetFirstEmptyBoxSlot()
        {
            for (var i = 0; i < _boxSlotsCount; ++i)
            {
                if (!_boxSlotsArray[i].IsOccupied)
                    return _boxSlotsArray[i];
            }
            return null;
        }

        /// <summary>
        /// Unsubscribes from operation events when the component is disabled.
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            _operationScheduler.OnGameOver -= GameOver;
            _operationScheduler.OnOperationsFinished -= OperationFinish;
        }
    }
}