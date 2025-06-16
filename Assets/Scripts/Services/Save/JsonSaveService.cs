using System.IO;
using Data.Save;
using UnityEngine;

namespace Services.Save {
    /// <summary>
    /// Static service class for saving and loading game data using JSON format.
    /// Handles file IO operations for persistent game state storage.
    /// </summary>
    public static class JsonSaveService {
        /// <summary>
        /// Full path to the save file within the persistent data path.
        /// </summary>
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        /// <summary>
        /// Saves the provided game data as a formatted JSON file on disk.
        /// Creates the save directory if it does not exist.
        /// </summary>
        /// <param name="data">Game data to save.</param>
        public static void Save(GameSaveData data) {
            try {
                var directory = Path.GetDirectoryName(SavePath);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonUtility.ToJson(data, true);
                Debug.Log("[JsonSaveService] Json to save: " + json);

                File.WriteAllText(SavePath, json);
                Debug.Log("[JsonSaveService] Game saved to: " + SavePath);
            } catch (System.Exception ex) {
                Debug.LogError("[JsonSaveService] Failed to save: " + ex.Message);
            }
        }

        /// <summary>
        /// Loads game data from the JSON save file if it exists.
        /// Returns null if no save file is found or if loading fails.
        /// </summary>
        /// <returns>Deserialized game save data or null.</returns>
        public static GameSaveData Load() {
            try {
                if (!File.Exists(SavePath)) {
                    Debug.LogWarning("[JsonSaveService] Save file not found at: " + SavePath);
                    return null;
                }

                var json = File.ReadAllText(SavePath);
                Debug.Log("[JsonSaveService] Save loaded from: " + SavePath);
                return JsonUtility.FromJson<GameSaveData>(json);
            } catch (System.Exception ex) {
                Debug.LogError("[JsonSaveService] Failed to load: " + ex.Message);
                return null;
            }
        }
    }
}