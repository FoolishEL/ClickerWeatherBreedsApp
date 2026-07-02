using System;
using R3;
using UnityEngine;
using Zenject;

namespace Game.Clicker
{
    using Utils.Audio;
    public class ClickerPresenter : ITickable, IDisposable
    {
        private readonly ClickerModel model;
        private readonly ClickerConfig config;
        private readonly IAudioPlayer audioPlayer;
        
        private ClickerView view;
        private float autoCollectTimer;
        private float energyRegenTimer;
        private IDisposable disposable;
        private bool isPaused;

        [Inject]
        public ClickerPresenter(ClickerModel model, ClickerConfig config, IAudioPlayer audioPlayer)
        {
            this.model = model;
            this.config = config;
            this.audioPlayer = audioPlayer;
            isPaused = false;
        }

        public void SetPauseStatus(bool isPaused) => this.isPaused = isPaused;

        public void Initialize(ClickerView view)
        {
            this.view = view;
            isPaused = !config.IsActiveWileTabOpened;
            view.SetPresenter(this);
            view.UpdateCurrency(model.Currency.CurrentValue);
            view.UpdateEnergy(model.Energy.CurrentValue, config.MaxEnergy);
            var disposableBag = new DisposableBag();
            disposableBag.Add(model.Currency.Subscribe(view.UpdateCurrency));
            disposableBag.Add(model.Energy.Subscribe(OnEnergyChanged));
            disposable = disposableBag;
        }

        void ITickable.Tick()
        {
            if (isPaused)
                return;

            autoCollectTimer += Time.deltaTime;
            if (autoCollectTimer >= config.AutoCollectInterval)
            {
                autoCollectTimer = 0f;
                if (model.TryAutoCollect())
                {
                    view.PlayClickVFX();
                    audioPlayer.Play(config.SuccessClickSound);
                }
                else
                    view.ShowNotEnoughEnergy();
            }

            energyRegenTimer += Time.deltaTime;
            if (energyRegenTimer >= config.EnergyRegenInterval)
            {
                energyRegenTimer = 0f;
                model.AddEnergy(config.EnergyRegenAmount);
            }
        }


        public void OnClickButtonPressed()
        {
            if (model.TryClick())
            {
                view.PlayClickVFX();
                audioPlayer.Play(config.SuccessClickSound);
            }
            else
            {
                view.ShowNotEnoughEnergy();
                audioPlayer.Play(config.FailureClickSound);
            }
        }

        private void OnEnergyChanged(int currentEnergy) => view.UpdateEnergy(currentEnergy, config.MaxEnergy);

        void IDisposable.Dispose() => disposable?.Dispose();
    }
}