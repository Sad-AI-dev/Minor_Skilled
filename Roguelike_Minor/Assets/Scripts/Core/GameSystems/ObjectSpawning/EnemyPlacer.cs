using Game.Core.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Core.GameSystems {
    public class EnemyPlacer : MonoBehaviour
    {
        //vars
        private NavMeshTriangulation triangulation;
        private EntropyRandom<int> vertexIndices;

        private void Start()
        {
            triangulation = NavMesh.CalculateTriangulation();
            //create entropy random
            List<int> vertices = new List<int>();
            for (int i = 0; i < triangulation.vertices.Length; i++) { vertices.Add(i); }
            vertexIndices = new EntropyRandom<int>(vertices);
            //setup placer
            EnemySpawner.instance.placer = this;
        }

        //============== Spawn Enemy =============
        public void SpawnEnemy(GameObject prefab)
        {
            //place enemy on navmesh
            if (NavMesh.SamplePosition(GetRandomSpawnPoint(), out NavMeshHit hit, 2f, -1))
            {
                GameObject enemy = Instantiate(prefab);
                enemy.GetComponent<NavMeshAgent>().Warp(hit.position);
            }
        }

        //========== Util funcs ===============
        private Vector3 GetRandomSpawnPoint()
        {
            return triangulation.vertices[vertexIndices.Next()];
        }
    }
}
