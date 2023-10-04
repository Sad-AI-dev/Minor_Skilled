using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    public class EnemySpawnerAttacher : MonoBehaviour
    {
        private void Start()
        {
            EnemySpawner.instance.spawner = GetComponent<ObjectSpawner>();
        }
    }
}
