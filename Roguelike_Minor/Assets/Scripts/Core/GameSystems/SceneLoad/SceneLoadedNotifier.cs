using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.GameSystems {
    public class SceneLoadedNotifier : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += InvokeSceneLoaded;
        }

        private void InvokeSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Additive) //don't invoke if additive was loaded
            {
                EventBus<SceneLoadedEvent>.Invoke(new SceneLoadedEvent());
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= InvokeSceneLoaded;
        }
    }
}
