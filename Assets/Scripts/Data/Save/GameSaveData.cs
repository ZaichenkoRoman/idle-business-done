using System.Collections.Generic;

namespace Data.Save {
    /// <summary>
    /// Serializable container for all data required to save/load the game state.
    /// Contains the player's balance and a list of saved businesses.
    /// </summary>
    [System.Serializable]
    public class GameSaveData {
        /// <summary>
        /// Player's current balance.
        /// </summary>
        public float balance;
        
        /// <summary>
        /// List of saved businesses with their state.
        /// </summary>
        public List<BusinessSaveData> businesses = new();
    }
}