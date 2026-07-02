using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Game.DogBreads
{
    public class DogBreedsPresenter : IDisposable
    {
        private readonly DogBreedsService breedsService;

        private DogBreedsView view;
        private CancellationTokenSource popupCts;
        private IDisposable disposable;

        public DogBreedsPresenter(DogBreedsService breedsService)
        {
            this.breedsService = breedsService;
        }

        public void Initialize(DogBreedsView view, ReadOnlyReactiveProperty<bool> isOnTabOpened)
        {
            this.view = view;
            var compositeDisposable = new CompositeDisposable();
            compositeDisposable.Add(view.OnBreedInfoClicked.Subscribe(LoadBreedInfo));
            compositeDisposable.Add(isOnTabOpened.Where(x => x).Subscribe(_ => LoadBreeds()));
            disposable = compositeDisposable;
        }

        private async void LoadBreeds()
        {
            view.ShowLoader(true);

            try
            {
                var breeds = await breedsService.LoadBreedsAsync();
                view.ShowBreeds(breeds);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                view.ShowLoader(false);
            }
        }

        private void LoadBreedInfo(string breedId)
        {
            HandleBreedClick(breedId).Forget();
        }

        private async UniTaskVoid HandleBreedClick(string breedId)
        {
            breedsService.CancelCurrentDogRequest();
            popupCts?.Cancel();
            popupCts = new CancellationTokenSource();

            view.ShowPopupLoader(true, breedId);

            try
            {
                var breedData = await breedsService.LoadBreedFactsAsync(breedId);
                view.ShowPopup(breedData.attributes.name, breedData.attributes.description);

            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                view.ShowPopupLoader(false, breedId);
            }
        }

        public void Dispose()
        {
            breedsService.CancelCurrentDogRequest();
            popupCts?.Cancel();
            disposable?.Dispose();
        }
    }
}