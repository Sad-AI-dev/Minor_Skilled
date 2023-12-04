using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;
using Game.Util;

namespace Game {
    public class LootSpawner : MonoBehaviour
    {
        public float stageRange;

        private void Start()
        {
            GameStateManager.instance.lootSpawner = this;
        }

        public void PlaceLoot(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = NavMeshUtil.RandomNavmeshLocationAtDistance(
                transform.position, Random.Range(0, stageRange)
            );
        }
    }
}
