using Components;
using Data.Save;
using Leopotam.Ecs;
using Services.Save;
using UnityEngine;

namespace Systems {
    /// <summary>
    /// Handles loading and saving game state (balance and businesses) to/from JSON.
    /// Loads saved data during initialization and saves current state on destruction.
    /// </summary>
    public class SaveLoadSystem : IEcsInitSystem, IEcsDestroySystem {
        private EcsWorld _world;
        private EcsFilter<BalanceComponent> _balanceFilter;
        private EcsFilter<BusinessComponent> _businessFilter;

        /// <summary>
        /// Loads saved game data from JSON and applies it to ECS components.
        /// </summary>
        public void Init() {
            var data = JsonSaveService.Load();
            if (data == null) return;

            // Restore balance component values from save data
            LoadBalanceDataFromSaveFile(data);
            
            // Restore business components from saved businesses list
            LoadBusinessDataFromSaveFile(data);
        }
        
        /// <summary>
        /// Collects current ECS state and saves it as JSON when the system is destroyed.
        /// </summary>
        public void Destroy() {
            var saveData = new GameSaveData();

            // Save balance component value
            SaveBalanceData(saveData);

            // Save all business component data into saveData
            SaveBusinessData(saveData);
            
            JsonSaveService.Save(saveData);
        }

        /// <summary>
        /// Restores the balance component value from the provided GameSaveData.
        /// Iterates through the balance filter and updates the BalanceComponent's Value.
        /// </summary>
        /// <param name="data">The GameSaveData object containing the saved balance.</param>
        private void LoadBalanceDataFromSaveFile(GameSaveData data) {
            foreach (var i in _balanceFilter) {
                ref var balance = ref _balanceFilter.Get1(i);
                balance.Value = data.balance;
                Debug.Log($"[JSON] Loaded balance: {balance.Value}");
            }
        }
        
        /// <summary>
        /// Restores business component values from the provided GameSaveData.
        /// Iterates through saved businesses and updates corresponding ECS BusinessComponents based on their Id.
        /// </summary>
        /// <param name="data">The GameSaveData object containing saved business data.</param>
        private void LoadBusinessDataFromSaveFile(GameSaveData data) {
            foreach (var saved in data.businesses) {
                foreach (var i in _businessFilter) {
                    ref var business = ref _businessFilter.Get1(i);
                    if (business.Id != saved.id) continue;
            
                    business.Level = saved.level;
                    business.Timer = saved.timer;
                    business.CurrentIncome = saved.currentIncome;
                    business.CurrentLevelUpPrice = saved.currentLevelUpPrice;
                    business.Upgrade1 = saved.upgrade1;
                    business.Upgrade2 = saved.upgrade2;
                    Debug.Log($"[JSON] Loaded business: {saved.id} Lvl {saved.level}");
                }
            }
        }

        /// <summary>
        /// Collects the current balance value from the BalanceComponent and sets it in the provided GameSaveData object.
        /// </summary>
        /// <param name="saveData">The GameSaveData object where the balance value will be stored.</param>
        private void SaveBalanceData(GameSaveData saveData) {
            foreach (var i in _balanceFilter) {
                ref var balance = ref _balanceFilter.Get1(i);
                saveData.balance = balance.Value;
            }
        }
        
        /// <summary>
        /// Collects data from all active BusinessComponents and adds them to the provided GameSaveData object.
        /// </summary>
        /// <param name="saveData">The GameSaveData object to which business data will be added.</param>
        private void SaveBusinessData(GameSaveData saveData) {
            foreach (var i in _businessFilter) {
                ref var business = ref _businessFilter.Get1(i);

                saveData.businesses.Add(new BusinessSaveData {
                    id = business.Id,
                    level = business.Level,
                    timer = business.Timer,
                    currentIncome = business.CurrentIncome,
                    currentLevelUpPrice = business.CurrentLevelUpPrice,
                    upgrade1 = business.Upgrade1,
                    upgrade2 = business.Upgrade2
                });
            }
        }
    }
}
