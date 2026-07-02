using R3;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Game.Clicker
{
    using Utils.UI;
    
    public class ClickerView : MonoBehaviour
    {
        [SerializeField] private Button clickButton;

        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private TextMeshProUGUI energyText;
        [SerializeField] private string currencyTextFormat;
        [SerializeField] private string energyTextFormat;

        [SerializeField] private ButtonParticles buttonParticles;

        private ClickerPresenter presenter;

        private void Awake() => clickButton.OnClickAsObservable().Subscribe(_ => OnClickButton()).AddTo(this);

        public void SetPresenter(ClickerPresenter presenter) => this.presenter = presenter;

        public void UpdateCurrency(int amount) => currencyText.text = string.Format(currencyTextFormat, amount);

        public void UpdateEnergy(int current, int max) => energyText.text = string.Format(energyTextFormat, current, max);

        private void OnClickButton()
        {
            presenter?.OnClickButtonPressed();
        }
        
        public void PlayClickVFX()
        {
            buttonParticles.PlayParticles();
        }
        
        public void ShowNotEnoughEnergy()
        {
            //TODO:
        }
    }
}