using UnityEngine;

namespace Game.Weather
{
    [CreateAssetMenu(menuName = "Game/Configs/Weather Config", fileName = "WeatherConfig")]
    public class WeatherConfig : ScriptableObject
    {
        [field: SerializeField]
        public string WeatherApiURL { get; private set; }
        
        [field: SerializeField]
        public float PeriodCheck { get; private set; }
    }
}