using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.DogBreads
{
    public class DogBreedInfoView : MonoBehaviour, IDisposable
    {
        [field: SerializeField]
        public TMP_Text BreedInfoText { get; set; }

        [field: SerializeField]
        public Button DetailsButton { get; set; }

        [SerializeField]
        private RectTransform loadingBar;
        
        [SerializeField]
        private CanvasGroup loadingBarGroup;
        
        [SerializeField]
        private float rotationSpeed;

        private CancellationTokenSource loaderCancellationTokenSource;
        private string breedId;
        private Action<string> callbackOnPress;

        public void Initialize(Action<string> callbackOnPress)
        {
            this.callbackOnPress = callbackOnPress;
            DetailsButton.onClick.AddListener(OnBreedInfoPressed);
        }

        public void SetInfo(DogBreedAttributes breedInfo, string breedId)
        {
            BreedInfoText.text = breedInfo.name;
            this.breedId = breedId;
        }

        public void SetLoadingStatus(bool loading)
        {
            StopToken();
            if (loading)
            {
                loadingBarGroup.alpha = 1f;
                loaderCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
                LoadingProcess(loaderCancellationTokenSource.Token).Forget();
            }
            else
            {
                loadingBarGroup.alpha = 0f;
            }
        }

        private async UniTaskVoid LoadingProcess(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield();
                if(token.IsCancellationRequested)
                    return;
                loadingBar.rotation *= Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
            }
        }

        private void StopToken()
        {
            loaderCancellationTokenSource?.Cancel();
            loaderCancellationTokenSource?.Dispose();
            loaderCancellationTokenSource = null;
        }

        private void OnBreedInfoPressed() => callbackOnPress?.Invoke(breedId);

        public void Dispose() => DetailsButton.onClick.RemoveListener(OnBreedInfoPressed);
    }
}