using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;

    public bool Pause = false;

    private void Start()
    {
        if (!instance) instance = this;
        else Destroy(this.gameObject);
    }
}
