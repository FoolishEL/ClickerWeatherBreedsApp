using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Game.Utils.UI
{
    public class ButtonScaleOnPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float pressedScale = 0.85f;
        [SerializeField] private float duration = 0.1f;
        [SerializeField] private Transform target;
        [SerializeField] private AnimationCurve scaleCurve;

        private Vector3 originalScale;
        private CancellationTokenSource cts;

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateTo(pressedScale).Forget();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            AnimateTo(1f).Forget();
        }

        private async UniTaskVoid AnimateTo(float targetMultiplier)
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();

            var token = cts.Token;

            Vector3 targetScale = originalScale * targetMultiplier;
            Vector3 startScale = target.localScale;

            float elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    token.ThrowIfCancellationRequested();

                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / duration);
                    var scale = scaleCurve.Evaluate(t);
                    target.localScale = Vector3.Lerp(startScale, targetScale, scale);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                target.localScale = targetScale;
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }

}
