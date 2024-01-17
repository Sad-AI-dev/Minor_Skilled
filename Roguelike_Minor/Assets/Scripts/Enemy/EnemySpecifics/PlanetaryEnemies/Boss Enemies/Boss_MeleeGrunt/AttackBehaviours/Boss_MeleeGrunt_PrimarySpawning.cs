using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game.Enemy
{
    public class Boss_MeleeGrunt_PrimarySpawning : MonoBehaviour
    {
        public GameObject prefab;
        public List<Transform> spawnLocations = new List<Transform>();
        Ability source;

        private void Start()
        {
            source = transform.GetComponent<Agent>().abilities.primary;
        }

        public void SpawnObjects()
        {
            foreach (var item in spawnLocations)
            {
                GameObject obj = Instantiate(prefab, item.position, item.rotation);
                obj.GetComponent<Boss_MeleeGrunt_PrimaryBehaviour>().source = source;
            }
        }
    }
}
