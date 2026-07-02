using UnityEngine;
using Zenject;

namespace Game.Windows
{
    using Clicker;
    
    public class ClickerWindow : WindowBase
    {
        [Inject] private ClickerViewFactory viewFactory;
        [Inject] private ClickerPresenter presenter;
        [Inject] private ClickerConfig config;

        private ClickerView currentView;
        private bool isInitialized;

        private void OnDestroy()
        {
            if (currentView == null)
                return;
            
            Destroy(currentView.gameObject);
            currentView = null;
        }
        
        protected override void OnOpened()
        {
            base.OnOpened();
            CreateView();
            if(config.IsActiveWileTabOpened)
                presenter.SetPauseStatus(false);
        }


        private void CreateView()
        {
            if (isInitialized) return;

            currentView = viewFactory.Create();
            
            if (currentView == null) throw new MissingComponentException("ClickerView is null");

            currentView.transform.SetParent(transform, false);

            presenter.Initialize(currentView);
            isInitialized = true;
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            if(config.IsActiveWileTabOpened)
                presenter.SetPauseStatus(true);
        }
    }

}