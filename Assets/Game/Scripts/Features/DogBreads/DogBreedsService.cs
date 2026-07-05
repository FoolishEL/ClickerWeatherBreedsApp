using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.DogBreads
{
    using Requests;

    public class DogBreedsService : IDisposable
    {
        private readonly RequestQueueService queue;
        private readonly CompositeDisposable disposables = new();
        private DogBreedsConfig config;

        public DogBreedsService(RequestQueueService queue, DogBreedsConfig config)
        {
            this.queue = queue;
            this.config = config;
        }
        
        public async UniTask<List<(DogBreedAttributes, string )>> LoadBreedsAsync()
        {
            const string tag = "dog_breeds";

            return await queue.EnqueueAsync(async ct =>
            {
                using var req = UnityWebRequest.Get(config.DogBreedRequestURL);
                await req.SendWebRequest().ToUniTask(cancellationToken: ct);

                var json = req.downloadHandler.text;
                var response = JsonUtility.FromJson<DogBreedsResponse>(json);
                return response.data.Take(config.DisplayBreedCount).Select(d => (d.attributes, d.id)).ToList();
            }, tag);
        }

        public async UniTask<DogBreedData> LoadBreedFactsAsync(string breedId)
        {
            const string tag = "dog_facts";

            return await queue.EnqueueAsync(async ct =>
            {
                using var req = UnityWebRequest.Get($"{config.DogBreedRequestURL}/{breedId}");
                await req.SendWebRequest().ToUniTask(cancellationToken: ct);

                var json = req.downloadHandler.text;
                var response = JsonUtility.FromJson<DogBreedDetailResponse>(json);

                return response.data;
            }, tag);
        }

        public void CancelCurrentDogRequest()
        {
            queue.CancelByTag("dog_breeds");
            queue.CancelByTag("dog_facts");
        }

        public void Dispose() => disposables.Dispose();
    }
}