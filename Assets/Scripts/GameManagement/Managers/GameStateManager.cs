using System;

namespace GameManagement.Managers
{
    public class GameStateManager: ManagerBase
    {
        public static event Action OnLevelComplete;
        public static event Action OnGameOver;

        public GameState CurrentState { get; private set; }

        public override void SetUp()
        {
            base.SetUp();
            SetGameState(GameState.Initializing);
        }

        // Sets the game state and triggers corresponding events if the state changes.
        public void SetGameState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;

            switch (newState)
            {
                case GameState.LevelComplete:
                    OnLevelComplete?.Invoke();
                    break;
                case GameState.GameOver:
                    OnGameOver?.Invoke();
                    break;
            }
        }
    }
}