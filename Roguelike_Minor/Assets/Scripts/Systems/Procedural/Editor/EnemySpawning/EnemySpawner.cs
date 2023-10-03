using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems
{
    public class EnemySpawner : MonoBehaviour
    {
        //======= Singleton ========
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                activator = GetComponent<CostBasedActivator>();
                SpawnEnemies();
            }
        }

        public static EnemySpawner instance;

        //vars
        private CostBasedActivator activator;
        private bool paused = false;

        private void SpawnEnemies()
        {

        }
    }
}
