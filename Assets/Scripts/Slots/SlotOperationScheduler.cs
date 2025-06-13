using System;
using System.Collections;
using GameManagement;
using UnityEngine;
using Utility;

namespace Slots
{
    public sealed class SlotOperationScheduler
    {
        private WaitForSeconds _operationDelayWait;
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();

        private Coroutine _operationsCoroutine;
        private MonoBehaviour _coroutineRunner;

        private SlotOperationsHandler _operationsHandler;
        private SlotGameStateValidator _gameStateValidator;

        public event Action OnGameOver;
        public event Action OnOperationsFinished;

        /// <summary>
        /// Initializes the scheduler with required components and sets up operation timing.
        /// </summary>
        /// <param name="coroutineRunner">MonoBehaviour to run coroutines</param>
        /// <param name="operationsHandler">Handler for slot operations</param>
        /// <param name="gameStateValidator">Validator for game state checks</param>
        public void Initialize(MonoBehaviour coroutineRunner, SlotOperationsHandler operationsHandler,
            SlotGameStateValidator gameStateValidator)
        {
            _coroutineRunner = coroutineRunner;
            _operationsHandler = operationsHandler;
            _gameStateValidator = gameStateValidator;
            _operationDelayWait = CoroutineExtensions.WaitForSeconds(GameSettings.Instance.OperationDuration);
        }

        /// <summary>
        /// Starts the slot operations sequence, stopping any existing operations first.
        /// </summary>
        public void StartOperations()
        {
            if (_operationsCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_operationsCoroutine);
            }

            _operationsCoroutine = _coroutineRunner.StartCoroutine(ExecuteOperationsCoroutine());
        }

        /// <summary>
        /// Executes slot operations in sequence until no changes occur or game ends.
        /// </summary>
        /// <returns>Coroutine enumerator for operation execution</returns>
        private IEnumerator ExecuteOperationsCoroutine()
        {
            yield return _waitForEndOfFrame;

            while (true)
            {
                var anyChangesMade = false;

                // Move items from boxes to available slots
                var moveItemsFromBoxesToSlots = _operationsHandler.MoveItemsFromBoxesToSlots();
                anyChangesMade |= moveItemsFromBoxesToSlots;

                if (moveItemsFromBoxesToSlots)
                {
                    // Move items from slots back to matching boxes
                    yield return _operationDelayWait;
                    var moveItemsFromSlotsToBoxes = _operationsHandler.MoveItemsFromSlotsToBoxes();
                    anyChangesMade |= moveItemsFromSlotsToBoxes;

                    if (moveItemsFromSlotsToBoxes)
                    {
                        // Remove completed boxes from the game
                        yield return _operationDelayWait;
                        anyChangesMade |= _operationsHandler.DestroyCompletedBoxes();
                    }
                }
                
                // If no changes were made, check if the game is over or level completed
                if (!anyChangesMade)
                {
                    if (_gameStateValidator.IsGameOver())
                    {
                        OnGameOver?.Invoke();
                    }
                    
                    // If the game is not over, we can consider operations finished
                    OnOperationsFinished?.Invoke();
                    break;
                }

                yield return _waitForEndOfFrame;
            }

            _operationsCoroutine = null;
        }
    }
}