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
        [HideInInspector] public bool paused = false;

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
        private IEnumerator PrewarmCo()
        {
            yield return null; //wait a frame
            if (placer)
            {
                placer.ignoreRings = true;
                activator.ForceActivate(prewarmMultiplier);
                placer.ignoreRings = false;
            }
        }

        //========= Spawn Enemy Wrapper ===========
        public void SpawnEnemy(GameObject prefab)
        {
            if (placer)
            {
                placer.SpawnEnemy(prefab);
            }
        }

        //========= Enemy Spawn Rate Wrapper =========
        public void SetExternalSpawnMultiplier(float toAdd)
        {
            activator.externalMultiplier += toAdd;
        }

        public void ForceSpawn(float multiplier)
        {
            activator.ForceActivate(multiplier);
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
            StartCoroutine(PrewarmCo());
        }

        //============ Handle Destroy ===============
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
