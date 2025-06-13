using UnityEngine.SceneManagement;

namespace UI
{
    public class RestartButton: UIButton
    {
        protected override void OnClick()
        {
            base.OnClick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}