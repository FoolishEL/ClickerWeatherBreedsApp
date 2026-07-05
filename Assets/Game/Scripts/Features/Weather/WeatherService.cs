using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Weather
{
    using Requests;

    public class WeatherService : IDisposable
    {
        private readonly RequestQueueService queue;
        private readonly CompositeDisposable disposables = new();
        private readonly ReactiveProperty<WeatherPeriod> currentWeather = new ReactiveProperty<WeatherPeriod>();
        private WeatherConfig weatherConfig;
        private CancellationTokenSource updateWeatherCancellationTokenSource;

        public ReadOnlyReactiveProperty<WeatherPeriod> CurrentWeather => currentWeather;

        public WeatherService(RequestQueueService queue, WeatherConfig weatherConfig)
        {
            this.queue = queue;
            this.weatherConfig = weatherConfig;
        }

        public void StartWeatherPolling(ReadOnlyReactiveProperty<bool> isOnWeatherTab)
        {
            disposables.Clear();
            isOnWeatherTab.Subscribe(OnWeatherTabSwitched).AddTo(disposables);
        }

        private void OnWeatherTabSwitched(bool isOnWeatherTab)
        {
            if (isOnWeatherTab)
            {
                updateWeatherCancellationTokenSource = new();
                UpdateWeather(updateWeatherCancellationTokenSource.Token).Forget();
            }
            else
            {
                CancelCurrentWeatherRequest();
            }
        }

        private async UniTaskVoid UpdateWeather(CancellationToken ct)
        {
            await FetchWeather();
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(weatherConfig.PeriodCheck), cancellationToken: ct);
                await FetchWeather();
            }
        }

        private async UniTask FetchWeather()
        {
            const string tag = "weather";

            try
            {
                var data = await queue.EnqueueAsync(async ct =>
                {
                    using var request = UnityWebRequest.Get(weatherConfig.WeatherApiURL);
                    await request.SendWebRequest().ToUniTask(cancellationToken: ct);

                    if (request.result != UnityWebRequest.Result.Success) throw new(request.error);

                    var json = request.downloadHandler.text;
                    var root = JsonUtility.FromJson<WeatherResponse>(json);
                    return root.properties.periods.FirstOrDefault();
                }, tag);
                currentWeather.Value = data;
            }
            catch (OperationCanceledException)
            {
            }
        }

        public async UniTask<Texture2D> DownloadIconAsync(string iconUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(iconUrl))
            {
                Debug.LogError("Icon URL is null or empty");
                return null;
            }
            Texture2D texture = null;
            try
            {
                using UnityWebRequest request = UnityWebRequestTexture.GetTexture(iconUrl);
                
                await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Ошибка загрузки иконки: {request.error}\nURL: {iconUrl}");
                    return null;
                }

                texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

                return texture;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Загрузка иконки была отменена");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Неожиданная ошибка при загрузке иконки: {ex.Message}");
            }
            return texture;
        }

        private void CancelCurrentWeatherRequest()
        {
            updateWeatherCancellationTokenSource?.Cancel();
            queue.CancelByTag("weather");
        }


        void IDisposable.Dispose()
        {
            disposables.Dispose();
            updateWeatherCancellationTokenSource?.Cancel();
            updateWeatherCancellationTokenSource?.Dispose();
            updateWeatherCancellationTokenSource = null;
        }
    }
}