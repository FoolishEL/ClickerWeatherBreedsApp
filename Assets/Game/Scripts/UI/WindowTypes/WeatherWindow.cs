using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Windows
{
    using Utils;
    using Weather;

    public class WeatherWindow : WindowBase
    {
        [Inject] private WeatherService weatherService;
        [Inject] private WeatherViewFactory viewFactory;
        [Inject] private LoaderViewFactory loaderFactory;

        private WeatherView weatherView;
        private ReactiveProperty<bool> isTabOpened;
        private bool isInitialized;
        private ILoaderView loader;
        private CancellationTokenSource imageLoadToken;
        private Texture2D currentTexture;

        public ReadOnlyReactiveProperty<bool> IsTabOpened => isTabOpened;

        protected override void OnOpened()
        {
            base.OnOpened();
            TryInitialize();
            loader.Show();
            weatherView.HideIcon();
            isTabOpened.Value = true;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            isTabOpened.Value = false;
            StopToken();
        }

        private void OnDestroy()
        {
            StopToken();
            DestroyCurrentTexture();
        }

        private void StopToken()
        {
            imageLoadToken?.Cancel();
            imageLoadToken?.Dispose();
            imageLoadToken = null;
        }

        private void DestroyCurrentTexture()
        {
            if (!currentTexture)
                return;
            Destroy(currentTexture);
        }

        private void TryInitialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;
            isTabOpened = new(true);
            weatherService.StartWeatherPolling(IsTabOpened);
            weatherView = viewFactory.Create();
            if (weatherView == null) throw new MissingComponentException("WeatherView is null");

            weatherView.transform.SetParent(transform, false);
            loader = loaderFactory.Create();
            if (loader is MonoBehaviour mb && mb.TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.SetParent(transform, false);
            }

            weatherService.CurrentWeather.Subscribe(OnWeatherChanged).AddTo(this);
        }

        private void OnWeatherChanged(WeatherPeriod period)
        {
            if (period is null)
            {
                loader.Show();
                return;
            }
            loader.Hide();
            weatherView.ShowWeather(period);
            StopToken();
            imageLoadToken = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
            if (!string.IsNullOrEmpty(period.icon))
            {
                UpdateWeatherIcon(period, imageLoadToken.Token).Forget();
            }
        }

        private async UniTaskVoid UpdateWeatherIcon(WeatherPeriod period, CancellationToken token)
        {
            var texture = await weatherService.DownloadIconAsync(period.icon, token);
            DestroyCurrentTexture();
            currentTexture = texture;
            weatherView.SetupIcon(texture);
        }
    }
}