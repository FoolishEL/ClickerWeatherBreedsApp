using Zenject;

namespace Game.Core
{
    using Utils.Audio;
    using Utils;
    using Requests;
    
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IContentManagementSystem>().To<ResourceCms>().AsSingle();
            Container.Bind<RequestQueueService>().AsSingle();
            
            Container.BindFactory<ILoaderView, LoaderViewFactory>()
                .FromFactory<CustomLoaderViewFactory>();
            
            Container.BindInterfacesAndSelfTo<AudioPlayService>().AsSingle();
        }
    }
}