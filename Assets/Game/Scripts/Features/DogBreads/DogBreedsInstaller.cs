using UnityEngine;
using Zenject;

namespace Game.DogBreads
{
    [CreateAssetMenu(fileName = "DogBreedsInstaller", menuName = "Game/Installers/DogBreedsInstaller")]
    public class DogBreedsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DogBreedsConfig dogBreedsConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(dogBreedsConfig);
            Container.BindInterfacesAndSelfTo<DogBreedsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<DogBreedsPresenter>().AsSingle();
            
            Container.BindFactory<DogBreedsView, DogBreedsViewFactory>()
                .FromFactory<CustomDogBreedsViewFactory>();
        }
    }
}