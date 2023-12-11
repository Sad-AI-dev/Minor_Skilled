using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Util;

namespace Game {
    public class AdvanceObjectSpawner : MonoBehaviour
    {
        public GameObject advanceObjectPrefab;

        [Header("Distance Settings")]
        public float minDistance = 15f;
        public float range = 15f;

        private void Start()
        {
            GameStateManager.instance.advanceObjectSpawner = this;
        }

        public void SpawnAdvanceObject(GameObject advanceObject)
        {
            NavMeshUtil.RandomNavmeshLocationAtDistance(
                out Vector3 spawnPos,
                GameStateManager.instance.player.transform.position,
                Random.Range(minDistance, minDistance + range)
            );
            advanceObject.transform.position = spawnPos;
        }
    }
}
