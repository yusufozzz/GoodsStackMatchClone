using GameManagement;
using UnityEngine;

namespace Boxes.Components
{
    public class BoxComponent: MonoBehaviour
    {
        protected Box Box;
        protected BoxSettings BoxSettings;

        // This method is called to initialize the component with the box and its settings.
        public virtual void Initialize(Box box)
        {
            BoxSettings = GameSettings.Instance.BoxSettings;
            Box = box;
        }
    }
}