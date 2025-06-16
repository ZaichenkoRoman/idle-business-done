using Components;
using Data;
using Leopotam.Ecs;

namespace Systems {
    /// <summary>
    /// System that processes business level-ups based on user input and available balance.
    /// It deducts the upgrade cost from the balance and increments the business level accordingly.
    /// </summary>
    public class LevelUpSystem : IEcsRunSystem {
        private EcsFilter<BusinessComponent> _businessFilter;
        private EcsFilter<BalanceComponent> _balanceFilter;
        private SharedData _shared;

        /// <summary>
        /// Runs the level-up logic every frame.
        /// </summary>
        public void Run() {
            ProcessLevelUp();
        }

        /// <summary>
        /// Checks if an upgrade is requested for any business, verifies the balance,
        /// performs the upgrade by deducting the cost, and updates business stats.
        /// </summary>
        private void ProcessLevelUp() {
            foreach (var i in _businessFilter) {
                ref var business = ref _businessFilter.Get1(i);
                var id = business.Id;

                // Continue only if this business was requested for upgrade
                if (_shared.Inputs.UpgradeRequested != id) continue;
                var cost = business.CurrentLevelUpPrice;

                foreach (var j in _balanceFilter) {
                    ref var balance = ref _balanceFilter.Get1(j);
                    // Skip if not enough balance to pay for upgrade
                    if (!(balance.Value >= cost)) continue;
                    balance.Value -= cost;
                    business.Level++;
                }
                
                // Update business income and next level-up price based on new level
                business.CurrentIncome = _shared.BusinessConfigs[id].basePrice * business.Level;
                business.CurrentLevelUpPrice = business.Level * _shared.BusinessConfigs[id].basePrice;
            }
        }
    }
}