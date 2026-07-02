using UnityEngine;
using Zenject;

namespace Game.Utils
{
    using Core;
    
    public class LoaderViewFactory : PlaceholderFactory<ILoaderView> { }
    
    public class CustomLoaderViewFactory : IFactory<ILoaderView>
    {
        private readonly IContentManagementSystem cms;
        private readonly DiContainer container;
        private const string VIEW_PATH = "UI/Elements/LoaderView";

        [Inject]
        public CustomLoaderViewFactory(IContentManagementSystem cms, DiContainer container)
        {
            this.cms = cms;
            this.container = container;
        }

        public ILoaderView Create()
        {
            LoaderView prefab = cms.LoadContent<LoaderView>(VIEW_PATH);

            return prefab == null ? throw new MissingReferenceException("ClickerView not found") : container.InstantiatePrefabForComponent<LoaderView>(prefab);
        }
    }
}