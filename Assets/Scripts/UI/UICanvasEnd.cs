using GameManagement.Managers;
using Utility;

namespace UI
{
    public class UICanvasEnd : UICanvas
    {
        public override void OnEnable()
        {
            base.OnEnable();
            GameStateManager.OnLevelComplete += HandleLevelComplete;
            GameStateManager.OnGameOver += HandleGameOver;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            GameStateManager.OnLevelComplete -= HandleLevelComplete;
            GameStateManager.OnGameOver -= HandleGameOver;
        }

        private void HandleLevelComplete()
        {
            var sb = StringBuilderExtensions.Get();
            sb.Append("Level Complete!");
            SetTitle(sb.ToString());
            sb.Release();

            Show();
        }

        private void HandleGameOver()
        {
            var sb = StringBuilderExtensions.Get();
            sb.Append("Game Over!");
            SetTitle(sb.ToString());
            sb.Release();

            Show();
        }
    }
}