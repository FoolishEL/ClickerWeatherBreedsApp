using UnityEngine;
using Zenject;

namespace Game.Clicker
{
    [CreateAssetMenu(fileName = "ClickerInstaller", menuName = "Game/Installers/ClickerInstaller")]
    public class ClickerInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ClickerConfig clickerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(clickerConfig);

            Container.Bind<ClickerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
            
            Container.BindFactory<ClickerView, ClickerViewFactory>()
                .FromFactory<CustomClickerViewFactory>();
        }
    }
}