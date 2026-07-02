using UnityEngine;
using Zenject;

namespace Game.Weather
{
    using Core;

    public class CustomWeatherViewFactory : IFactory<WeatherView>
    {
        private readonly IContentManagementSystem cms;
        private readonly DiContainer container;
        private const string VIEW_PATH = "UI/Elements/WeatherView";

        [Inject]
        public CustomWeatherViewFactory(IContentManagementSystem cms, DiContainer container)
        {
            this.cms = cms;
            this.container = container;
        }
        public WeatherView Create()
        {
            WeatherView prefab = cms.LoadContent<WeatherView>(VIEW_PATH);

            return prefab == null ? throw new MissingReferenceException("ClickerView not found") : container.InstantiatePrefabForComponent<WeatherView>(prefab);
        }
    }
}