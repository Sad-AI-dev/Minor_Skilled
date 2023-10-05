using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems {
    [RequireComponent(typeof(ObjectSpawner))]
    public class ObjectiveSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objectives;

        private void Start()
        {
            GetComponent<ObjectSpawner>().SpawnObject(objectives[Random.Range(0, objectives.Count)]);
        }
    }
}
