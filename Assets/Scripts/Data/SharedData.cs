using Configs;

namespace Data {
    /// <summary>
    /// Shared data container used by ECS systems.
    /// Holds business configuration data and current input states.
    /// </summary>
    public class SharedData {
        public BusinessConfig[] BusinessConfigs; // Array of business configurations
        public SharedInputData Inputs;            // Input data shared across systems
    }
}