using Components;
using Configs;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems {
    /// <summary>
    /// Handles passive income generation for each business entity based on its current level, upgrades, and configuration.
    /// </summary>
    public class IncomeSystem : IEcsRunSystem {
        private EcsFilter<BusinessComponent> _businessFilter;
        private EcsFilter<BalanceComponent> _balanceFilter;
        private SharedData _shared;

        public void Run() {
            CountAndUpdateBusinessIncome();
        }
        
        /// <summary>
        /// Iterates over all business entities, updates timers, and adds income to the balance when appropriate.
        /// </summary>
        private void CountAndUpdateBusinessIncome() {
            foreach (var i in _businessFilter) {
                ref var business = ref _businessFilter.Get1(i);
                
                // Skip businesses that haven't been unlocked yet
                if (business.Level < 1) return;
                
                // Accumulate time passed since last income tick
                business.Timer += Time.deltaTime;

                var config = _shared.BusinessConfigs[business.Id];
                
                // Calculate current income including upgrades
                var income = GetAndUpdateTotalIncome(ref business, config);

                // Only proceed if the configured delay threshold has been reached
                if (!(business.Timer >= config.delay)) continue;
                
                // Reset the timer and add the income to the global balance
                business.Timer = 0;
                
                foreach (var j in _balanceFilter) {
                    ref var balance = ref _balanceFilter.Get1(j);
                    balance.Value += income;
                }
            }
        }

        /// <summary>
        /// Calculates the total income for a business based on level and active upgrades.
        /// Also updates the business component with the latest computed income.
        /// </summary>
        /// <param name="business">Reference to the business component.</param>
        /// <param name="config">Configuration data for this business.</param>
        /// <returns>The final income value after applying multipliers.</returns>
        private static float GetAndUpdateTotalIncome(ref BusinessComponent business, BusinessConfig config) {
            var multiplier = 1f;
            if (business.Upgrade1) multiplier += config.upgrade1Multiplier / 100f;
            if (business.Upgrade2) multiplier += config.upgrade2Multiplier / 100f;
            var income = business.Level * config.baseIncome * multiplier;
            
            // Store current income value for display purposes or other logic
            business.CurrentIncome = income;
        
            return income;
        }
    }
}