using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core
{
    public class ResourceCms : IContentManagementSystem
    {
        public T LoadContent<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is null or empty!");
                return null;
            }

            T asset = Resources.Load<T>(path);

            if (asset == null)
            {
                Debug.LogError($"Не удалось загрузить ассет по пути: {path}. Тип: {typeof(T)}");
            }

            return asset;
        }

        public T[] LoadAllContent<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is null or empty!");
                return Array.Empty<T>();
            }

            T[] assets = Resources.LoadAll<T>(path);

            if (assets == null || assets.Length == 0)
            {
                Debug.LogWarning($"Не найдено ассетов по пути: {path}");
            }

            return assets;
        }

        public ResourceRequest LoadContentAsync<T>(string path) where T : Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is null or empty!");
                return null;
            }

            return Resources.LoadAsync<T>(path);
        }
    }
}