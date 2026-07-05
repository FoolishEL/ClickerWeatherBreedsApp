using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Utils
{
    public class LoaderView : MonoBehaviour, ILoaderView
    {
        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        public float rotationSpeed = 5f;

        private CancellationTokenSource rotationToken;

        [ContextMenu(nameof(Show))]
        public void Show()
        {
            SetCanvasGroupStatus(true);
            StopToken();
            rotationToken = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            SpinIcon(rotationToken.Token).Forget();
        }

        [ContextMenu(nameof(Hide))]
        public void Hide()
        {
            SetCanvasGroupStatus(false);
            StopToken();
        }

        private void StopToken()
        {
            rotationToken?.Cancel();
            rotationToken?.Dispose();
            rotationToken = null;
        }

        private async UniTaskVoid SpinIcon(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                rectTransform.rotation *= Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
        }

        private void SetCanvasGroupStatus(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.blocksRaycasts = isActive;
            canvasGroup.interactable = isActive;
        }

        private void OnDestroy() => StopToken();
    }
}