using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems {
    [CreateAssetMenu(fileName = "Objective", menuName = "ScriptableObjects/Objective")]
    public class ObjectiveDataSO : ScriptableObject
    {
        [Header("UI Settings")]
        public string displayName;

        [Header("GamePlay Settings")]
        public List<GameObject> stepPrefabs;

        public ObjectiveStep ActivateStep(int index)
        {
            if (stepPrefabs != null && stepPrefabs.Count > index)
            {
                return InitializeStep(stepPrefabs[index]);
            }
            else return null;
        }

        private ObjectiveStep InitializeStep(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            if (obj.TryGetComponent(out ObjectiveStep step))
            {
                return step;
            }
            else
            {
                Debug.LogWarning($"objective step script not found on {obj.name}, searching children...");
                return obj.GetComponentInChildren<ObjectiveStep>();
            }
        }
    }
}
