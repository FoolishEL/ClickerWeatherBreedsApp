using R3;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Zenject;

namespace Game.DogBreads
{
    using Core;
    using Utils;

    public class DogBreedsView : MonoBehaviour
    {
        private const string BREED_INFO_VIEW = "UI/Elements/DoggBreedInfoView";
        [SerializeField] private Transform breedsContainer;

        [SerializeField] private GameObject popupPanel;
        [SerializeField] private TextMeshProUGUI popupTitle;
        [SerializeField] private TextMeshProUGUI popupDescription;
        [SerializeField] private Button popupCloseButton;
        [SerializeField] private RectTransform mainRootPopup;

        private DogBreedInfoView breedItemPrefab;
        private ILoaderView loader;

        public ReactiveCommand<string> OnBreedInfoClicked;

        private readonly CompositeDisposable disposables = new();
        private readonly Dictionary<string, DogBreedInfoView> spawnedItems = new();
        private ObjectPool<DogBreedInfoView> infoViewPool;
        private IContentManagementSystem cms;

        [Inject]
        private void Construct(LoaderViewFactory loaderFactory, IContentManagementSystem cms)
        {
            this.cms = cms;
            loader = loaderFactory.Create();
            if (loader is MonoBehaviour mb && mb.TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.SetParent(transform, false);
            }
            OnBreedInfoClicked = new();
            CreatePool();
        }

        private void Awake()
        {
            popupCloseButton.OnClickAsObservable()
                .Subscribe(_ => HidePopup())
                .AddTo(disposables);
        }

        public void ShowLoader(bool show)
        {
            if (show)
                loader.Show();
            else
                loader.Hide();
        }

        public void ShowBreeds(List<(DogBreedAttributes attribute, string id)> breeds)
        {
            ClearBreeds();

            for (int i = 0; i < breeds.Count; i++)
            {
                var breed = breeds[i];
                var item = infoViewPool.Get();
                item.SetInfo(breed.attribute, breed.id);
                spawnedItems.Add(breed.id, item);
            }
        }

        private void OnBreedInspectClicked(string breedId) => OnBreedInfoClicked.Execute(breedId);

        private void ClearBreeds()
        {
            foreach (var (id, item) in spawnedItems) infoViewPool.Release(item);
            spawnedItems.Clear();
        }

        public void ShowPopup(string title, string description)
        {
            popupPanel.SetActive(true);

            popupTitle.text = title;
            popupDescription.text = description;

            popupTitle.ForceMeshUpdate(true);
            popupDescription.ForceMeshUpdate(true);
            UpdateView().Forget();
        }

        private async UniTaskVoid UpdateView()
        {
            await UniTask.Yield();
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(mainRootPopup);
        }

        public void ShowPopupLoader(bool show, string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            if (spawnedItems.TryGetValue(id, out DogBreedInfoView item))
            {
                item.SetLoadingStatus(show);
            }
        }

        private void HidePopup() => popupPanel.SetActive(false);

        private void OnDestroy()
        {
            ClearBreeds();
            disposables.Dispose();
            OnBreedInfoClicked.Dispose();
        }

        private void CreatePool()
        {
            breedItemPrefab = cms.LoadContent<DogBreedInfoView>(BREED_INFO_VIEW);
            infoViewPool = new(() =>
            {
                var item = Instantiate(breedItemPrefab, breedsContainer);
                item.Initialize(OnBreedInspectClicked);
                return item;
            });
        }
    }
}