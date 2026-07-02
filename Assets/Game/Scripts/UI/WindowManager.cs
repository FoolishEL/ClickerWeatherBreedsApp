using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Windows
{
    using Core;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindowManager : IWindowManager, IInitializable
    {
        private const string WINDOWS_MAIN_ID = "UI/Windows/{0}";

        private readonly DiContainer container;
        private readonly Dictionary<Type, WindowBase> openedWindows = new();
        private IContentManagementSystem cms;
        private bool isInitialized;
        private RectTransform windowsRoot;
        private WindowBase currentWindow;

        public WindowManager(DiContainer container)
        {
            this.container = container;
            cms = container.Resolve<IContentManagementSystem>();
            windowsRoot = container.InstantiatePrefabForComponent<RectTransform>(cms.LoadContent<RectTransform>(string.Format(WINDOWS_MAIN_ID, "CanvasRoot")));
        }

        public void CloseCurrent() => currentWindow?.Close();

        public void Initialize()
        {
            if (isInitialized)
                return;
            isInitialized = true;
        }

        public void OpenWindow<T>() where T : WindowBase
        {
            var type = typeof(T);

            if (openedWindows.TryGetValue(type, out var existing))
            {
                if (existing.IsOpen)
                    return;
                if(!existing.IsModal)
                    currentWindow = existing;
                existing.Open();
                return;
            }

            var window = container.InstantiatePrefabForComponent<T>(GetPrefab<T>(), windowsRoot);

            window.Open();
            openedWindows[type] = window;
            if(!window.IsModal)
                currentWindow = window;
        }

        public void CloseWindow<T>() where T : WindowBase
        {
            var type = typeof(T);
            if (openedWindows.TryGetValue(type, out var window))
            {
                window.Close();
                if (window == currentWindow)
                    currentWindow = null;
            }
        }

        public void CloseAll()
        {
            foreach (var (_, window) in openedWindows)
            {
                window.Close();
            }
        }


        private MonoBehaviour GetPrefab<T>() where T : WindowBase
        {
            return cms.LoadContent<T>(string.Format(WINDOWS_MAIN_ID, typeof(T).Name));
        }
    }
}