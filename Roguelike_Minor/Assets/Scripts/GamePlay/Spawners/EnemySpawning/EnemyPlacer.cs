using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Util;

namespace Game {
    public class EnemyPlacer : MonoBehaviour
    {
        [Header("Spawn Ranges")]
        public float innerRingMin = 10f;
        public float innerRingSize = 5f;
        public float outerRingSize = 30f;

        [Header("Distribution Settings")]
        [SerializeField] [Range(0f, 1f)] private float outerRingChance = 0.2f;

        //vars
        [HideInInspector] public bool ignoreRings = false;
        private Transform player;

        private void Start()
        {
            //setup placer
            EnemySpawner.instance.placer = this;
            player = GameStateManager.instance.player.transform;
        }

        //============== Spawn Enemy =============
        public GameObject SpawnEnemy(GameObject prefab)
        {
            GameObject enemy = Instantiate(prefab);
            Vector3 spawnPos = GetSpawnPos();
            if (enemy.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                navMeshAgent.Warp(spawnPos);
            }
            else { enemy.transform.position = spawnPos; }
            return enemy;
        }

        private Vector3 GetSpawnPos()
        {
            bool success = false;
            Vector3 pos = new Vector3();

            while (!success)
            {
                success = NavMeshUtil.RandomNavmeshLocationAtDistance(
                    out Vector3 position, player.position, GetRandomRadius()
                );
                pos = position;
            }
            return pos;
        }

        private float GetRandomRadius()
        {
            float rand = Random.Range(0, 1);
            if (rand < outerRingChance || ignoreRings) 
            {
                return Random.Range(
                    innerRingMin + innerRingSize,
                    innerRingMin + innerRingSize + outerRingSize
                );
            }
            else { return Random.Range(innerRingMin, innerRingMin + innerRingSize); }
        }
    }
}
