using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyRegister : MonoBehaviour
{
    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else { instance = this; }
    }
    public static DontDestroyRegister instance;

    private readonly List<GameObject> dontDestroyRegister = new();

    public void RegisterObject(GameObject obj)
    {
        dontDestroyRegister.Add(obj);
        DontDestroyOnLoad(obj);
    }

    public void UnloadDontDestroy()
    {
        foreach (GameObject obj in dontDestroyRegister) {
            Destroy(obj);
        }
    }
}
