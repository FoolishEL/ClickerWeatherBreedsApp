using System;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    using Windows;

    [RequireComponent(typeof(SceneContext))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneContext sceneContext;
        
        #if UNITY_EDITOR
        
        private void Reset()
        {
            sceneContext = GetComponent<SceneContext>();
        }
        
        #endif

        private void Start()
        {
            sceneContext.Install();
            sceneContext.Resolve();

            var windowManager = sceneContext.Container.Resolve<IWindowManager>();
            windowManager.Initialize();
            windowManager.OpenWindow<HotbarWindow>();
        }
    }
}