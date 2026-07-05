using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Game.Utils.Audio
{
    using Core;
    
    public class AudioPlayService : IAudioPlayer, IInitializable
    {
        private const string AUDIO_PLAYER_PREFAB_PATH = "Audio/AudioPlayer";

        [Inject] private IContentManagementSystem cms;
        private ObjectPool<AudioPlayer> audioPlayerPool;
        private Transform spawnRoot;
        private AudioPlayer prefab;

        void IInitializable.Initialize()
        {
            prefab = cms.LoadContent<AudioPlayer>(AUDIO_PLAYER_PREFAB_PATH);
            spawnRoot = new GameObject("AudioPlayerRoot").transform;
            audioPlayerPool = new(CreateFunc);
        }
        private AudioPlayer CreateFunc()
        {
            var spawned = Object.Instantiate(prefab, spawnRoot);
            spawned.Initialize(ReturnToPool);
            return spawned;
        }
        private void ReturnToPool(AudioPlayer audioPlayer) => audioPlayerPool.Release(audioPlayer);

        void IAudioPlayer.Play(AudioPlayInfo audioPlayInfo) => audioPlayerPool.Get().PlayAudio(audioPlayInfo);
    }
}