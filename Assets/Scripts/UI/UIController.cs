using Data;
using UnityEngine;

namespace UI {
    /// <summary>
    /// UI controller responsible for handling upgrade button clicks
    /// and setting the requested upgrade IDs in shared input data.
    /// </summary>
    public class UIController : MonoBehaviour {
        public int businessId;
        private SharedInputData _input;

        /// <summary>
        /// Initializes the controller with shared input data reference.
        /// </summary>
        public void Setup(SharedInputData input) => _input = input;
        
        /// <summary>
        /// Called on base upgrade button click, sets UpgradeRequested to this business's ID.
        /// </summary>
        public void OnUpgradeClick() => _input.UpgradeRequested = businessId;
        
        /// <summary>
        /// Called on Upgrade1 button click, sets Upgrade1Requested to this business's ID.
        /// </summary>
        public void OnUpgrade1Click() => _input.Upgrade1Requested = businessId;
        
        /// <summary>
        /// Called on Upgrade2 button click, sets Upgrade2Requested to this business's ID.
        /// </summary>
        public void OnUpgrade2Click() => _input.Upgrade2Requested = businessId;
    }
}