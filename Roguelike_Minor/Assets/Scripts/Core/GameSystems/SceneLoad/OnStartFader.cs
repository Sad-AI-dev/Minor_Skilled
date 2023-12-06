using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (ShouldFade()) 
            {
                uiFader.StartFade();
            }
        }
        private bool ShouldFade()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).buildIndex > 1) //dont fade on Main Menu = 0 and Dont Destroy = 1
                {
                    return true;
                }
            }
            return false;
        }

        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
