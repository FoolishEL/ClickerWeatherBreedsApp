using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Weather
{

    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private RawImage iconImage;
        [SerializeField] private TextMeshProUGUI temperatureText;
        [SerializeField] private TextMeshProUGUI forecastText;
        [SerializeField] private string temperatureFormat;
        [SerializeField] private string shortForecastFormat;

        public void ShowWeather(WeatherPeriod period)
        {
            temperatureText.text = string.Format(temperatureFormat, period.temperature, period.temperatureUnit);
            forecastText.text = string.Format(shortForecastFormat, period.shortForecast);
        }

        public void SetupIcon(Texture2D icon)
        {
            iconImage.texture = icon;
            iconImage.gameObject.SetActive(true);
        }

        public void HideIcon()
        {
            iconImage.gameObject.SetActive(false);
        }
    }
}