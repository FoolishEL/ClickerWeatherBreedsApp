using System.Collections.Generic;

namespace Game.Weather
{
    [System.Serializable]
    public class WeatherResponse
    {
        public WeatherProperties properties;
    }

    [System.Serializable]
    public class WeatherProperties
    {
        public string units;
        public List<WeatherPeriod> periods;
    }

    [System.Serializable]
    public class WeatherPeriod
    {
        public int number;
        public string name;
        public string startTime;
        public string endTime;
        public bool isDaytime;
        public int temperature;
        public string temperatureUnit;
        public string icon;
        public string shortForecast;
        public string detailedForecast;
    }

}