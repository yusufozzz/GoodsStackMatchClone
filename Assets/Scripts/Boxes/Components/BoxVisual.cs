using Elements;
using UnityEngine;
using UnityEngine.Rendering;

namespace Boxes.Components
{
    public class BoxVisual: BoxComponent
    {
        [SerializeField]
        private ElementVisual elementVisual;

        [SerializeField]
        private SortingGroup sortingGroup;

        public override void Initialize(Box box)
        {
            base.Initialize(box);
            SetVisual(Box.ItemData.Color);
        }

        /// Sets the visual representation of the box using the specified color.
        private void SetVisual(Color color)
        {
            if (elementVisual != null)
            {
                elementVisual.SetColor(color);
            }
            else
            {
                Debug.LogWarning("ElementVisual is not assigned.");
            }
        }

        // Sets the sorting order of the box to control rendering order.
        public void SetSortOrder(int order)
        {
            if (sortingGroup != null || sortingGroup.sortingOrder != order)
            {
                sortingGroup.sortingOrder = order;
            }
            else
            {
                Debug.LogWarning("SortingGroup is not assigned.");
            }
        }
    }
}