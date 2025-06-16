using Data;
using Leopotam.Ecs;

namespace Systems {
    /// <summary>
    /// System responsible for clearing input data each frame to ensure fresh input handling.
    /// This prevents stale input states from affecting game logic.
    /// </summary>
    public class InputResetSystem : IEcsRunSystem {
        private SharedData _shared;
    
        /// <summary>
        /// Clears the shared input data container on every ECS run cycle.
        /// </summary>
        public void Run() {
            _shared.Inputs.Clear();
        }
    }
}