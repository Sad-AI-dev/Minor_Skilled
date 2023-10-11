using Game.Core.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    public class ObjectSpawner : MonoBehaviour
    {
        public List<Transform> spawnPoints;
        private EntropyRandom<Transform> randomizer;

        private void Awake()
        {
            if (spawnPoints == null || spawnPoints.Count == 0) {
                spawnPoints = new List<Transform>();
                foreach (Transform child in transform) {
                    spawnPoints.Add(child);
                }
            }
            randomizer = new EntropyRandom<Transform>(spawnPoints);
        }

        //============ Spawn Object ==============
        public void SpawnObject(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetPositionAndRotation(GetRandomSpawnPoint(), GetRandomYRotation());
        }

        private Vector3 GetRandomSpawnPoint()
        {
            return randomizer.Next().position;
        }
        private Quaternion GetRandomYRotation()
        {
            return Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}
