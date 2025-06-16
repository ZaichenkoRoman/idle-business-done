using System.Collections.Generic;
using Components;
using Configs;
using Data;
using Leopotam.Ecs;
using Systems;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Startup {
    /// <summary>
    /// Entry point for ECS architecture. Handles world, system, UI initialization and game state loading.
    /// </summary>
    public class EcsStartup : MonoBehaviour {
        [Header("Configs")]
        public BusinessConfig[] configs;
        
        [Header("UI References")]
        public GameObject businessPanelPrefabs;
        public Transform businessPanelContainer;
        public TMP_Text BalanceText;

        private EcsWorld _world;
        private EcsSystems _systems;
        private SaveLoadSystem _saveLoadSystem; // Stored separately to call Destroy() manually on exit/pause

        /// <summary>
        /// Initializes ECS world, systems, loads configs, sets up UI and shared data.
        /// </summary>
        private void Start() {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            
            // Load all business configurations from Resources
            configs = Resources.LoadAll<BusinessConfig>("Configs/Businesses");

            var sharedData = InitSharedData();
            var sharedUIData = InitSharedUIData();

            // Create business entities and corresponding UI
            CreateBusinessesAndBalance(sharedUIData, sharedData);
            
            // Store SaveLoadSystem to call it manually later
            _saveLoadSystem = new SaveLoadSystem(); 
            
            // Register ECS systems and inject shared dependencies
            _systems
                .Add(_saveLoadSystem)         // Load/save system first to initialize persisted state
                .Add(new IncomeSystem())      // Periodic income logic
                .Add(new LevelUpSystem())     // Handles level-up progression
                .Add(new UpgradeSystem())     // Applies upgrade bonuses
                .Add(new UISystem())          // Syncs ECS state to UI
                .Add(new InputResetSystem())  // Clears input flags after processing
                .Inject(sharedData)
                .Inject(sharedUIData)
                .Init();
        }

        /// <summary>
        /// Creates all business entities and their UI panels. Binds configs to component state and UI.
        /// </summary>
        private void CreateBusinessesAndBalance(SharedUIData sharedUIData, SharedData sharedData) {
            // Create a single balance entity for tracking player money
            var balanceEntity = _world.NewEntity();
            balanceEntity.Get<BalanceComponent>();
        
            for (var i = 0; i < configs.Length; i++) {
                var entity = _world.NewEntity();
                ref var business = ref entity.Get<BusinessComponent>();
                business.Id = i;
                business.Level = (i < 1) ? 1 : 0; // First business is unlocked by default
                business.Timer = 0f;
                
                // Instantiate UI panel and extract references to child components
                var businessPanel = Instantiate(businessPanelPrefabs, businessPanelContainer);
            
                // Core UI elements
                sharedUIData.BusinessNameText.Add(businessPanel.transform.Find("NameText").GetComponent<TMP_Text>());
                sharedUIData.BusinessLevelText.Add(businessPanel.transform.Find("LevelText").GetComponent<TMP_Text>());
                sharedUIData.BusinessIncomeText.Add(businessPanel.transform.Find("IncomeText").GetComponent<TMP_Text>());
                sharedUIData.BusinessProgressBar.Add(businessPanel.transform.Find("ProgressBarFrame/ProgressBarFill")
                    .GetComponent<Image>());
                sharedUIData.BusinessLevelUpText.Add(businessPanel.transform.Find("LevelUpButton/PriceText")
                    .GetComponent<TMP_Text>());
                
                // Initial ECS and UI state
                sharedUIData.BusinessNameText[i].text = configs[i].businessName;
                business.CurrentIncome = business.Level * configs[i].baseIncome;
                sharedUIData.BusinessLevelText[i].text = "LVL\n" + business.Level;
                
                var levelUpPrice = business.Level > 1 
                    ? (business.Level + 1) * configs[i].basePrice 
                    : configs[i].basePrice;
                
                business.CurrentLevelUpPrice = levelUpPrice;
                sharedUIData.BusinessLevelUpText[i].text = "LVL UP \nЦена: " + levelUpPrice + "$";
            
                // Upgrade 1 UI
                sharedUIData.Upgrade1NameText.Add(businessPanel.transform.Find("Upgrade1Button/NameText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade1IncomeRaiseText.Add(businessPanel.transform.Find("Upgrade1Button/IncomeText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade1PriceText.Add(businessPanel.transform.Find("Upgrade1Button/PriceText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade1NameText[i].text = configs[i].upgrade1Name;
                var upgrade1IncomeRaise = configs[i].upgrade1Multiplier;
                sharedUIData.Upgrade1IncomeRaiseText[i].text = "Доход: +" + upgrade1IncomeRaise + "%";
                var upgrade1Price = configs[i].upgrade1Price;
                sharedUIData.Upgrade1PriceText[i].text = upgrade1Price + "$";

                // Upgrade 2 UI
                sharedUIData.Upgrade2NameText.Add(businessPanel.transform.Find("Upgrade2Button/NameText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade2IncomeRaiseText.Add(businessPanel.transform.Find("Upgrade2Button/IncomeText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade2PriceText.Add(businessPanel.transform.Find("Upgrade2Button/PriceText")
                    .GetComponent<TMP_Text>());
                sharedUIData.Upgrade2NameText[i].text = configs[i].upgrade2Name;
                var upgrade2IncomeRaise = configs[i].upgrade2Multiplier;
                sharedUIData.Upgrade2IncomeRaiseText[i].text = "Доход: +" + upgrade2IncomeRaise + "%";
                var upgrade2Price = configs[i].upgrade2Price;
                sharedUIData.Upgrade2PriceText[i].text = upgrade2Price + "$";

                // Assign business ID and input reference to UI controller
                var uiController = businessPanel.GetComponent<UIController>();
                uiController.businessId = configs[i].id;
                uiController.Setup(sharedData.Inputs);
            }
        }

        /// <summary>
        /// Initializes shared ECS data, like configs and input buffer.
        /// </summary>
        private SharedData InitSharedData() {
            var sharedData = new SharedData {
                BusinessConfigs = configs,
                Inputs = new SharedInputData()
            };
            return sharedData;
        }
    
        /// <summary>
        /// Initializes UI references used by ECS systems for rendering/updating the interface.
        /// </summary>
        private SharedUIData InitSharedUIData() {
            var sharedUIData = new SharedUIData() {
                BalanceText = BalanceText,
                BusinessNameText = new List<TMP_Text>(),
                BusinessLevelText = new List<TMP_Text>(),
                BusinessIncomeText = new List<TMP_Text>(),
                BusinessProgressBar = new List<Image>(),
                BusinessLevelUpText = new List<TMP_Text>(),
                Upgrade1NameText = new List<TMP_Text>(),
                Upgrade1IncomeRaiseText = new List<TMP_Text>(),
                Upgrade1PriceText = new List<TMP_Text>(),
                Upgrade2NameText = new List<TMP_Text>(),
                Upgrade2IncomeRaiseText = new List<TMP_Text>(),
                Upgrade2PriceText = new List<TMP_Text>()
            };
            return sharedUIData;
        }
        
        /// <summary>
        /// Runs all active ECS systems once per frame.
        /// </summary>
        private void Update() => _systems?.Run();
        
        /// <summary>
        /// Called when the app is paused (e.g. on mobile). Triggers save routine.
        /// </summary>
        private void OnApplicationPause(bool pause) {
            if (!pause) return;
            SaveGame();
        }
    
        /// <summary>
        /// Called when the app is quitting. Ensures the game state is saved.
        /// </summary>
        private void OnApplicationQuit() {
            SaveGame();
        }

        /// <summary>
        /// Wrapper to manually destroy SaveLoadSystem and persist data to storage.
        /// </summary>
        private void SaveGame() {
            _saveLoadSystem?.Destroy();
        }
        
        /// <summary>
        /// Properly disposes ECS systems and world on object destruction.
        /// </summary>
        private void OnDestroy() {
            _systems?.Destroy();
            _world?.Destroy();
        }
    }
}