using DG.Tweening;
using GameManagement;
using GameManagement.Managers;
using Particles;
using Slots;
using UnityEngine;

namespace Boxes.Components
{
    public class BoxMovement : BoxComponent
    {
        private Transform _cachedTransform;
        private SlotManager _slotManager;
        private GameStateManager _gameStateManager;
        private bool _isMoving;
        private Sequence _quitGameTween;

        private void Awake()
        {
            _cachedTransform = transform;
            _slotManager = ManagerAccess.Get<SlotManager>();
            _gameStateManager = ManagerAccess.Get<GameStateManager>();
        }

        private void OnMouseDown()
        {
            if (_gameStateManager.CurrentState != GameState.WaitingForInput)
                return;

            if (!Box.CanMove || _isMoving || _slotManager == null)
                return;

            var targetSlot = _slotManager.GetFirstEmptyBoxSlot();
            if (targetSlot == null)
                return;

            MoveToSlot(targetSlot);
        }

        /// Moves the box to the target slot with a jump animation.
        private void MoveToSlot(BoxSlot targetSlot)
        {
            _isMoving = true;
            SetSortOrder(BoxSettings.JumpSortOrder);

            var targetPos = targetSlot.transform.position;
            _gameStateManager.SetGameState(GameState.BoxMoved);

            _cachedTransform.DOJump(
                targetPos,
                BoxSettings.JumpPower,
                1,
                BoxSettings.JumpDuration
            ).OnComplete(() =>
            {
                _isMoving = false;
                SetSortOrder(BoxSettings.DefaultSortOrder);
                Box.PlaceToSlot(targetSlot);
            });
        }

        /// Sets the sorting order of the box to control rendering order.
        private void SetSortOrder(int order)
        {
            Box.SetSortOrder(order);
        }

        /// Quits the game with a jump animation and plays a particle effect.
        public void QuitGame()
        {
            _quitGameTween = DOTween.Sequence();
            var upTarget = _cachedTransform.position + Vector3.up * BoxSettings.QuitJumpHeight;
            _quitGameTween
                .AppendCallback(CompleteParticle)
                .Append(
                    _cachedTransform.DOMoveY(upTarget.y, BoxSettings.QuitUpDuration).SetEase(BoxSettings.QuitUpEase))
                .Join(_cachedTransform.DORotate(
                    BoxSettings.RotateVector,
                    BoxSettings.QuitUpDuration,
                    RotateMode.FastBeyond360
                ))
                .Append(_cachedTransform.DOJump(
                    Vector3.left * BoxSettings.BoxQuitDistanceMultiplier,
                    BoxSettings.JumpPower,
                    1,
                    BoxSettings.QuitUpDuration
                ).SetEase(BoxSettings.QuitUpEase))
                .AppendCallback(() =>
                {
                    DOVirtual.DelayedCall(BoxSettings.QuitUpDuration, () =>
                    {
                        _quitGameTween?.Kill();
                        _quitGameTween = null;
                        Destroy(gameObject);
                    }).SetLink(gameObject);
                });
        }

        /// Completes the particle effect when the box is successfully moved or quit.
        private void CompleteParticle()
        {
            var particlePosition = _cachedTransform.position;
            particlePosition.y += 0.5f;
            ManagerAccess.Get<ParticleManager>()
                .PlayParticle(ParticleType.BoxCompleted, particlePosition, Vector3.zero);
        }

        private void OnDestroy()
        {
            _cachedTransform?.DOKill();
            _quitGameTween?.Kill();
        }
    }
}