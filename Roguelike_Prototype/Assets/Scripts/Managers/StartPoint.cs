using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.spawnPoint = transform;
        GameManager.instance.player.transform.position = transform.position;
    }
}
