using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.GameSystems {
    public class SceneLoadedNotifier : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += InvokeSceneLoaded;
            SceneManager.sceneUnloaded += InvokeSceneUnloaded;
        }

        private void InvokeSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EventBus<SceneLoadedEvent>.Invoke(new SceneLoadedEvent { loadedIndex = scene.buildIndex });
        }

        private void InvokeSceneUnloaded(Scene scene)
        {
            EventBus<SceneUnloadedEvent>.Invoke(new SceneUnloadedEvent { 
                unloadedIndex = scene.buildIndex, 
                name = scene.name 
            });
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= InvokeSceneLoaded;
            SceneManager.sceneUnloaded -= InvokeSceneUnloaded;
        }
    }
}
