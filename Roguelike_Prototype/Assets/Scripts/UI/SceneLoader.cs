using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //========= static load ============
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //========= relative load ============
    public void LoadSceneRelative(int relativeID)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + relativeID);
    }
    public void LoadRelativeSceneAdditive(int relativeID)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + relativeID, LoadSceneMode.Additive);
    }

    //======== Additive Load ============
    public void LoadAdditiveScene(int id)
    {
        SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
    }
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    //=========== Static unload =============
    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
