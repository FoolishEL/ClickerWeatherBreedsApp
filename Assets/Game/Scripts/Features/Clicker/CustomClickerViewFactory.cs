using Zenject;
using UnityEngine;

namespace Game.Clicker
{
    using Core;

    public class CustomClickerViewFactory : IFactory<ClickerView>
    {
        private readonly IContentManagementSystem cms;
        private readonly DiContainer container;
        private const string VIEW_PATH = "UI/Elements/ClickerView";

        [Inject]
        public CustomClickerViewFactory(IContentManagementSystem cms, DiContainer container)
        {
            this.cms = cms;
            this.container = container;
        }

        public ClickerView Create()
        {
            ClickerView prefab = cms.LoadContent<ClickerView>(VIEW_PATH);

            return prefab == null ? throw new MissingReferenceException("ClickerView not found") : container.InstantiatePrefabForComponent<ClickerView>(prefab);
        }
    }
}