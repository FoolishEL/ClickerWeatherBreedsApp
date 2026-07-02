using UnityEngine;

namespace Game.Utils.UI
{
    public class ButtonParticles : MonoBehaviour
    {
        [SerializeField] private ParticlesInfo[] particlesInfos;

        public void PlayParticles()
        {
            foreach (var particlesInfo in particlesInfos)
                particlesInfo.Emmit();
        }
        
        [System.Serializable]
        private class ParticlesInfo
        {
            [SerializeField] private ParticleSystem particleSystem;
            [SerializeField] private int particlesCount;

            public void Emmit() => particleSystem.Emit(particlesCount);
        }
    }
}