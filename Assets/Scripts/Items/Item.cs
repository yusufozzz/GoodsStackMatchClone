using DG.Tweening;
using Elements;
using GameManagement;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private ElementVisual elementVisual;
        public ItemData ItemData { get; private set; }

        private Tween _itemMovementTween;
        private float _itemMovementDuration;
        private bool _isCompleted;
        
        // Sets up the item with the provided ItemData and parent transform.
        public void SetUp(ItemData itemData, Transform parent)
        {
            ItemData = itemData;
            elementVisual.SetColor(ItemData.Color);
            _itemMovementDuration = GameSettings.Instance.OperationDuration;
            SetTransform(parent);
        }
        
        // Sets the transform of the item to the specified parent and resets its local rotation.
        public void SetTransform(Transform parent)
        {
            Transform t;
            (t = transform).SetParent(parent);
            t.localRotation = Quaternion.identity;
            _itemMovementTween?.Kill();
            _itemMovementTween = t.DOLocalJump(Vector3.zero, 3f, 1, _itemMovementDuration)
                .OnComplete(() => _itemMovementTween = null);
        }
    }
}