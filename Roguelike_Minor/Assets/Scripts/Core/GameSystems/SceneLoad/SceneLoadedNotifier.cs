using System.Collections;
using System.Collections.Generic;
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
            EventBus<SceneLoadedEvent>.Invoke(new SceneLoadedEvent());
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= InvokeSceneLoaded;
        }
    }
}
