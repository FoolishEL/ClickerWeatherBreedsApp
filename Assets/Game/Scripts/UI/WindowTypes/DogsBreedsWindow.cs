using R3;
using Zenject;

namespace Game.Windows
{
    using DogBreads;

    public class DogsBreedsWindow : WindowBase
    {
        [Inject] private DogBreedsPresenter breedsPresenter;
        [Inject] private DogBreedsViewFactory viewFactory;
        [Inject] private DogBreedsService breedsService;

        private bool isInitialized;
        private ReactiveProperty<bool> isBreedsTabOpened;
        public ReadOnlyReactiveProperty<bool> IsBreedsTabOpen => isBreedsTabOpened;

        protected override void OnOpened()
        {
            base.OnOpened();
            TryInitialize();
            isBreedsTabOpened.Value = true;
        }

        public override void Close()
        {
            base.Close();
            isBreedsTabOpened.Value = false;
            breedsService.CancelCurrentDogRequest();
        }

        private void TryInitialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;
            isBreedsTabOpened = new(false);
            var viewPrefab = viewFactory.Create();
            viewPrefab.transform.SetParent(transform, false);
            breedsPresenter.Initialize(viewPrefab, IsBreedsTabOpen);
        }
    }
}