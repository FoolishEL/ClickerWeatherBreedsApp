using UnityEngine;
using Zenject;

namespace Game.Weather
{
    [CreateAssetMenu(fileName = "WeatherInstaller", menuName = "Game/Installers/WeatherInstaller")]
    public class WeatherInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private WeatherConfig weatherConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(weatherConfig);
            
            Container.BindInterfacesAndSelfTo<WeatherService>().AsSingle();
            
            Container.BindFactory<WeatherView, WeatherViewFactory>()
                .FromFactory<CustomWeatherViewFactory>();
        }
    }
}