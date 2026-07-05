using UnityEngine;

namespace Game.Clicker
{
    using Utils.Audio;

    [CreateAssetMenu(menuName = "Game/Configs/Clicker Config", fileName = "ClickerConfig")]
    public class ClickerConfig : ScriptableObject
    {
        [field: Header("General settings"), SerializeField,]
        public bool IsActiveWileTabOpened { get; private set; } = true;

        [field: Header("Currency"), SerializeField,]
        public int ClickReward { get; private set; } = 1;

        [field: Header("Energy"), SerializeField,]
        public int MaxEnergy { get; private set; } = 1000;

        [field: SerializeField]
        public int StartEnergy { get; private set; } = 1000;

        [field: SerializeField]
        public int ClickEnergyCost { get; private set; } = 1;

        [field: SerializeField]
        public int AutoCollectEnergyCost { get; private set; } = 1;

        [field: Header("Time settings"), SerializeField,]
        public float AutoCollectInterval { get; private set; } = 3f;

        [field: SerializeField]
        public float EnergyRegenInterval { get; private set; } = 10f;

        [field: SerializeField]
        public int EnergyRegenAmount { get; private set; } = 10;

        [field: SerializeField]
        public AudioPlayInfo SuccessClickSound { get; private set; } = new();

        [field: SerializeField]
        public AudioPlayInfo FailureClickSound { get; private set; } = new();
    }
}