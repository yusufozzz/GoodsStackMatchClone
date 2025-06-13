using GameManagement.Managers;
using PoolSystem;
using UnityEngine;

namespace Particles
{
    public class ParticleManager : ManagerBase
    {
        public void PlayParticle(ParticleType particleType, Vector3 position, Vector3 rotation, Color? color = null)
        {
            var particle = PoolAccess.Get<ParticlePool>().GetObject((int)particleType);
            if (particle == null) return;

            if (color.HasValue)
            {
                var main = particle.main;
                main.startColor = color.Value;
            }

            particle.transform.position = position;
            particle.transform.rotation = Quaternion.Euler(rotation);
            var particleReturner = TrySetParticleReturner(particle);
            particleReturner.Init(() => PoolAccess.Get<ParticlePool>().ReturnObject(particle));
            particle.Play();
        }

        private static ParticleReturner TrySetParticleReturner(ParticleSystem particle)
        {
            var particleReturner = particle.GetComponent<ParticleReturner>();

            if (particleReturner == null)
                particleReturner = particle.gameObject.AddComponent<ParticleReturner>();
            return particleReturner;
        }
    }
}