using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Windows
{
    public class HotbarWindow : WindowBase
    {
        [Inject]
        private IWindowManager windowManager;

        [SerializeField]
        private ToggleTab[] tabs;
        

        private void Start()
        {
            List<Observable<WindowType>> observers = new(tabs.Length);
            foreach (var toggleTab in tabs)
            {
                Observable<WindowType> observer = toggleTab.Toggle.OnValueChangedAsObservable().Where(status => status).Select(_ => toggleTab.WindowType);
                observers.Add(observer);
            }
            observers.Merge().Subscribe(OnTabSwitch).AddTo(this);
        }

        private void OnTabSwitch(WindowType windowType)
        {
            windowManager.CloseCurrent();
            switch (windowType)
            {
                case WindowType.Clicker:
                    windowManager.OpenWindow<ClickerWindow>();
                    break;
                case WindowType.Weather:
                    windowManager.OpenWindow<WeatherWindow>();
                    break;
                case WindowType.Breeds:
                    windowManager.OpenWindow<DogsBreedsWindow>();
                    break;
            }
        }

        [System.Serializable]
        private class ToggleTab
        {
            [field: SerializeField]
            public Toggle Toggle { get; private set; }

            [field: SerializeField]
            public WindowType WindowType { get; private set; }
        }
    }
}