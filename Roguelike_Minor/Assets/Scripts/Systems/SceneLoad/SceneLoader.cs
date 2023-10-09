using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems {
    public class SceneLoader : MonoBehaviour
    {
        //========= Load Scene =============
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(sceneName)));
        }

        public void LoadScene(int buildIndex)
        {
            StartCoroutine(LoadSceneCo(SceneManager.LoadSceneAsync(buildIndex)));
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
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }

        //========== Quit ===========
        public void Quit()
        {
            Application.Quit();
        }
    }
}
