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
        private GameStateManager gameState;

        //=========== Initialize ===========
        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            gameState = GameStateManager.instance;
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
            if (!gameState.scalingIsPaused)
            {
                activator.Activate();
            }
            else
            { //not active, wait for reactivation
                while (gameState.scalingIsPaused)
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

        //=========== Handle Scene Loaded ===============
        private void OnSceneLoaded(SceneLoadedEvent data)
        {
            StartCoroutine(PrewarmCo());
        }

        //============ Handle Destroy ===============
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
