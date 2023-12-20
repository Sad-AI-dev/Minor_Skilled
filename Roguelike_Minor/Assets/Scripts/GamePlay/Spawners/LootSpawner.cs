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
            obj.transform.position = GetLootPosition();
        }

        public void PlaceObject(GameObject obj)
        {
            obj.transform.position = GetLootPosition();
        }

        private Vector3 GetLootPosition()
        {
            bool success = false;
            Vector3 pos = new Vector3();

            while (!success)
            {
                success = NavMeshUtil.RandomNavmeshLocationAtDistance(
                    out Vector3 position, transform.position, Random.Range(0, stageRange)
                );
                pos = position;
            }
            return pos;
        }
    }
}
