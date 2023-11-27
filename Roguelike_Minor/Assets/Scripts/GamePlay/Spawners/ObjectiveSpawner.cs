using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game {
    [RequireComponent(typeof(ObjectSpawner))]
    public class ObjectiveSpawner : MonoBehaviour
    {
        private ObjectSpawner spawner;

        private void Awake()
        {
            spawner = GetComponent<ObjectSpawner>();
            EventBus<ObjectiveSpawned>.AddListener(HandleObjectiveSpawned);
        }

        private void Start()
        {
            EventBus<SpawnObjectiveEvent>.Invoke(new SpawnObjectiveEvent());
        }

        private void HandleObjectiveSpawned(ObjectiveSpawned eventData)
        {
            spawner.PlaceObject(eventData.objective);
        }

        private void OnDestroy()
        {
            EventBus<ObjectiveSpawned>.RemoveListener(HandleObjectiveSpawned);
        }
    }
}
