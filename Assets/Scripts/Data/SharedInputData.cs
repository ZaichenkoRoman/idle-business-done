namespace Data {
    /// <summary>
    /// Holds the current input requests for upgrades.
    /// Reset each frame to track fresh input events.
    /// </summary>
    public class SharedInputData {
        public int UpgradeRequested = -1;    // Requested level-up business ID
        public int Upgrade1Requested = -1;   // Requested first upgrade business ID
        public int Upgrade2Requested = -1;   // Requested second upgrade business ID

        /// <summary>
        /// Clears all upgrade requests (sets them to -1).
        /// Should be called every frame after processing inputs.
        /// </summary>
        public void Clear() {
            UpgradeRequested = -1;
            Upgrade1Requested = -1;
            Upgrade2Requested = -1;
        }
    }
}