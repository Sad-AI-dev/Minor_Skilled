using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUIHandler : MonoBehaviour
{
    [Header("Refs")]
    public GameObject gameplayUI;

    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }

    public void Die()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        gameplayUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        GameManager.instance.camInputManager.LockCamera();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        gameplayUI.SetActive(true);
        GameManager.instance.camInputManager.UnLockCamera();
        //unload dontdestroyonload objects
        DontDestroyRegister.instance.UnloadDontDestroy();
        //start new run
        sceneLoader.LoadScene(0);
    }
}
