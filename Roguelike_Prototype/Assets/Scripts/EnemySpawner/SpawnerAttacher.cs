using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAttacher : MonoBehaviour
{
    private void Start()
    {
        ObjectSpawner spawner = GetComponent<ObjectSpawner>();
        GameManager.instance.enemySpawner = spawner;
    }
}
