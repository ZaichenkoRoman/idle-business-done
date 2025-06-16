namespace Data.Save {
    /// <summary>
    /// Serializable data class representing the saved state of a single business.
    /// Contains all necessary fields to restore business progress on load.
    /// </summary>
    [System.Serializable]
    public class BusinessSaveData {
        public int id;                    // Unique identifier for the business
        public int level;                 // Current level of the business
        public float timer;               // Timer tracking business progress or cooldown
        public float currentIncome;       // Current income generated per cycle
        public float currentLevelUpPrice; // Price required for next level up
        public bool upgrade1;             // Whether the first upgrade is purchased
        public bool upgrade2;             // Whether the second upgrade is purchased
    }
}