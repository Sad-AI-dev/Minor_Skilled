using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core.GameSystems {
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private UIFader uiFader;
        [HideInInspector] public bool allowLoadScene;

        private void Start()
        {
            //setup fader
            if (uiFader)
            {
                uiFader.onFadeEnd.AddListener(() => allowLoadScene = true);
            }
        }

        //========= Load Scene =============
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(sceneName)));
        }

        public void LoadScene(int buildIndex)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(buildIndex)));
        }

        public void FastLoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        //========= Load Scene Relative ===========
        public void LoadSceneRelative(int relativeIndex)
        {
            int indexToLoad = SceneManager.GetActiveScene().buildIndex + relativeIndex;
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(indexToLoad)));
        }

        //========= Load Scene Additive ============
        public void LoadSceneAdditive(string sceneName)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)));
        }

        public void LoadSceneAdditive(int buildIndex)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive)));
        }

        //========= Load Relative Scene Additive ==========
        public void LoadRelativeSceneAdditive(int relativeIndex)
        {
            int indexToLoad = SceneManager.GetActiveScene().buildIndex + relativeIndex;
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(indexToLoad, LoadSceneMode.Additive)));
        }

        //=========== Load Async ==========
        private IEnumerator LoadSceneCo(AsyncOperation asyncOperation)
        {
            asyncOperation.allowSceneActivation = false;
            //setup UI fader
            if (uiFader)
            {
                uiFader.StartFade();
                allowLoadScene = false;
            }
            else { allowLoadScene = true; }
            //load scene
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f && allowLoadScene)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        //========== Manage AllowLoad ==========
        public void SetAllowLoad(bool value)
        {
            allowLoadScene = value;
        }

        //========== Quit ===========
        public void Quit()
        {
            Application.Quit();
        }
    }
}
