namespace Components {
    /// <summary>
    /// Component representing a business entity in the game.
    /// Holds data about the business state, upgrades, income, and progression timer.
    /// </summary>
    public struct BusinessComponent {
        public int Id;                    // Unique identifier for the business
        public int Level;                 // Current level of the business
        public float CurrentLevelUpPrice; // Price required for the next level up
        public float CurrentIncome;       // Income generated per cycle, considering upgrades
        public float Timer;               // Timer tracking progress toward income generation
        public bool Upgrade1;             // Whether the first upgrade is purchased
        public bool Upgrade2;             // Whether the second upgrade is purchased
    }
}