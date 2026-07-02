using R3;

namespace Game.Clicker
{
    using UnityEngine;

    public class ClickerModel
    {
        private readonly ClickerConfig config;
        private ReactiveProperty<int> currency;
        private ReactiveProperty<int> energy;
        public ReadOnlyReactiveProperty<int> Currency => currency;
        public ReadOnlyReactiveProperty<int> Energy => energy;

        public ClickerModel(ClickerConfig config)
        {
            this.config = config;
            Reset();
        }

        public void Reset()
        {
            currency = new(0);
            energy = new(config.StartEnergy);
        }

        public bool TryClick()
        {
            if (Energy.CurrentValue < config.ClickEnergyCost) return false;

            energy.Value -= config.ClickEnergyCost;
            currency.Value += config.ClickReward;
            return true;
        }

        public bool TryAutoCollect()
        {
            if (Energy.CurrentValue < config.AutoCollectEnergyCost) return false;

            energy.Value -= config.AutoCollectEnergyCost;
            currency.Value += config.ClickReward;
            return true;
        }

        public void AddEnergy(int amount)
        {
            var energyValue = Energy.CurrentValue + amount;
            energyValue = Mathf.Clamp(energyValue, 0, config.MaxEnergy);
            energy.Value = energyValue;
        }
    }
}