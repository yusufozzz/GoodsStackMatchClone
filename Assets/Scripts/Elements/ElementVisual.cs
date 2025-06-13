using UnityEngine;

namespace Elements
{
    public class ElementVisual: MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer mainSprite;

        // Sets the color of the main sprite renderer to the specified color.
        public void SetColor(Color color)
        {
            if (mainSprite != null)
            {
                mainSprite.color = color;
            }
            else
            {
                Debug.LogWarning("Main sprite renderer is not assigned.");
            }
        }
    }
}