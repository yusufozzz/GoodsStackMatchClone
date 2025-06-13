using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

namespace Utility
{
    public abstract class ScriptableSingletonMono<T> : ScriptableObject where T : ScriptableSingletonMono<T>
    {
        private static T _instance;
        private static AsyncOperationHandle<T> _handle;

        public static T Instance 
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"[{typeof(T).Name}] is not loaded yet. Use `await LoadAsync()` before accessing.");
                }
                return _instance;
            }
        }

        /// <summary>
        /// Loads the singleton asset via Addressables.
        /// Make sure the asset name and type match.
        /// </summary>
        public static async Task<T> LoadAsync()
        {
            if (_instance != null)
                return _instance;

            string address = typeof(T).Name;
            _handle = Addressables.LoadAssetAsync<T>(address);
            
            try
            {
                await _handle.Task;

                if (_handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _instance = _handle.Result;
                    return _instance;
                }
                else
                {
                    Debug.LogError($"[ScriptableSingletonMono] Failed to load {typeof(T).Name} via Addressables. Status: {_handle.Status}");
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[ScriptableSingletonMono] Exception while loading {typeof(T).Name}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Releases the Addressable handle when no longer needed.
        /// Call this when the game is shutting down or the asset is no longer needed.
        /// </summary>
        public static void Release()
        {
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }
            _instance = null;
        }

        /// <summary>
        /// Check if the singleton is loaded without triggering an error.
        /// </summary>
        public static bool IsLoaded => _instance != null;

        protected virtual void OnDestroy()
        {
            // Automatically release when the ScriptableObject is destroyed
            Release();
        }
    }
}