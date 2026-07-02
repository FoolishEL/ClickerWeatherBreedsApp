using UnityEngine;
using Zenject;

namespace Game.Windows
{
    [CreateAssetMenu(fileName = "UIInstaller", menuName = "Game/Installers/UIInstaller", order = -1000)]
    public class UIInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWindowManager>().To<WindowManager>().AsSingle();
        }
    }
}