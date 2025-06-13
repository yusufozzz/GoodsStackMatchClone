using System;
using UnityEngine;

namespace Particles
{
    public class ParticleReturner: MonoBehaviour
    {
        private Action _onDisable;
        public void Init(Action onDisable)
        {
            _onDisable = onDisable;
        }

        private void OnDisable()
        {
            _onDisable?.Invoke();
        }
    }
}