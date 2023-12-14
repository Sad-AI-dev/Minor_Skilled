using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    public class EnemySpawner : MonoBehaviour
    {
        [System.Serializable]
        public struct EnemySpawnData
        {
            public EnemySpawnData(GameObject prefab, bool isFlying = false)
            {
                this.prefab = prefab;
                this.isFlying = isFlying;
            }

            public GameObject prefab;
            public bool isFlying;
        }

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

        public EnemySpawnPoolSO spawnPool;

        [Header("Spawn Frequency Settings")]
        public float spawnFrequency = 1f;

        [Header("Batching Settings")]
        public int minBatchSize = 2;
        public int maxBatchSize = 5;

        [Header("Spawn Queue Settings")]
        public float minDelay = 0.05f;
        public float maxDelay = 0.2f;

        [Header("Prewarm Settings")]
        public float prewarmMultiplier = 2f;

        [Header("Flying Enemy Settings")]
        public float minHeight = 10f;
        public float maxHeight = 30f;

        //refs
        [HideInInspector] public EnemyPlacer placer;
        private CostBasedActivator activator;
        private GameStateManager gameState;

        //vars
        private Queue<EnemySpawnData> spawnQueue;
        private Coroutine placeEnemyRoutine;

        //=========== Initialize ===========
        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            gameState = GameStateManager.instance;
            activator = GetComponent<CostBasedActivator>();
            EventBus<SceneLoadedEvent>.AddListener(OnSceneLoaded);
            //initialize Queue
            spawnQueue = new Queue<EnemySpawnData>();
            //spawn wave
            SpawnEnemies();
        }

        //========================== Spawn Enemy Cycle ==============================
        private void SpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCo());
        }

        private IEnumerator SpawnEnemiesCo()
        {
            yield return new WaitForSeconds(spawnFrequency);
            if (!gameState.scalingIsPaused)
            {
                activator.Activate();
                placeEnemyRoutine ??= StartCoroutine(PlaceEnemiesCo());
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
                placeEnemyRoutine ??= StartCoroutine(PlaceEnemiesCo());
                placer.ignoreRings = false;
            }
        }

        //========= Spawn Enemy Wrapper ===========
        public void SpawnEnemy(int category)
        {
            EnemySpawnData data = spawnPool.categories[category].contents.GetRandomEntry();
            spawnQueue.Enqueue(data);
        }

        //=============================== Enemy Spawn Queue Handling =======================================
        private IEnumerator PlaceEnemiesCo()
        {
            while (spawnQueue.Count > 0)
            {
                if (placer) 
                {
                    //batch spawns
                    for (int i = 0; i < GetBatchSize(); i++)
                    {
                        PlaceEnemy(spawnQueue.Dequeue());
                    }
                }
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }
            placeEnemyRoutine = null;
        }

        private int GetBatchSize()
        {
            return Mathf.Min(spawnQueue.Count, Random.Range(minBatchSize, maxBatchSize));
        }
        private void PlaceEnemy(EnemySpawnData data)
        {
            GameObject enemy = placer.SpawnEnemy(data.prefab);
            if (data.isFlying)
            {
                enemy.transform.position = enemy.transform.position + (Vector3.up * Random.Range(minHeight, maxHeight));
            }
        }

        //=================== Spawn Rate Handling ====================
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
            //empty queue
            spawnQueue.Clear();
            //start prewarm
            StartCoroutine(PrewarmCo());
        }

        //============ Handle Destroy ===============
        private void OnDestroy()
        {
            EventBus<SceneLoadedEvent>.RemoveListener(OnSceneLoaded);
        }
    }
}
