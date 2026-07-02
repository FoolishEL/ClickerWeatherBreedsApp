using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace Game.Utils.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        private Action<AudioPlayer> onCompleteCallback;
        private CancellationTokenSource cts;
        
        public void Initialize(Action<AudioPlayer> onComplete)
        {
            onCompleteCallback = onComplete;
        }
        
        public void PlayAudio(AudioPlayInfo audioPlayInfo)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            audioSource.volume = audioPlayInfo.Volume;
            audioSource.pitch = Random.Range(audioPlayInfo.Pitch.x, audioPlayInfo.Pitch.y);
            
            audioSource.PlayOneShot(audioPlayInfo.Clip);
            
            PlayAndReturnToPoolAsync(cts.Token).Forget();
        }

        private async UniTask PlayAndReturnToPoolAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                
                while (audioSource.isPlaying && !token.IsCancellationRequested)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
                if (!token.IsCancellationRequested)
                {
                    onCompleteCallback?.Invoke(this);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
        
        public void Stop()
        {
            cts?.Cancel();
            audioSource.Stop();
        }

        private void OnDisable()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = null;
        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}