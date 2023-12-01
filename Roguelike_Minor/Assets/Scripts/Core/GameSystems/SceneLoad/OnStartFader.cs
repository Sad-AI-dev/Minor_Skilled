using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    public class OnStartFader : MonoBehaviour
    {
        [SerializeField] private UIFader uiFader;

        private void Awake()
        {
            EventBus<SceneLoadedEvent>.AddListener(OnSceneLoaded);
        }

        private void OnSceneLoaded(SceneLoadedEvent eventData)
        {
            uiFader.targetGroup.alpha = 1f;
            uiFader.StartFade();
        }

        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
