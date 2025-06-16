using Components;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems {
    /// <summary>
    /// ECS system responsible for updating the UI elements every frame.
    /// It updates business progress bars, level-up texts, income display, 
    /// upgrade purchase status, and player's balance text.
    /// </summary>
    public class UISystem : IEcsRunSystem {
        private EcsFilter<BalanceComponent> _balanceFilter;
        private EcsFilter<BusinessComponent> _businessFilter;
        private SharedData _shared;
        private SharedUIData _sharedUI;

        public void Run() {
            // Update UI for each business entity
            foreach (var i in _businessFilter) {
                ref var business = ref _businessFilter.Get1(i);
                var delay = _shared.BusinessConfigs[i].delay;
                
                // Update progress bar fill amount based on business timer and delay
                _sharedUI.BusinessProgressBar[i].fillAmount = Mathf.Clamp01(business.Timer / delay);
                // Update level-up button text with current level-up price
                _sharedUI.BusinessLevelUpText[i].text =  "LVL UP \nЦена: " + business.CurrentLevelUpPrice + "$";
                // Update business level text
                _sharedUI.BusinessLevelText[i].text = "LVL\n" + business.Level;
                // Update income text formatted with thousands separator
                _sharedUI.BusinessIncomeText[i].text = $"Доход\n{business.CurrentIncome:N0}$";
                // Mark upgrade buttons as "Purchased" if bought
                if (business.Upgrade1) _sharedUI.Upgrade1PriceText[i].text = "Куплено";
                if (business.Upgrade2) _sharedUI.Upgrade2PriceText[i].text = "Куплено";
            }

            // Update the player's balance text UI
            foreach (var i in _balanceFilter) {
                ref var balance = ref _balanceFilter.Get1(i);
                _sharedUI.BalanceText.text = $"Баланс: {balance.Value:N0}";
            }
        }
    }
}