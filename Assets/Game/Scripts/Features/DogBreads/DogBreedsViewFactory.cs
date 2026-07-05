using UnityEngine;
using Zenject;

namespace Game.DogBreads
{
    using Core;
    
    public class DogBreedsViewFactory : PlaceholderFactory<DogBreedsView> { }
    
    public class CustomDogBreedsViewFactory : IFactory<DogBreedsView>
    {
        private readonly IContentManagementSystem cms;
        private readonly DiContainer container;
        private const string VIEW_PATH = "UI/Elements/DogBreedsView";

        [Inject]
        public CustomDogBreedsViewFactory(IContentManagementSystem cms, DiContainer container)
        {
            this.cms = cms;
            this.container = container;
        }

        public DogBreedsView Create()
        {
            DogBreedsView prefab = cms.LoadContent<DogBreedsView>(VIEW_PATH);

            return prefab == null ? throw new MissingReferenceException("ClickerView not found") : container.InstantiatePrefabForComponent<DogBreedsView>(prefab);
        }
    }
}