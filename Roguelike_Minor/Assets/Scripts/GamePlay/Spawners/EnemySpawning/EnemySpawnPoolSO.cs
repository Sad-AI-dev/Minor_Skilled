using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "SpawnPool", menuName = "ScriptableObjects/Enemy Spawn Pool")]
    public class EnemySpawnPoolSO : ScriptableObject
    {
        [System.Serializable]
        public class EnemyPool
        {
            public string name;
            public WeightedChance<EnemySpawner.EnemySpawnData> contents;
        }

        public List<EnemyPool> categories;
    }
}
