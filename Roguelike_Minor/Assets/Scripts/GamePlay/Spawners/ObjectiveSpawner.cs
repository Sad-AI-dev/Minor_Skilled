using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.GameSystems;

namespace Game {
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
