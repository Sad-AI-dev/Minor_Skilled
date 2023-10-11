using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    [RequireComponent(typeof(ObjectSpawner))]
    public class AdvanceObjectSpawner : MonoBehaviour
    {
        public GameObject advanceObjectPrefab;

        private void Start()
        {
            GameStateManager.instance.advanceObjectSpawner = this;
        }

        public void SpawnAdvanceObject()
        {
            GetComponent<ObjectSpawner>().SpawnObject(advanceObjectPrefab);
        }
    }
}
