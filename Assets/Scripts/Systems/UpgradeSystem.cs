using Components;
using Data;
using Leopotam.Ecs;

namespace Systems {
    /// <summary>
    /// ECS system that processes upgrade purchases for businesses.
    /// Checks requested upgrades and player's balance to apply upgrades
    /// and deduct corresponding costs.
    /// </summary>
    public class UpgradeSystem : IEcsRunSystem {
        private EcsFilter<BusinessComponent> _businessFilter;
        private EcsFilter<BalanceComponent> _balanceFilter;
        private SharedData _shared;

        public void Run() {

            foreach (var i in _businessFilter) {
                ref var business = ref _businessFilter.Get1(i);
                var id = business.Id;
                var input = _shared.Inputs;

                foreach (var j in _balanceFilter) {
                    ref var balance = ref _balanceFilter.Get1(j);
                    var value = balance.Value;
                    var upgrade1Price = _shared.BusinessConfigs[id].upgrade1Price;
                    var upgrade2Price = _shared.BusinessConfigs[id].upgrade2Price;
                    
                    // If upgrade 1 requested, not yet purchased, and enough balance -> buy it
                    if (input.Upgrade1Requested == id && !business.Upgrade1 && value >= upgrade1Price) {
                        business.Upgrade1 = true;
                        balance.Value -= upgrade1Price;
                    }
                    
                    // If upgrade 2 requested, not yet purchased, and enough balance -> buy it
                    if (input.Upgrade2Requested == id && !business.Upgrade2 && value >= upgrade2Price) {
                        business.Upgrade2 = true;
                        balance.Value -= upgrade2Price;
                    }
                }
            }
        }
    }
}