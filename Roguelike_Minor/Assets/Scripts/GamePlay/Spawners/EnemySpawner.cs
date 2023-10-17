using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
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
            }
        }

        public static EnemySpawner instance;

        [Header("Spawn Settings")]
        public float spawnDelay = 1f;

        [Header("Prewarm Settings")]
        public float prewarmMultiplier = 2f;

        //vars
        [HideInInspector] public EnemyPlacer placer;
        private CostBasedActivator activator;
        private bool paused = false;

        //=========== Initialize ===========
        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            activator = GetComponent<CostBasedActivator>();
            SpawnEnemies();
            EventBus<SceneLoadedEvent>.AddListener(OnSceneLoaded);
        }

        //=========== Spawn Enemy Cycle ==========
        private void SpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCo());
        }

        private IEnumerator SpawnEnemiesCo()
        {
            yield return new WaitForSeconds(spawnDelay);
            if (!paused)
            {
                activator.Activate();
            }
            else
            { //not active, wait for reactivation
                while (paused)
                {
                    yield return null;
                }
            }
            SpawnEnemies(); //loop
        }

        //========= Prewarm Stage ======
        private void Prewarm()
        {
            activator.externalMultiplier += prewarmMultiplier;
            activator.Activate(); //activate twice (activator gains budget after puchasing)
            activator.externalMultiplier -= prewarmMultiplier;
            activator.Activate();
        }

        //========= Spawn Enemy Wrapper ===========
        public void SpawnEnemy(GameObject prefab)
        {
            if (placer)
            {
                placer.SpawnEnemy(prefab);
            }
        }

        //=========== State Management ============
        public void SetSpawnState(bool state)
        {
            paused = state;
        }

        //=========== Handle Scene Loaded ===============
        private void OnSceneLoaded(SceneLoadedEvent data)
        {
            paused = false;
            Prewarm();
            Debug.Log("prewarmed stage");
        }

        //============ Handle Destroy ===============
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
